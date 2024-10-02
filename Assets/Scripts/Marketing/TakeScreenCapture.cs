using UnityEngine;

namespace Marketing
{
    public class TakeScreenCapture : MonoBehaviour
    {
        private int screenAmount = 0;

        public void TakeScreenShoot()
        {
            ScreenCapture.CaptureScreenshot($"ScreenShoot{screenAmount}.png");
            screenAmount++;
        }
    }
}