using UnityEngine;
using DG.Tweening;
using cky.Structs;
using static Unity.Burst.Intrinsics.X86.Avx;
using TMPro;

namespace cky.Helpers
{
    public static class AnimationHelper
    {

        public static Tween VibrateQuickly(Transform itemTr)
        {
            var seq = DOTween.Sequence();
            seq.Append(itemTr.DOScale(1.15f, 0.04f));
            seq.Append(itemTr.DOScale(1.0f, 0.2f));
            seq.SetEase(Ease.OutBounce);

            return seq;
        }
        public static Tween Vibrate(Transform itemTr)
        {
            var seq = DOTween.Sequence();
            seq.Append(itemTr.DOScale(1.15f, 0.1f));
            seq.Append(itemTr.DOScale(1.0f, 0.5f));
            seq.SetEase(Ease.OutBounce);

            return seq;
        }

        public static Tween Fall(GameObject item, float targetPosY, float arrivingTime, Ease easeType)
        {
            var seq = DOTween.Sequence();
            seq.Append(item.transform.DOMoveY(targetPosY, arrivingTime));
            seq.SetEase(easeType);

            return seq;
        }

        public static Tween RemoveAnimation(GameObject actor, Transform clickedItemTr, MatchingData matchingData)
        {
            var actorTr = actor.transform;
            var diffVector = actorTr.position - clickedItemTr.position;
            var addedPos = actorTr.position + diffVector * matchingData.removeDistancingFactor;

            Sequence seq = DOTween.Sequence();

            seq.Append(actorTr.DOMove(addedPos, matchingData.scaleUpTime).SetEase(Ease.Flash))
               .Join(actorTr.DOScale(matchingData.itemMaxScale, matchingData.scaleUpTime));

            seq.Append(actorTr.DOMove(clickedItemTr.position, matchingData.scaleDownTime))
               .Join(actorTr.DOScale(1f, matchingData.scaleDownTime));

            return seq;
        }

        public static Tween BoingImage(Transform actorTr, TextMeshProUGUI tmp, float maxScale, float completedScale, float imageScaleUpTime, float imageScaleDownTime)
        {
            var seq = DOTween.Sequence();
            seq.Append(actorTr.DOScale(maxScale, imageScaleUpTime));
            seq.Append(actorTr.DOScale(completedScale, imageScaleDownTime));

            return seq;
        }
    }
}