using UnityEngine;
using Popeye.Core.Services.ServiceLocator;

namespace Popeye.Core.Installers
{
	public abstract class AInstaller : MonoBehaviour
	{
		public abstract void Install(ServiceLocator serviceLocator);
		public abstract void Uninstall(ServiceLocator serviceLocator);
	}
}