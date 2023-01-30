using UnityEngine;
using DG.Tweening;
using cky.Interfaces;
using cky.Structs;
using cky.Helpers;
using cky.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cky.Managers
{
    public class FallManager : MonoBehaviour
    {
        #region Variables

        LevelSettings _levelSettings;
        EventManager _eventManager;
        int _width, _height;
        FallData _fallData;

        List<float> fallTimes = new List<float>();

        #endregion

        #region Preparing

        private void Start()
        {
            _levelSettings = LevelManager.Instance.levelSettings;
            _eventManager = FindObjectOfType<EventManager>();
            _width = _levelSettings.width;
            _height = _levelSettings.height;
            _fallData = _levelSettings.fallingData;

            EventManager.CheckFalls += Fall;
        }

        private void OnDisable()
        {
            EventManager.CheckFalls -= Fall;
        }

        #endregion

        #region Test

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //        Fall();
        //}

        #endregion

        #region Falling

        private async void Fall()
        {
            fallTimes.Clear();

            for (int i = 0; i < _width; i++)
            {
                GameObject go = null;

                for (int j = 1; j < _height; j++)
                {
                    go = _levelSettings.GridData[i, j];
                    if (go == null)
                    {
                        continue;
                    }
                    if (go.TryGetComponent<IFallable>(out var iFallable) == false)
                    {
                        continue;
                    }

                    fallTimes.Add(CheckDownside(i, j, go));
                }
            }

            await WaitForFallingFÝnish();

            CheckFill();
        }

        private async Task WaitForFallingFÝnish()
        {
            var operationTime = 0.0f;
            foreach (var t in fallTimes)
            {
                if (t > operationTime)
                    operationTime = t;
            }
            var milisec = (int)(operationTime * 1000);

            await Task.Delay(milisec);
        }

        private float CheckDownside(int i, int j, GameObject go)
        {
            int c = 0;
            bool b = false;

            for (int k = j - 1; k >= 0; k--)
            {
                var goWillCheck = _levelSettings.GridData[i, k];

                if (goWillCheck == null)
                {
                    c++;
                    b = true;
                }
                else
                {
                    break;
                }
            }

            if (b == false) return 0;

            _levelSettings.GridData[i, j] = null;
            _levelSettings.GridData[i, j - c] = go;

            var goItemAbstract = go.GetComponent<ItemAbstract>();
            goItemAbstract.UpdateGrid(i, j - c);

            var targetPosY = PositionHelper.CalculatePositionY(new GridData(i, j - c));

            var fallingTime = _fallData.fallTimePerUnit * c;

            AnimationHelper.Fall(go, targetPosY, fallingTime, _fallData.fallingEase)
                .OnComplete(() => UpdateHintsAfterFall());

            return fallingTime;
        }

        #endregion

        #region After Fall

        private void UpdateHintsAfterFall() => _eventManager.CheckHintsAction();

        private void CheckFill() => _eventManager.CheckFillAction();

        #endregion
    }
}