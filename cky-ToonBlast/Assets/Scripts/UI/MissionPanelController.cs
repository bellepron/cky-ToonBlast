using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using cky.Managers;
using cky.Structs;
using cky.Enums;
using TMPro;
using cky.Helpers;

namespace cky.UI.Controllers
{
    public class MissionPanelController : MonoBehaviour
    {
        SpriteManager _spriteManager;
        int _questCount;

        [SerializeField] Transform questPanelHolderTr;
        [SerializeField] GameObject questPanelPrefab;
        [SerializeField] float questImageMaxScale = 1.5f, questImageCompletedScale = 1.25f;
        [SerializeField] float imageScaleUpTime = 0.25f, imageScaleDownTime = 0.12f;

        List<TextMeshProUGUI> _questRemainingTMPs = new List<TextMeshProUGUI>();
        List<QuestData> _questDatas = new List<QuestData>();
        List<Transform> _imageGOs = new List<Transform>();

        private void Start()
        {
            EventManager.UpdateMissionPanel += UpdateMissionPanel;

            _spriteManager = SpriteManager.Instance;

            var _levelSettings = LevelManager.Instance.levelSettings;
            _questCount = _levelSettings.questDatas.Count;

            for (int i = 0; i < _questCount; i++)
            {
                var questPanel = Instantiate(questPanelPrefab, questPanelHolderTr.position, Quaternion.identity, questPanelHolderTr);
                _questRemainingTMPs.Add(questPanel.GetComponentInChildren<TextMeshProUGUI>());

                var data = _levelSettings.questDatas[i];
                _imageGOs.Add(SetQuestSprite(questPanel, data.type));
                _questDatas.Add(new QuestData(data.type, data.count, data.isCompleted));
            }
        }

        private void OnDestroy() => EventManager.UpdateMissionPanel -= UpdateMissionPanel;

        private Transform SetQuestSprite(GameObject go, ItemType type)
        {
            var spriteRenderer = go.GetComponentInChildren<Image>();
            spriteRenderer.sprite = _spriteManager.GetSpritesForItemType(type);

            return spriteRenderer.transform;
        }

        private void UpdateMissionPanel(ItemType type, int count)
        {
            for (int i = 0; i < _questCount; i++)
            {
                var data = _questDatas[i];
                var tmp = _questRemainingTMPs[i];

                if (type == data.type)
                {
                    UpdateTmp(tmp, count);

                    if (count == 0)
                    {
                        BoingImage(_imageGOs[i], tmp);
                    }
                }
            }
        }

        private void UpdateTmp(TextMeshProUGUI tmp, int count) => tmp.text = $"{count}";
        private void CloseTMP(TextMeshProUGUI tmp) => tmp.text = $"";

        private void BoingImage(Transform actorTr, TextMeshProUGUI tmp)
        {
            AnimationHelper.BoingImage(actorTr, tmp, questImageMaxScale, questImageCompletedScale, imageScaleUpTime, imageScaleDownTime)
                .OnComplete(() => CloseTMP(tmp));
        }
    }
}