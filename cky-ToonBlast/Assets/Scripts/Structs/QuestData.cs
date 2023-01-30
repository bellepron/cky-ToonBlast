using UnityEngine;
using cky.Enums;

namespace cky.Structs
{
    [System.Serializable]
    public class QuestData
    {
        public ItemType type;
        public int count;
        [HideInInspector] public bool isCompleted;

        public QuestData(ItemType type, int count, bool isCompleted = false)
        {
            this.type = type;
            this.count = count;
            this.isCompleted = isCompleted;
        }

        public void Completed() => isCompleted = true;
    }
}