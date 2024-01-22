using Cysharp.Threading.Tasks;
using Project.Scripts.Core.Transitions;

namespace Project.Scripts.Time.TimeScale
{
    public interface ITimeScaleManager
    {
        float CurrentTimeScale { get; }
        
        void SetTimeScale(float timeScale);
    }
}