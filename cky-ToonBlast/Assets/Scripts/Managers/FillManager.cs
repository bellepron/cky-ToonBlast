using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using cky.Structs;
using cky.Helpers;

namespace cky.Managers
{
    public class FillManager : MonoBehaviour
    {
        #region Variables

        LevelSettings _levelSettings;
        ItemManager _itemManager;
        EventManager _eventManager;

        int _width, _height;
        FallData _fallData;
        int _maxFillingTimeMilisec;

        #endregion

        #region Preparing

        private void Start()
        {
            _levelSettings = LevelManager.Instance.levelSettings;
            _itemManager = ItemManager.Instance;
            _eventManager = FindObjectOfType<EventManager>();

            _width = _levelSettings.width;
            _height = _levelSettings.height;
            _fallData = _levelSettings.fallingData;

            EventManager.CheckFills += Fill;
        }

        private void OnDisable() => EventManager.CheckFills -= Fill;

        #endregion

        #region Filling

        private void Fill()
        {
            var fillAmount = 0;
            var noNeedToFill = true;

            for (int i = 0; i < _width; i++)
            {
                fillAmount = 0;
                for (int j = _height - 1; j >= 0; j--)
                {
                    var go = _levelSettings.GridData[i, j];

                    if (go == null)
                    {
                        fillAmount++;
                        noNeedToFill = false;

                        if (j == 0)
                            FillColumn(i, fillAmount);
                    }
                    else
                    {
                        if (fillAmount != 0)
                            FillColumn(i, fillAmount);

                        break;
                    }
                }
            }

            if (noNeedToFill)
                _eventManager.ActivateTouchEvent(true);
        }

        //private void Test(int i, int value)
        //{
        //    Debug.Log($"{i}.Column needs {value}");
        //}

        public async void FillColumn(int i, int fillAmount)
        {
            await Filling(i, fillAmount);

            ActivateTouchAfterFilling(_maxFillingTimeMilisec);
        }

        private async Task Filling(int i, int fillAmount)
        {
            _maxFillingTimeMilisec = 0;
            var maxJ = _levelSettings.height - 1;
            for (int j = 0; j < fillAmount; j++)
            {
                FillWithOrder(new GridData(i, maxJ - j), fillAmount - j);
            }

            await Task.Yield();
        }

        private void FillWithOrder(GridData gridData, int fillingOrder)
        {
            var offsetY = _levelSettings.height + 1;
            var targetPos = PositionHelper.CalculatePosition(gridData);
            var initPos = new Vector2(targetPos.x, offsetY + fillingOrder);

            var randomItemType = _levelSettings.GetRandomCubeItemType();
            var go = _itemManager.CreateItem(randomItemType, gridData, initPos);

            var diffY = initPos.y - targetPos.y;
            var fallingTime = _fallData.fallTimePerUnit * diffY + fillingOrder * _fallData.fallTimeIncConst;
            UpdateMaxFillingTime(fallingTime);

            AnimationHelper.Fall(go, targetPos.y, fallingTime, _fallData.fallingEase)
                .OnComplete(() => UpdateHintsAfterFall());
        }

        private void UpdateMaxFillingTime(float fillTime)
        {
            if (fillTime > _maxFillingTimeMilisec)
                _maxFillingTimeMilisec = (int)(fillTime * 1000);
        }

        #endregion

        #region After Filling

        private async void ActivateTouchAfterFilling(int delayMilisec)
        {
            await Task.Delay(delayMilisec);

            _eventManager.ActivateTouchEvent(true);
        }

        private void UpdateHintsAfterFall()
        {
            _eventManager.CheckHintsAction();
        }

        #endregion
    }
}