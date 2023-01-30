using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace cky.GamePanels
{
    public class GamePanelInput : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        public static event Action OnDown, OnMove, OnUp;

        bool _down;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _down = true;
            OnDown?.Invoke();
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            if (_down == true)
                OnMove?.Invoke();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            _down = false;
            OnUp?.Invoke();
        }
    }
}