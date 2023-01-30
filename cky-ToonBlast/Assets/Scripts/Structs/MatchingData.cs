using DG.Tweening;

namespace cky.Structs
{
    [System.Serializable]
    public struct MatchingData
    {
        public float itemMaxScale;
        public float scaleUpTime;
        public float scaleDownTime;
        public float removeDistancingFactor;

        public MatchingData(float itemMaxScale, float scaleUpTime, float scaleDownTime, float removeDistancingFactor)
        {
            this.itemMaxScale = itemMaxScale;
            this.scaleUpTime = scaleUpTime;
            this.scaleDownTime = scaleDownTime;
            this.removeDistancingFactor = removeDistancingFactor;
        }
    }
}