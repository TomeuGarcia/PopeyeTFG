namespace Popeye.Core.Services.InformationDisplay
{
    public interface IVideoDisplayer
    {
        void StartShowing(VideoDisplayConfig videoDisplayConfig);
        void StopShowing(VideoDisplayConfig videoDisplayConfig);
    }
}