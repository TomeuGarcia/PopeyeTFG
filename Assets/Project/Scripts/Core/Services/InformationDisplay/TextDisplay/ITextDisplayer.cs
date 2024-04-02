using Cysharp.Threading.Tasks;

namespace Popeye.Core.Services.InformationDisplay
{
    public interface ITextDisplayer
    {
        void StartShowing(TextDisplayConfig textDisplayConfig);
        void StopShowing(TextDisplayConfig textDisplayConfig);
    }
}