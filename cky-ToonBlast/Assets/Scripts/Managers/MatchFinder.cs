using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using cky.Structs;
using cky.Enums;
using cky.Items;
using System;

namespace cky.Managers
{

    public class MatchFinder : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<ItemAbstract> _neighbours = new List<ItemAbstract>();
        private LevelSettings _levelSettings;
        private bool[,] _visitedCells;
        int _width, _height;
        MinMatchData _minMatchData;
        MatchingData _matchingData;
        float _itemDestroyAnimationTime;
        int _itemDestroyAnimationTimeMiliSecons;

        ItemAbstract _clickedItemAbsract;
        int _otherNeighboursCount;

        EventManager _eventManager;
        ItemManager _itemManager;
        Action CreateSpecial;

        #endregion

        #region Preparing

        private void Start()
        {
            _levelSettings = LevelManager.Instance.levelSettings;
            _visitedCells = new bool[_levelSettings.width, _levelSettings.height];
            _width = _levelSettings.width;
            _height = _levelSettings.height;

            _minMatchData = _levelSettings.minMatchData;
            _matchingData = _levelSettings.matchingData;
            _itemDestroyAnimationTime = _matchingData.scaleUpTime + _matchingData.scaleDownTime;
            _itemDestroyAnimationTimeMiliSecons = (int)(_itemDestroyAnimationTime * 1000);

            _eventManager = FindObjectOfType<EventManager>();
            _itemManager = ItemManager.Instance;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.ExecuteTouch += FindClickedItemNeighbours;
        }

        #endregion

        #region Fining Neighbours

        public async void FindClickedItemNeighbours(ItemAbstract clickedItemAbstract)
        {
            ResetNeighbours();

            _clickedItemAbsract = clickedItemAbstract;

            await Find(clickedItemAbstract);

            ExecuteIfPossible();
        }

        private async Task Find(ItemAbstract clickedItemAbstract)
        {
            FindNeighbours(clickedItemAbstract);
            await Task.Yield();
        }

        public void FindNeighbours(ItemAbstract clickedItemAbstract)
        {
            var i = clickedItemAbstract.Grid.i;
            var j = clickedItemAbstract.Grid.j;

            if (_visitedCells[i, j] == true) { Debug.Log("Already added"); return; }
            _visitedCells[i, j] = true;

            if (clickedItemAbstract.IsMatchable() == false)
            {
                Debug.Log("Item is not matchable!");
                clickedItemAbstract.Vibrate();

                return;
            }

            var matchType = clickedItemAbstract.GetMatchType();

            if (_neighbours.Contains(clickedItemAbstract) == false)
                _neighbours.Add(clickedItemAbstract);

            NeighboursData neighboursData = CheckAllNeighbours(i, j, matchType);

            if (HasNeighbour(neighboursData) == true)
            {
                FindNeighboursNeighbours(neighboursData);
            }
        }

        private NeighboursData CheckAllNeighbours(int i, int j, MatchType matchType)
        {
            return new NeighboursData(CheckNeighbour(i, j + 1, matchType),  // Up
                                      CheckNeighbour(i, j - 1, matchType),  // Down
                                      CheckNeighbour(i + 1, j, matchType),  // Right
                                      CheckNeighbour(i - 1, j, matchType)); // Left
        }

        private void FindNeighboursNeighbours(NeighboursData nData)
        {
            if (nData.up != null) FindNeighbours(nData.up);
            if (nData.down != null) FindNeighbours(nData.down);
            if (nData.right != null) FindNeighbours(nData.right);
            if (nData.left != null) FindNeighbours(nData.left);
        }

        private ItemAbstract CheckNeighbour(int i, int j, MatchType itemMatchType)
        {
            if (IsCheckable(i, j) == false) return null;

            var item = _levelSettings.GridData[i, j];

            if (item.TryGetComponent<ItemAbstract>(out var itemAbstractToCheck) == true)
            {
                MatchType itemMatchTypeToCheck = itemAbstractToCheck.GetMatchType();

                if (_clickedItemAbsract.GetMatchType() != MatchType.Special)
                {
                    if (itemAbstractToCheck.GetMatchType() == MatchType.None)
                    {
                        if (_neighbours.Contains(itemAbstractToCheck) == false)
                        {
                            _otherNeighboursCount++;
                            _neighbours.Add(itemAbstractToCheck);

                            return null;
                        }

                        return null;
                    }
                }

                if (itemMatchType == itemMatchTypeToCheck)
                {
                    if (_neighbours.Contains(itemAbstractToCheck) == false)
                    {
                        _neighbours.Add(itemAbstractToCheck);

                        return itemAbstractToCheck;
                    }
                }
            }

            return null;
        }

