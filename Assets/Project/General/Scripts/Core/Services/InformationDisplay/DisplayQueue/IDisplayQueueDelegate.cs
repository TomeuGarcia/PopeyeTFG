using Cysharp.Threading.Tasks;

namespace Popeye.Core.Services.InformationDisplay
{
    public interface IDisplayQueueDelegate
    {
        UniTask DoStartShowing();
        UniTask DoStopShowing();
    }
}