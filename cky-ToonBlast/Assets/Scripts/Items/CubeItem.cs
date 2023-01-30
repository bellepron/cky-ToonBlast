using UnityEngine;
using cky.Managers;
using cky.Interfaces;
using cky.Structs;

namespace cky.Items
{
    public class CubeItem : ItemAbstract, IClickable, ISpriteChanger, IFallable
    {
        public GameObject RocketHintGO { get; set; }
        public GameObject BombHintGO { get; set; }

        void IClickable.Execute()
        {
            Debug.Log(ItemType);
        }

        protected override void SetHintObjects()
        {
            RocketHintGO = SetHintObjectAtChild(0);
            BombHintGO = SetHintObjectAtChild(1);
        }

        private GameObject SetHintObjectAtChild(int childIndex)
        {
            var go = transform.GetChild(childIndex).gameObject;
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Item");
            spriteRenderer.sortingOrder = childIndex + 1;

            if (childIndex == 0) spriteRenderer.sprite = GetSpritesForItemTypeRocketHint();
            else if (childIndex == 1) spriteRenderer.sprite = GetSpritesForItemTypeBombHint();

            go.SetActive(false);

            return go;
        }

        void ISpriteChanger.SetSpriteHint(int allNeighboursCount, MinMatchData minMatchData)
        {
            if (allNeighboursCount >= minMatchData.minMacthToBomb)
            {
                BombHint();
            }
            else if (allNeighboursCount >= minMatchData.minMacthToRocket)
            {
                RocketHint();
            }
            else
            {
                BaseHint();
            }
        }

        private Sprite GetSpritesForItemTypeRocketHint()
            => SpriteManager.Instance.GetSpritesForRocketHint(ItemType);

        private Sprite GetSpritesForItemTypeBombHint()
            => SpriteManager.Instance.GetSpritesForBombHint(ItemType);

        private void BaseHint()
        {
            RocketHintGO.SetActive(false);
            BombHintGO.SetActive(false);
        }
        private void RocketHint()
        {
            RocketHintGO.SetActive(true);
            BombHintGO.SetActive(false);
        }
        private void BombHint()
        {
            RocketHintGO.SetActive(false);
            BombHintGO.SetActive(true);
        }

        public override void CombinedExecute(ItemAbstract clickedItemAbstract, MatchingData matchingData)
        {
            base.CombinedExecute(clickedItemAbstract, matchingData);

            CombinedExecuteAnimation(clickedItemAbstract, matchingData);
        }
    }
}