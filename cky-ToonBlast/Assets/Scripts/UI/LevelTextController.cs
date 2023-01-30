using UnityEngine;
using cky.Managers;
using TMPro;

namespace cky.UI.Controllers
{
    public class LevelTextController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelTextTMP;

        private void Start()
        {
            EventManager.SetLevelText += UpdateLevelText;
        }

        public void UpdateLevelText(int levelIndex)
        {
            levelTextTMP.text = $"Level {levelIndex + 1}";
        }
    }
}