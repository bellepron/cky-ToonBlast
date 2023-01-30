using UnityEngine;
using cky.Structs;

namespace cky.Interfaces
{
    public interface ISpriteChanger
    {
        public GameObject RocketHintGO { get; set; }
        public GameObject BombHintGO { get; set; }

        void SetSpriteHint(int neighbourCount, MinMatchData minMatchData);
    }
}