using System.Collections;
using UnityEngine;
using System;

namespace cky.Helpers
{
    public static class Call
    {
        public static void DelayedCall(MonoBehaviour script, Action action, float delayedTime)
        {
            script.StartCoroutine(DelayedExecute(action, delayedTime));
        }

        static IEnumerator DelayedExecute(Action action, float delayedTime)
        {
            yield return new WaitForSeconds(delayedTime);

            action.Invoke();
        }
    }
}