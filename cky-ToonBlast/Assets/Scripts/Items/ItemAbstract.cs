using cky.Managers;
using UnityEngine;
using cky.Structs;
using cky.Enums;
using DG.Tweening;
using cky.Helpers;

namespace cky.Items
{
    [System.Serializable]
    public abstract class ItemAbstract : MonoBehaviour
    {
        #region Variables

        [SerializeField] protected MatchType matchType = MatchType.None;
        public MatchType MatchType { get { return matchType; } }

        protected SpriteRenderer spriteRenderer;

        private ItemType _itemType;
        public ItemType ItemType { get { return _itemType; } set { value = _itemType; } }
        public GridData Grid { get; set; }
        public int NeighbourCount { get; set; }

        public TextMesh labelText;
        protected ParticleSystem particle;

        LevelManager _levelManager;
        protected EventManager eventManager;

        #endregion

        #region Preparing

        public virtual void InitializeItem(ItemType itemType, GridData posInGridData)
        {
            GetComponents();

            _itemType = itemType;
            matchType = MatchTypeHelper.SetMatchType(ItemType);
            Grid = posInGridData;

            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetSpritesForItemType();

            labelText = transform.GetComponentInChildren<TextMesh>();
            UpdateLabelText();

            particle = GetComponentInChildren<ParticleSystem>();
            particle.Stop();

            SetHintObjects();
        }

        private void GetComponents()
        {
            _levelManager = LevelManager.Instance;
            eventManager = FindObjectOfType<EventManager>();
        }

        protected virtual void SetHintObjects() { }

        public MatchType GetMatchType() => matchType;

        public virtual bool IsMatchable()
        {
            if (MatchType == MatchType.None)
                return false;

            return true;
        }

        private Sprite GetSpritesForItemType()
            => SpriteManager.Instance.GetSpritesForItemType(ItemType);

        #endregion

        #region Executing

        public virtual void Vibrate()
        {
            AnimationHelper.Vibrate(transform);
        }

        public virtual void SingleExecute(Transform clickedItemTr, MatchingData matchingData)
        {
            if (IsOnGridData() == false) return;

            RemoveFromGridData();

            AnimationHelper.VibrateQuickly(transform).OnComplete(() => Destroy());
        }

        public virtual void CombinedExecute(ItemAbstract clickedItemTr, MatchingData matchingData)
        {
            if (IsOnGridData() == false) return;

            RemoveFromGridData();
        }

        protected void CombinedExecuteAnimation(ItemAbstract clickedItemAbstract, MatchingData matchingData)
        {
            particle.Play();

            AnimationHelper.RemoveAnimation(gameObject, clickedItemAbstract.transform, matchingData)
                 .OnComplete(() =>
                 {
                     particle.Stop();
                     Destroy();
                 });
        }

        #endregion

        #region Update Operations

        public void UpdateGrid(int i, int j)
        {
            Grid = new GridData(i, j);

            UpdateLabelText();
        }

        private void UpdateLabelText() => labelText.text = $"{Grid.i}:{Grid.j}";

        #endregion

        #region Remove Operations

        protected void RemoveFromGridData()
        {
            _levelManager.levelSettings.GridData[Grid.i, Grid.j] = null;

            eventManager.ItemRemovedEvent(ItemType, 1);
        }

        protected void Destroy() => Destroy(gameObject);

        #endregion

        #region Conditions

        protected bool IsOnGridData()
        {
            if (_levelManager.levelSettings.GridData[Grid.i, Grid.j] == null)
                return false;

            return true;
        }

        #endregion
    }
}