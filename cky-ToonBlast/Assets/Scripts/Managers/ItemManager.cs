using UnityEngine;
using cky.Structs;
using cky.Helpers;
using cky.Enums;
using cky.Items;
using System;

namespace cky.Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        LevelSettings _levelSettings;
        [SerializeField] GameObject itemPrefab;

        private void Start() => _levelSettings = LevelManager.Instance.levelSettings;

        #region Creating

        public void CreateItemAtGrid(ItemType itemType, GridData gridData)
        {
            var pos = PositionHelper.CalculatePosition(gridData);

            CreateItem(itemType, gridData, pos);
        }

        private string GetItemScript(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.GreenCube:
                    return ItemScriptNames.CUBE_ITEM;
                case ItemType.YellowCube:
                    return ItemScriptNames.CUBE_ITEM;
                case ItemType.BlueCube:
                    return ItemScriptNames.CUBE_ITEM;
                case ItemType.RedCube:
                    return ItemScriptNames.CUBE_ITEM;

                case ItemType.Balloon:
                    return ItemScriptNames.BALLOON_ITEM;
                case ItemType.Crate:
                    return ItemScriptNames.CRATE_ITEM;
                case ItemType.VerticalRocket:
                    return ItemScriptNames.ROCKET_ITEM;
                case ItemType.HorizontalRocket:
                    return ItemScriptNames.ROCKET_ITEM;
                case ItemType.Bomb:
                    return ItemScriptNames.BOMB_ITEM;

                default:
                    return ItemScriptNames.CUBE_ITEM;
            }
        }

        public GameObject CreateItem(ItemType itemType, GridData gridData, Vector2 pos)
        {
            var go = Create(pos);
            var script = GetItemScript(itemType);
            var item = go.AddComponent(Type.GetType(script));
            var itemAbstract = item.GetComponent<ItemAbstract>();

            AddItemToGridData(go, gridData.i, gridData.j);

            itemAbstract.InitializeItem(itemType, gridData);

            return go;
        }

        private void AddItemToGridData(GameObject go, int i, int j)
        {
            if (_levelSettings == null) { Debug.Log("Couldn't get level settings"); return; }

            _levelSettings.GridData[i, j] = go;
        }

        private GameObject Create(Vector2 pos) => Instantiate(itemPrefab, pos, Quaternion.identity);

        #endregion

        #region Create Specials

        public void CreateRandomRocket(GridData gridData)
        {
            var randomNumber = UnityEngine.Random.Range(0, 2);

            switch (randomNumber)
            {
                case 0:
                    CreateItemAtGrid(ItemType.VerticalRocket, gridData);
                    break;
                case 1:
                    CreateItemAtGrid(ItemType.HorizontalRocket, gridData);
                    break;
            }
        }

        public void CreateBomb(GridData gridData)
        {
            CreateItemAtGrid(ItemType.Bomb, gridData);
        }

        #endregion
    }
}