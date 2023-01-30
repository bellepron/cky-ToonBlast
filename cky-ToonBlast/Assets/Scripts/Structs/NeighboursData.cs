using cky.Items;

namespace cky.Structs
{
    public struct NeighboursData
    {
        public ItemAbstract up, down, right, left;
        public NeighboursData(ItemAbstract up, ItemAbstract down, ItemAbstract right, ItemAbstract left)
        {
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
        }
    }
}