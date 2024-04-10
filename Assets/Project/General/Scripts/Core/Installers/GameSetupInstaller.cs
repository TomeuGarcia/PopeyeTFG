
using Popeye.Core.Services.ServiceLocator;
using Popeye.Core.Services.CommandQueue;
using Popeye.Core.Services.DataStorage;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.Serializer;
using UnityEngine;

namespace Popeye.Core.Installers
{
	public abstract class GameSetupInstaller : GeneralInstaller
	{
		protected override void DoInstallDependencies()
		{			
			ServiceLocator.Instance.RegisterService<ICommandQueueService>(new CommandQueueServiceImpl());
			ServiceLocator.Instance.RegisterService<IEventSystemService>(new EventSystemService());
	
			var serializer = new UnityJsonSerializerService();
			var dataStore = new PlayerPrefsDataStorageService(serializer);
			ServiceLocator.Instance.RegisterService<IDataStorageService>(dataStore);
			
		}
		
		protected override void DoUninstallDependencies()
		{
			
		}
	}
}