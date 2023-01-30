using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using cky.Interfaces;
using cky.Structs;
using cky.Enums;
using cky.Items;

namespace cky.Managers
{
    public class HintManager : Singleton<HintManager>
    {
        #region Variables

        private LevelSettings _levelSettings;
        private bool[,] _visitedCells;
        int _width, _height;
        MinMatchData _minMatchData;

        #endregion

        #region Preparing

        private void Start()
        {
            _levelSettings = LevelManager.Instance.levelSettings;
            _visitedCells = new bool[_levelSettings.width, _levelSettings.height];
            _width = _levelSettings.width;
            _height = _levelSettings.height;
            _minMatchData = _levelSettings.minMatchData;

            ControlHints();

            EventManager.CheckHints += ControlHints;
        }

        private void OnDisable() => EventManager.CheckHints += ControlHints;

        #endregion

        #region Control Hints

        public async void ControlHints()
        {
            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    if (_visitedCells[i, j] == true) { continue; }
                    if (_levelSettings.GridData[i, j] == null) { _visitedCells[i, j] = true; continue; }

                    if (_levelSettings.GridData[i, j].TryGetComponent<ItemAbstract>(out var itemAbstract) == true)
                    {
                        if (itemAbstract.IsMatchable() == false) { _visitedCells[i, j] = true; continue; }

                        var neighbours = new List<ItemAbstract>();
                        await FindAllNeighbours(itemAbstract, neighbours);

                        ChangeSprites(neighbours);
                    }
                }
            }
        }

        #endregion

        #region Finding Neighbours

        private async Task FindAllNeighbours(ItemAbstract clickedItemAbstract, List<ItemAbstract> neighbours)
        {
            CheckItemNeighbours(clickedItemAbstract, neighbours);

            await Task.Yield();
        }

        public void CheckItemNeighbours(ItemAbstract clickedItemAbstract, List<ItemAbstract> neighbours)
        {
            var i = clickedItemAbstract.Grid.i;
            var j = clickedItemAbstract.Grid.j;

            if (_visitedCells[i, j] == true) { Debug.Log("Already added"); return; }
            _visitedCells[i, j] = true;

            if (clickedItemAbstract.IsMatchable() == false)
            {
                Debug.Log("Item is not matchable!");
                _visitedCells[i, j] = true;

                return;
            }

            var matchType = clickedItemAbstract.GetMatchType();

            if (neighbours.Contains(clickedItemAbstract) == false)
                neighbours.Add(clickedItemAbstract);

            NeighboursData neighboursData = CheckAllNeighbours(i, j, matchType, neighbours);

            if (HasNeighbour(neighboursData) == true)
            {
                FindNeighboursNeighbours(neighboursData, neighbours);
            }
        }

        private NeighboursData CheckAllNeighbours(int i, int j, MatchType matchType, List<ItemAbstract> neighbours)
        {
            return new NeighboursData(CheckNeighbour(i, j + 1, matchType, neighbours),  // Up
                                      CheckNeighbour(i, j - 1, matchType, neighbours),  // Down
                                      CheckNeighbour(i + 1, j, matchType, neighbours),  // Right
                                      CheckNeighbour(i - 1, j, matchType, neighbours)); // Left
        }

        private ItemAbstract CheckNeighbour(int i, int j, MatchType matchType, List<ItemAbstract> neighbours)
        {
            if (IsCheckable(i, j) == false) return null;

            var item = _levelSettings.GridData[i, j];
            var itemAbstractToCheck = item.GetComponent<ItemAbstract>();
            MatchType itemMatchTypeToCheck = itemAbstractToCheck.GetMatchType();

            if (matchType == itemMatchTypeToCheck)
            {
                if (neighbours.Contains(itemAbstractToCheck) == false)
                {
                    neighbours.Add(itemAbstractToCheck);

                    return itemAbstractToCheck;
                }
            }

            return null;
        }

        private void FindNeighboursNeighbours(NeighboursData nData, List<ItemAbstract> neighbours)
        {
            if (nData.up != null) CheckItemNeighbours(nData.up, neighbours);
            if (nData.down != null) CheckItemNeighbours(nData.down, neighbours);
            if (nData.right != null) CheckItemNeighbours(nData.right, neighbours);
            if (nData.left != null) CheckItemNeighbours(nData.left, neighbours);
        }

        #endregion

        #region Result - Changing Sprites

        private void ChangeSprites(List<ItemAbstract> neighbours)
        {
            var allNeighboursCount = neighbours.Count;

            for (int i = 0; i < allNeighboursCount; i++)
            {
                if (neighbours[i] == null) continue; // TODO:

                if (neighbours[i].TryGetComponent<ISpriteChanger>(out var spriteChanger) == true)
                    spriteChanger.SetSpriteHint(allNeighboursCount, _minMatchData);
            }

            ResetNeighbours(neighbours);
        }

        #endregion

        #region Coditions

        private bool HasNeighbour(NeighboursData nData)
        {
            if (nData.up || nData.down || nData.right || nData.left)
                return true;

            return false;
        }

        private bool IsCheckable(int i, int j)
        {
            if (i >= _levelSettings.width || i < 0 ||
                j >= _levelSettings.height || j < 0)
                return false;

            if (_visitedCells[i, j] == true)
                return false;

            if (_levelSettings.GridData[i, j] == null)
                return false;

            return true;
        }

        #endregion

        #region Reset

        private void ResetNeighbours(List<ItemAbstract> neighbours)
        {
            neighbours.Clear();
            neighbours = new List<ItemAbstract>();

            ResetVisiteds();
        }
        private void ResetVisiteds()
        {
            for (int j = 0; j < _visitedCells.GetLength(1); j++)
                for (int i = 0; i < _visitedCells.GetLength(0); i++)
                    _visitedCells[i, j] = false;
        }

        #endregion
    }
}