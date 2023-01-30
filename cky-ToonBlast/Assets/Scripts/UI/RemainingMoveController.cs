using cky.Managers;
using UnityEngine;
using TMPro;
using System.Collections;

namespace cky.UI.Controllers
{
    public class RemainingMoveController : MonoBehaviour
    {
        int _moves;
        EventManager _eventManager;

        [SerializeField] TextMeshProUGUI _movesTMP;

        private void Start()
        {
            _moves = LevelManager.Instance.levelSettings.moves;
            _eventManager = FindObjectOfType<EventManager>();

            UpdateMovesText(_moves);

            EventManager.DecreaseRemainingMove += Decrease;
        }

        private void OnDestroy()
        {
            EventManager.DecreaseRemainingMove -= Decrease;
        }

        private void Decrease()
        {
            _moves--;
            UpdateMovesText(_moves);

            if (_moves <= 0)
            {
                _eventManager.NoMovesLeftEvent();
            }
        }

        private void UpdateMovesText(int moves)
        {
            _movesTMP.text = $"Moves:{moves}";
        }
    }
}