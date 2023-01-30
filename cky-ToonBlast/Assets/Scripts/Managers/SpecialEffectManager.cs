using cky.Enums;
using cky.Items;
using cky.Structs;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace cky.Managers
{
    public class SpecialEffectManager : Singleton<SpecialEffectManager>
    {
        #region Variables

        EventManager _eventManager;
        LevelSettings _levelSettings;
        int _width, _height;
        MatchingData _matchingData;

        float _itemDestroyAnimationTime;
        int _itemDestroyAnimationTimeMilisec;
        int _rocketDelayMilisec = 50;

        float _timer = 0.0f;
        bool _isEffectsActive;

        #endregion

        #region Preparing

        private void Start()
        {
            _eventManager = FindObjectOfType<EventManager>();
            _levelSettings = LevelManager.Instance.levelSettings;
            _width = _levelSettings.width;
            _height = _levelSettings.height;
            _matchingData = _levelSettings.matchingData;
            _itemDestroyAnimationTime = _matchingData.scaleUpTime + _matchingData.scaleDownTime;
            _itemDestroyAnimationTimeMilisec = (int)(_itemDestroyAnimationTime * 1000);
        }

        #endregion

        #region Set by Score

        public async void ResponseByScore(GridData clickedItemGridData, int score)
        {
            await Task.Delay(_itemDestroyAnimationTimeMilisec + 100);

            if (score == 2) CombinedRocketRocketExplosion(clickedItemGridData);
            if (score == 3) CombinedBombRocketExplosion(clickedItemGridData);
            if (score == 4) BombExplosion(clickedItemGridData, 7);
        }

        #endregion

        #region Rocket Explosion

        public async void RocketExplosion(ItemAbstract itemAbstract, ItemType type)
        {
            var gridData = itemAbstract.Grid;

            if (type == ItemType.VerticalRocket)
            {
                RocketUp(gridData.i, gridData.j);
                RocketDown(gridData.i, gridData.j);

                await RocketDelay(gridData.j, _height - 1, 0, 0);
            }

            if (type == ItemType.HorizontalRocket)
            {
                RocketRight(gridData.i, gridData.j);
                RocketLeft(gridData.i, gridData.j);

                await RocketDelay(0, 0, gridData.i, _width - 1);
            }
        }

        private async void CombinedRocketRocketExplosion(GridData clickedItemGridData)
        {
            MinusShapedExplosion(clickedItemGridData, 0, 0);

            await RocketDelay(clickedItemGridData.j, _height - 1, clickedItemGridData.i, _width - 1);
        }

        #region Rocket Up & Down & Right & Left

        private async void RocketUp(int gridI, int gridJ)
        {
            for (int j = gridJ + 1; j < _height; j++)
            {
                var item = _levelSettings.GridData[gridI, j];
                if (item == null) continue;

                await Task.Delay(_rocketDelayMilisec);

                Execute(item);
            }
        }

        private async void RocketDown(int gridI, int gridJ)
        {
            for (int j = gridJ - 1; j >= 0; j--)
            {
                var item = _levelSettings.GridData[gridI, j];
                if (item == null) continue;

                await Task.Delay(_rocketDelayMilisec);

                Execute(item);
            }
        }

        private async void RocketRight(int gridI, int gridJ)
        {
            for (int i = gridI + 1; i < _width; i++)
            {
                var item = _levelSettings.GridData[i, gridJ];
                if (item == null) continue;

                await Task.Delay(_rocketDelayMilisec);

                Execute(item);
            }
        }

        private async void RocketLeft(int gridI, int gridJ)
        {
            for (int i = gridI - 1; i >= 0; i--)
            {
                var item = _levelSettings.GridData[i, gridJ];
                if (item == null) continue;

                await Task.Delay(_rocketDelayMilisec);

                Execute(item);
            }
        }

        private async Task RocketDelay(int gridIndexJ, int maxJ, int gridIndexI, int maxI)
        {
            var up = maxJ - gridIndexJ;
            var down = gridIndexJ;
            var right = maxI - gridIndexI;
            var left = gridIndexI;
            var vertical = up > down ? up : down;
            var horizontal = right > left ? right : left;
            var multiplier = vertical > horizontal ? vertical : horizontal;

            await TaskDelay(_rocketDelayMilisec * (multiplier));
        }

        private async Task TaskDelay(int multiplier)
        {
            await Task.Delay(multiplier);
        }

        #endregion

        #endregion

        #region Bomb Explosion

        public void BombExplosion(GridData gridData, int diameter = 3)
        {
            var offset = Mathf.FloorToInt(diameter * 0.5f);
            var gridI = gridData.i - offset;
            var gridJ = gridData.j - offset;

            for (int j = 0; j < diameter; j++)
            {
                for (int i = 0; i < diameter; i++)
                {
                    if (!InBounds(gridI + i, gridJ + j)) continue;

                    var item = _levelSettings.GridData[gridI + i, gridJ + j];
                    if (item == null) continue;

                    Execute(item);
                }
            }
        }

        #endregion

        #region Bomb & Rocket Combine

        public async void CombinedBombRocketExplosion(GridData gridData)
        {
            MinusShapedExplosion(gridData, 0, 0);
            MinusShapedExplosion(gridData, 1, 0);
            MinusShapedExplosion(gridData, -1, 0);
            MinusShapedExplosion(gridData, 0, 1);
            MinusShapedExplosion(gridData, 0, -1);

            await RocketDelay(gridData.j, _height - 1, gridData.i, _width - 1);
        }

        private void MinusShapedExplosion(GridData gridData, int offI, int offJ)
        {
            if (InBounds(gridData.i + offI, gridData.j + offJ))
            {
                RocketUp(gridData.i + offI, gridData.j + offJ);
                RocketDown(gridData.i + offI, gridData.j + offJ);
                RocketRight(gridData.i + offI, gridData.j + offJ);
                RocketLeft(gridData.i + offI, gridData.j + offJ);
            }
        }

        #endregion

        #region Conditions

        private bool InBounds(int i, int j)
        {
            if (i < 0 || i >= _width || j < 0 || j >= _height)
                return false;

            return true;
        }

        #endregion

        #region Execute

        private void Execute(GameObject item)
        {
            if (item == null) return;

            if (item.TryGetComponent<ItemAbstract>(out var itemAbs) == true)
            {
                itemAbs.SingleExecute(itemAbs.transform, _matchingData);
            }

            TriggerEventsEndOfEffects();
        }

        #endregion

        #region Trigger Events - When all the special effects are done!

        private void ResetTimer() => _timer = 0.0f;
        private void TriggerEventsEndOfEffects()
        {
            ResetTimer();

            if (_isEffectsActive == false)
            {
                _isEffectsActive = true;
                StartCoroutine(WaitForEndOfEffects());
            }
        }
        IEnumerator WaitForEndOfEffects()
        {
            while (_isEffectsActive)
            {
                _timer += Time.deltaTime;

                if (_timer >= _itemDestroyAnimationTime)
                {
                    TriggerEvents();

                    _isEffectsActive = false;
                }

                yield return null;
            }
        }
        private void TriggerEvents() => _eventManager.CheckFallsAction();

        #endregion
    }
}