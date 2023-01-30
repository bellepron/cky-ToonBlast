using UnityEngine;
using cky.Interfaces;
using cky.Structs;
using cky.Managers;

namespace cky.Items
{
    public class BombItem : ItemAbstract, IClickable, IFallable
    {
        bool _executed;

        public void Execute() { }

        public override void SingleExecute(Transform clickedItemTr, MatchingData matchingData)
        {
            if (_executed) return; _executed = true;

            SpecialEffectManager.Instance.BombExplosion(Grid);

            base.SingleExecute(clickedItemTr, matchingData);
        }

        public override void CombinedExecute(ItemAbstract clickedItemAbstract, MatchingData matchingData)
        {
            _executed = true;

            base.CombinedExecute(clickedItemAbstract, matchingData);

            CombinedExecuteAnimation(clickedItemAbstract, matchingData);
        }
    }
}