using UnityEngine;

namespace cky.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        void Awake()
        {
            PrepareCamera();
        }

        private void PrepareCamera()
        {
            var cam = GetComponent<Camera>();
            cam.orthographicSize = (10 / cam.aspect) / 2;
        }
    }
}