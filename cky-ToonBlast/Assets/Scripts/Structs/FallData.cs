using DG.Tweening;

namespace cky.Structs
{
    [System.Serializable]
    public struct FallData
    {
        public Ease fallingEase;
        public float fallTimePerUnit;
        public float fallTimeIncConst;

        public FallData(Ease fallingEase, float fallingTimePerUnit, float fallingTimeIncreaseConstant)
        {
            this.fallingEase = fallingEase;
            this.fallTimePerUnit = fallingTimePerUnit;
            this.fallTimeIncConst = fallingTimeIncreaseConstant;
        }
    }
}