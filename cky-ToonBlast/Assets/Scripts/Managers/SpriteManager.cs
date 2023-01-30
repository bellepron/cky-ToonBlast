using cky.Enums;
using UnityEngine;

namespace cky.Managers
{
    public class SpriteManager : Singleton<SpriteManager>
    {
        #region Sprites

        public Sprite GreenCubeSprite;
        public Sprite GreenCubeRocketHintSprite;
        public Sprite GreenCubeBombHintSprite;
        public Sprite YellowCubeSprite;
        public Sprite YellowCubeRocketHintSprite;
        public Sprite YellowCubeBombHintSprite;
        public Sprite BlueCubeSprite;
        public Sprite BlueCubeRocketHintSprite;
        public Sprite BlueCubeBombHintSprite;
        public Sprite RedCubeSprite;
        public Sprite RedCubeRocketHintSprite;
        public Sprite RedCubeBombHintSprite;

        public Sprite BalloonSprite;

        public Sprite GreenBalloonSprite;
        public Sprite YellowBalloonSprite;
        public Sprite BlueBalloonSprite;
        public Sprite RedBalloonSprite;

        public Sprite CrateLayer1Sprite;
        public Sprite CrateLayer2Sprite;

        public Sprite BombSprite;

        public Sprite RocketVertical;
        public Sprite RocketHorizontal;
        public Sprite RocketUp;
        public Sprite RocketRight;
        public Sprite RocketDown;
        public Sprite RocketLeft;

        #endregion

        public Sprite GetSpritesForItemType(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.GreenCube:
                    return GreenCubeSprite;
                case ItemType.YellowCube:
                    return YellowCubeSprite;
                case ItemType.BlueCube:
                    return BlueCubeSprite;
                case ItemType.RedCube:
                    return RedCubeSprite;

                case ItemType.Balloon:
                    return BalloonSprite;
                case ItemType.Crate:
                    return CrateLayer2Sprite;
                case ItemType.VerticalRocket:
                    return RocketVertical;
                case ItemType.HorizontalRocket:
                    return RocketHorizontal;
                case ItemType.Bomb:
                    return BombSprite;
            }

            return null;
        }

        public Sprite GetSpritesForRocketHint(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.GreenCube:
                    return GreenCubeRocketHintSprite;
                case ItemType.YellowCube:
                    return YellowCubeRocketHintSprite;
                case ItemType.BlueCube:
                    return BlueCubeRocketHintSprite;
                case ItemType.RedCube:
                    return RedCubeRocketHintSprite;
            }

            return null;
        }

        public Sprite GetSpritesForBombHint(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.GreenCube:
                    return GreenCubeBombHintSprite;
                case ItemType.YellowCube:
                    return YellowCubeBombHintSprite;
                case ItemType.BlueCube:
                    return BlueCubeBombHintSprite;
                case ItemType.RedCube:
                    return RedCubeBombHintSprite;
            }

            return null;
        }
    }
}