using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;

namespace Popeye.Commands
{
	public class WaitCommand : ICommand
	{
		private readonly int _millisecondsToWait;

		public WaitCommand(int millisecondsToWait)
		{
			_millisecondsToWait = millisecondsToWait;
		}
		
		public async UniTask Execute()
		{
			await UniTask.Delay(_millisecondsToWait);
		}
	}
}