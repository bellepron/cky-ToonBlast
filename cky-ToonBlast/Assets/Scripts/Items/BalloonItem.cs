using cky.Interfaces;
using cky.Structs;

namespace cky.Items
{
    public class BalloonItem : ItemAbstract, IClickable, IFallable
    {
        public void Execute() { }

        public override void CombinedExecute(ItemAbstract clickedItemAbstract, MatchingData matchingData)
        {
            base.CombinedExecute(clickedItemAbstract, matchingData);

            Destroy();
        }
    }
}