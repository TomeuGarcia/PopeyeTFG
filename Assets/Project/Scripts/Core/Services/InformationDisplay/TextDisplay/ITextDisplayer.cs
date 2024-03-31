using Cysharp.Threading.Tasks;

namespace Popeye.Core.Services.InformationDisplay
{
    public interface ITextDisplayer
    {
        UniTask StartShowing(TextDisplayConfig textDisplayConfig);
        UniTask StopShowing(TextDisplayConfig textDisplayConfig);
    }
}