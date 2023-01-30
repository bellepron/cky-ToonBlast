using UnityEngine;
using cky.Managers;
using cky.Structs;

public class BoardCreator : MonoBehaviour
{
    LevelManager _levelManager;
    ItemManager _itemManager;

    LevelSettings _settings;
    int width, height;

    private void Start()
    {
        _levelManager = LevelManager.Instance;
        _itemManager = ItemManager.Instance;

        _settings = _levelManager.levelSettings;
        width = _settings.width;
        height = _settings.height;

        CreateBoard();
    }

    private void CreateBoard()
    {
        _levelManager.levelSettings.GridData = new GameObject[width, height];

        CreateAdjustedItems();
        CreateCells();
    }

    private void CreateAdjustedItems()
    {
        _levelManager.levelSettings.GridData = new GameObject[width, height];

        var adjustedItemCount = _settings.adjustedItemData.Count;
        for (int i = 0; i < adjustedItemCount; i++)
        {
            var item = _settings.adjustedItemData[i];

            if (item.gridData.i < width && item.gridData.j < height)
            {
                if (_settings.GridData[item.gridData.i, item.gridData.j] == null)
                {
                    _itemManager.CreateItemAtGrid(item.itemType, item.gridData);
                }
            }
        }
    }

    private void CreateCells()
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (_settings.GridData[i, j] == null)
                {
                    var itemType = _settings.GetRandomCubeItemType();

                    _itemManager.CreateItemAtGrid(itemType, new GridData(i, j));
                }
            }
        }
    }

    private void CheckGridsInGridData()
    {
        var settings = _levelManager.levelSettings;
        var width = settings.width;
        var height = settings.height;

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (settings.GridData[i, j] == null)
                {
                    var message = $"{i}:{j} is Empty";
                    Debug.Log(message);
                }
            }
        }
    }
}