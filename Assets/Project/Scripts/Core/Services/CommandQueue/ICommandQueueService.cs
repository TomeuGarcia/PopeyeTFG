namespace Popeye.Core.Services.CommandQueue
{
	public interface ICommandQueueService
	{
		public void AddCommand(ICommand commandToEnqueue);
	}
}