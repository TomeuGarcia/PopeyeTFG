using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Popeye.Core.Services.CommandQueue
{
	public class CommandQueueServiceImpl : ICommandQueueService
	{
		private readonly Queue<ICommand> _commandsToExecute;
		private bool _runningCommand;

		public CommandQueueServiceImpl()
		{
			_commandsToExecute = new Queue<ICommand>();
			_runningCommand = false;
		}
        
		public void AddCommand(ICommand commandToEnqueue)
		{
			_commandsToExecute.Enqueue(commandToEnqueue);
			RunNextCommand().Forget();
		}

		private async UniTask RunNextCommand()
		{
			if (_runningCommand)
			{
				return;
			}
            
			while (_commandsToExecute.Count > 0)
			{
				_runningCommand = true;
				var commandToExecute = _commandsToExecute.Dequeue();
				await commandToExecute.Execute();
			}

			_runningCommand = false;
		}
	}
}