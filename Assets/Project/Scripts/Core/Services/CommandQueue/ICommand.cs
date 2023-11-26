using Cysharp.Threading.Tasks;

namespace Popeye.Core.Services.CommandQueue
{
    public interface ICommand
    {
        UniTask Execute();
    }
}
