namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public interface ITimerPressurePlateView
    {
        void SetTimerStart();
        void StartTimerCountdown(float totalDuration, float durationBeforeFinish);
        void CancelTimerCountdown();
        void CancelAndLockTimerCountdown();
    }
}