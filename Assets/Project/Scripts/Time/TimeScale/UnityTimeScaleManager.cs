namespace Project.Scripts.Time.TimeScale
{
    public class UnityTimeScaleManager : ITimeScaleManager
    {
        public void SetTimeScale(float timeScale)
        {
            UnityEngine.Time.timeScale = timeScale;
        }
    }
}