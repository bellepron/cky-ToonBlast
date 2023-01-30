using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using cky.Structs;
using cky.Enums;

[CreateAssetMenu(menuName = "Scriptable Objects/Level/Level Settings")]
public class LevelSettings : ScriptableObject
{
    [Header("Board Size")]
    public int width = 9;
    public int height = 9;

    public float increaseConstant = 1.0f;

    [Header("Matching")]
    public int moves = 10;
    public MinMatchData minMatchData;
    public MatchingData matchingData = new MatchingData(1.1f, 0.35f, 0.15f, 0.15f);

    [Header("Falling&Filling")]
    public FallData fallingData = new FallData(Ease.InSine, 0.05f, 0.01f);

    public Vector2 StartPosition
    {
        get
        {
            return new Vector2((increaseConstant - width) * 0.5f,
                                    (increaseConstant - height) * 0.5f);
        }
    }

    [Header("Quest Data")]
    public List<QuestData> questDatas = new List<QuestData>();

    [Header("Items")]
    [SerializeField]
    private ItemType[] ItemsToCome = new[] { ItemType.GreenCube,
                                                  ItemType.YellowCube,
                                                  ItemType.BlueCube,
                                                  ItemType.RedCube };

    private GameObject[,] _gridData;
    public List<CreateItemData> adjustedItemData = new List<CreateItemData>();
    public GameObject[,] GridData { get { return _gridData; } set { _gridData = value; } }

    public ItemType GetRandomCubeItemType()
    {
        return ItemsToCome[Random.Range(0, ItemsToCome.Length)];
    }
}