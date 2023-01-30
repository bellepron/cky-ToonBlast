using UnityEngine;
using cky.Managers;
using cky.Structs;

namespace cky.Helpers
{
    public static class PositionHelper
    {
        public static Vector2 CalculatePosition(GridData gridData)
        {
            LevelSettings levelSettings = LevelManager.Instance.levelSettings;

            var i = gridData.i;
            var j = gridData.j;
            var inc = new Vector2(i, j) * levelSettings.increaseConstant;
            var pos = levelSettings.StartPosition + inc;

            return pos;
        }

        public static float CalculatePositionY(GridData gridData)
        {
            LevelSettings levelSettings = LevelManager.Instance.levelSettings;

            var i = gridData.i;
            var j = gridData.j;
            var inc = new Vector2(i, j) * levelSettings.increaseConstant;
            var pos = levelSettings.StartPosition + inc;

            return pos.y;
        }
    }
}