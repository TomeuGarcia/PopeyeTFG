using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class GeneralGameAudiosManager : MonoBehaviour, IGameAudiosManager
    {
        [Header("SUB MANAGERS")]
        [SerializeField] private InterfaceReference<IGameAudiosManager, MonoBehaviour>[] _subGameAudiosManagers;


        public void Init(IFMODAudioManager audioManager, IEventSystemService eventSystemService)
        {
            foreach (InterfaceReference<IGameAudiosManager, MonoBehaviour> subGameAudiosManager in _subGameAudiosManagers)
            {
                subGameAudiosManager.Value.Init(audioManager, eventSystemService);
            }
        }

        public void StartListeningToGameEvents()
        {
            foreach (InterfaceReference<IGameAudiosManager, MonoBehaviour> subGameAudiosManager in _subGameAudiosManagers)
            {
                subGameAudiosManager.Value.StartListeningToGameEvents();
            }
        }

        public void StopListeningToGameEvents()
        {
            foreach (InterfaceReference<IGameAudiosManager, MonoBehaviour> subGameAudiosManager in _subGameAudiosManagers)
            {
                subGameAudiosManager.Value.StopListeningToGameEvents();
            }
        }
    }
}