        private void ExecuteIfPossible()
        {
            var neighboursCount = _neighbours.Count;
            var matchedCubesCount = _neighbours.Count - _otherNeighboursCount;

            if (_clickedItemAbsract.GetMatchType() == MatchType.Special)
            {
                DecreaseMove();

                if (IsCombined(matchedCubesCount) == true)
                {
                    ExecuteSpecials(neighboursCount, matchedCubesCount);
                    SpecialEffectManager.Instance.ResponseByScore(_clickedItemAbsract.Grid, SpecialItemCombineScore());
                }
                else
                {
                    _clickedItemAbsract.SingleExecute(_clickedItemAbsract.transform, _matchingData);
                }

            }
            else
            {
                if (IsCombined(matchedCubesCount) == true)
                {
                    DecreaseMove();

                    ExecuteCubes(neighboursCount, matchedCubesCount);
                }
                else
                {
                    _clickedItemAbsract.Vibrate();
                }
            }

            ResetNeighbours();
        }

        #endregion

        #region Executing

        private async void ExecuteCubes(int neighboursCount, int matchedCubesCount)
        {
            PrepareSpecialItem(matchedCubesCount);

            ExecuteAll(neighboursCount);

            await WaitForItemExecuting();

            CreateSpeacialItem();
            TriggerEvents();
        }

        private void ExecuteSpecials(int neighboursCount, int matchedCubesCount)
        {
            ExecuteAll(neighboursCount);
        }

        private void ExecuteAll(int neighboursCount)
        {
            for (int i = 0; i < neighboursCount; i++)
            {
                _neighbours[i].CombinedExecute(_clickedItemAbsract, _matchingData);
            }
        }

        #endregion

        #region Reset

        private void ResetNeighbours()
        {
            _otherNeighboursCount = 0;
            _neighbours.Clear();
            _neighbours = new List<ItemAbstract>();

            ResetVisiteds();
        }

        private void ResetVisiteds()
        {
            for (int j = 0; j < _height; j++)
                for (int i = 0; i < _width; i++)
                    _visitedCells[i, j] = false;
        }

        #endregion

        #region Create Special

        private async Task WaitForItemExecuting()
        {
            await Task.Delay(_itemDestroyAnimationTimeMiliSecons);
        }

        private void TriggerEvents() => _eventManager.CheckFallsAction();

        private void CreateSpeacialItem()
        {
            CreateSpecial?.Invoke();
            CreateSpecial = null;
        }

        private void PrepareSpecialItem(int matchedCubesCount)
        {
            if (matchedCubesCount >= _minMatchData.minMacthToBomb)
            {
                CreateSpecial += CreateBomb;
            }
            else if (matchedCubesCount >= _minMatchData.minMacthToRocket)
            {
                CreateSpecial += CreateRandomRocket;
            }
        }

        private void CreateRandomRocket() =>
            _itemManager.CreateRandomRocket(_clickedItemAbsract.Grid);

        private void CreateBomb() =>
            _itemManager.CreateBomb(_clickedItemAbsract.Grid);

        #endregion

        #region Conditions

        private bool IsCombined(int matchedCubesCount) => matchedCubesCount >= _minMatchData.minToMacth;

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

        private int SpecialItemCombineScore()
        {
            var max1 = 0;
            var max2 = 0;
            var num = 0;
            foreach (var item in _neighbours)
            {
                if (item.ItemType == ItemType.VerticalRocket || item.ItemType == ItemType.HorizontalRocket) num = 1;
                if (item.ItemType == ItemType.Bomb) num = 2;

                if (num > max1) { max2 = max1; max1 = num; }
                else if (num > max2) { max2 = num; }
            }

            return max1 + max2;
        }

        #endregion

        #region Event Behaviour When Execute

        private void DecreaseMove()
        {
            _eventManager.ActivateTouchEvent(false);
            _eventManager.DecreaseRemainingMoveEvent();
        }

        #endregion
    }
}