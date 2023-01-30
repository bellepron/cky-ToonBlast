using UnityEngine;
using cky.Interfaces;
using cky.Managers;
using cky.Structs;
using cky.Enums;

namespace cky.Items
{
    public class CrateItem : ItemAbstract, IClickable
    {
        int _layer = 2;
        bool _interactable = true;
        public void Execute() { }

        public override void SingleExecute(Transform clickedItemTr, MatchingData matchingData)
        {
            ExecuteOneTimePerClick();
        }

        public override void CombinedExecute(ItemAbstract clickedItemAbstract, MatchingData matchingData)
        {
            ExecuteOneTimePerClick();
        }

        public override void InitializeItem(ItemType itemType, GridData posInGridData)
        {
            base.InitializeItem(itemType, posInGridData);

            EventManager.ActivateTouch += Interactable;
        }

        private void Interactable(bool obj)
        {
            _interactable = true;
        }

        private void ExecuteOneTimePerClick()
        {
            if (_interactable == true)
            {
                _interactable = false;

                _layer--;
                if (_layer == 1)
                {
                    Vibrate();
                    spriteRenderer.sprite = SpriteManager.Instance.CrateLayer1Sprite;
                }
                else if (_layer == 0)
                {
                    if (IsOnGridData() == false) return;
                    RemoveFromGridData();

                    EventManager.ActivateTouch -= Interactable;

                    Destroy();
                }
            }
        }
    }
}