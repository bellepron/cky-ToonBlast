using System.Collections.Generic;
using cky.Enums;
using cky.Helpers;
using cky.Structs;
using UnityEngine;

namespace cky.Managers
{
    public class QuestManager : MonoBehaviour
    {
        #region Variables

        EventManager _eventManager;
        LevelSettings _levelSettings;
        List<QuestData> _questDatas = new List<QuestData>();
        int _questCount;
        bool _success;

        #endregion

        #region Preparing

        private void Start()
        {
            GetComponents();

            SetQuestPanel(_levelSettings);

            EventManager.ItemRemoved += CheckSituation;
            EventManager.NoMovesLeft += NoMovesLeft;
            EventManager.GameSuccess += OnSuccess;
        }

        private void OnSuccess() => EventManager.ItemRemoved -= CheckSituation;
        private void OnDestroy() => EventManager.ItemRemoved -= CheckSituation;

        private void GetComponents()
        {
            _eventManager = FindObjectOfType<EventManager>();

            _levelSettings = LevelManager.Instance.levelSettings;
            _questCount = _levelSettings.questDatas.Count;
        }

        private void SetQuestPanel(LevelSettings levelSettings)
        {
            for (int i = 0; i < _questCount; i++)
            {
                var data = levelSettings.questDatas[i];
                _questDatas.Add(new QuestData(data.type, data.count, data.isCompleted));

                UpdateQuestPanel(data.type, data.count);
            }
        }

        #endregion

        private void CheckSituation(ItemType itemType, int count)
        {
            var completedQuestCount = 0;

            for (int i = 0; i < _questCount; i++)
            {
                var questData = _questDatas[i];

                if (questData.isCompleted == true)
                {
                    completedQuestCount++;
                    continue;
                }

                if (itemType == questData.type)
                {
                    questData.count = questData.count - count;

                    if (questData.count <= 0)
                    {
                        completedQuestCount++;
                        questData.Completed();

                        UpdateQuestPanel(questData.type, 0);
                    }
                    else
                    {
                        UpdateQuestPanel(questData.type, questData.count);
                    }
                }
            }

            if (completedQuestCount >= _questCount)
            {
                if (_success == true) return;
                _success = true;

                _eventManager.NoMovesLeftEvent();
            }
        }

        private void NoMovesLeft()
        {
            Call.DelayedCall(this, DecideSuccessOrFail, 1.5f);
        }

        private void UpdateQuestPanel(ItemType type, int count)
        {
            _eventManager.UpdateMissionPanelEvent(type, count);
        }

        private void DecideSuccessOrFail()
        {
            if (_success == true)
            {
                Debug.Log("success");
                _eventManager.GameSuccessEvent();
            }
            else
            {
                _eventManager.GameFailEvent();
            }
        }
    }
}