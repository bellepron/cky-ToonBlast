using System;
using UnityEngine;
using cky.Items;
using cky.Enums;

namespace cky.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static event Action UpdateEvent, GameSuccess, GameFail;
        public static event Action<bool> ActivateTouch;
        public static event Action<ItemAbstract> ExecuteTouch;
        public static event Action CheckFalls, CheckFills, CheckHints;

        public static event Action DecreaseRemainingMove, NoMovesLeft;
        public static event Action<ItemType, int> ItemRemoved, UpdateMissionPanel;

        public static event Action<int> SetLevelText;

        private void Awake()
        {
            UpdateEvent = null;
            GameSuccess = null;
            GameFail = null;

            ActivateTouch = null;
            ExecuteTouch = null;
            CheckFalls = null;
            CheckFills = null;
            CheckHints = null;

            DecreaseRemainingMove = null;
            NoMovesLeft = null;

            SetLevelText = null;
        }

        private void Update() => UpdateEvent?.Invoke();
        public void GameSuccessEvent() => GameSuccess?.Invoke();
        public void GameFailEvent() => GameFail?.Invoke();

        public void ActivateTouchEvent(bool active) => ActivateTouch?.Invoke(active);
        public void ExecuteTouchEvent(ItemAbstract clickedItemAbstract)
            => ExecuteTouch?.Invoke(clickedItemAbstract);
        public void CheckFallsAction() => CheckFalls?.Invoke();
        public void CheckFillAction() => CheckFills?.Invoke();
        public void CheckHintsAction() => CheckHints?.Invoke();
        public void DecreaseRemainingMoveEvent() => DecreaseRemainingMove?.Invoke();
        public void NoMovesLeftEvent() => NoMovesLeft?.Invoke();
        public void ItemRemovedEvent(ItemType itemType, int count)
            => ItemRemoved?.Invoke(itemType, count);
        public void UpdateMissionPanelEvent(ItemType itemType, int count)
            => UpdateMissionPanel?.Invoke(itemType, count);

        public void SetLevelTextEvent(int levelIndex) => SetLevelText?.Invoke(levelIndex);
    }
}