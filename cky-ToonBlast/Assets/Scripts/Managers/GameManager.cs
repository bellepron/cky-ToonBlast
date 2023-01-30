using UnityEngine;

namespace cky.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] bool showLabels;
        [SerializeField] GameObject labelCamera;
        cky.GamePanels.EventManager _gamePanelEvents;

        #endregion

        #region Preparing

        private void Awake()
        {
            _gamePanelEvents = cky.GamePanels.EventManager.Instance;
            _gamePanelEvents.Initialize();
        }

        private void Start()
        {
            ShowLabels();

            SubscribeEvents();
        }

        #endregion

        #region Show Item Indexes

        private void ShowLabels()
        {
            if (showLabels == true)
                labelCamera.SetActive(true);
            else
                labelCamera.SetActive(false);
        }

        #endregion

        #region Event Operations

        private void SubscribeEvents()
        {
            EventManager.GameSuccess += Success;
            EventManager.GameFail += Fail;
        }

        private void UnSubscribeEvents()
        {
            EventManager.GameSuccess -= Success;
            EventManager.GameFail -= Fail;
        }

        #endregion

        private void Success()
        {
            UnSubscribeEvents();

            _gamePanelEvents.GameSuccessEvent();
        }

        private void Fail()
        {
            UnSubscribeEvents();

            _gamePanelEvents.GameFailEvent();
        }
    }
}