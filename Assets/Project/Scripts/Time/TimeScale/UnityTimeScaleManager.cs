using Cysharp.Threading.Tasks;
using Project.Scripts.Core.Transitions;

namespace Project.Scripts.Time.TimeScale
{
    public class UnityTimeScaleManager : ITimeScaleManager
    {
        public float CurrentTimeScale => UnityEngine.Time.timeScale;
        
        public void SetTimeScale(float timeScale)
        {
            UnityEngine.Time.timeScale = timeScale;
        }

    }
}