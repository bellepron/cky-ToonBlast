using UnityEngine;
using cky.Interfaces;
using cky.Items;

namespace cky.Managers
{
    public class TouchManager : MonoBehaviour
    {
        EventManager _eventManager;
        Camera _camera;
        bool _isActive;
        bool _thereIsAMove = true;

        private void Start()
        {
            _camera = Camera.main;
            _eventManager = FindObjectOfType<EventManager>();

            SubscribeGamePanelEvents();
            SubscribeEvents();
        }

        private void OnGameStart() => ActivateTouch(true);

        #region Event Operations

        private void SubscribeGamePanelEvents() =>
            cky.GamePanels.EventManager.Instance.Add_OnGameStart(OnGameStart);
        private void UnSubscribeGamePanelEvents() =>
            cky.GamePanels.EventManager.Instance.Remove_OnGameStart(OnGameStart);
        private void NoMovesLeft() => _thereIsAMove = false;

        private void OnDisable() => UnSubscribeEvents();

        private void SubscribeEvents()
        {
            EventManager.ActivateTouch += ActivateTouch;
            EventManager.NoMovesLeft += NoMovesLeft;
        }

        private void UnSubscribeEvents()
        {
            EventManager.ActivateTouch -= ActivateTouch;
            EventManager.NoMovesLeft -= NoMovesLeft;

            EventManager.UpdateEvent -= GetTouch;
        }

        #endregion

        #region Activate Touch

        private void ActivateTouch(bool active)
        {
            if (_thereIsAMove == false)
                return;

            if (_isActive == false)
            {
                if (active == true)
                {
                    _isActive = true;
                    EventManager.UpdateEvent += GetTouch;
                }
            }

            if (_isActive == true)
            {
                if (active == false)
                {
                    _isActive = false;
                    EventManager.UpdateEvent -= GetTouch;
                }
            }
        }

        #endregion

        #region Core

        private void GetTouch()
        {
            if (Input.GetMouseButtonUp(0))
            {
                ExecuteTouch(Input.mousePosition);
            }
        }

        private void ExecuteTouch(Vector3 pos)
        {
            var hit = Physics2D.OverlapPoint(_camera.ScreenToWorldPoint(pos)) as BoxCollider2D;

            if (hit && hit.transform.TryGetComponent<IClickable>(out IClickable iClickable) == true)
            {
                _eventManager.ExecuteTouchEvent(hit.GetComponent<ItemAbstract>());
            }
        }

        #endregion
    }
}