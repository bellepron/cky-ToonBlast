using cky.Enums;

namespace cky.Structs
{
    [System.Serializable]
    public struct CreateItemData
    {
        public GridData gridData;
        public ItemType itemType;

        public CreateItemData(GridData gridData, ItemType itemType)
        {
            this.gridData = new GridData(gridData.i, gridData.j);
            this.itemType = itemType;
        }
    }
}