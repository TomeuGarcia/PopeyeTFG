using System.Collections.Generic;
using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class GeneralGameAudiosManager : MonoBehaviour, IGameAudiosManager
    {
        [Header("SUB MANAGERS")]
        [SerializeField] private InterfaceReference<IGameAudiosManager, MonoBehaviour>[] _subGameAudiosManagers;
        private List<IGameAudiosManager> _subGameAudiosManagersList;


        public void ConfigureBeforeInit(IGameAudiosManager[] extraSubGameAudiosManagersList)
        {
            _subGameAudiosManagersList = new List<IGameAudiosManager>(extraSubGameAudiosManagersList);
            
            foreach (InterfaceReference<IGameAudiosManager, MonoBehaviour> subGameAudiosManager in _subGameAudiosManagers)
            {
                _subGameAudiosManagersList.Add(subGameAudiosManager.Value);
            }

        }
        
        public void Init(AFMODAudioManagerReference audioManager, IEventSystemService eventSystemService)
        {
            foreach (IGameAudiosManager subGameAudiosManager in _subGameAudiosManagersList)
            {
                subGameAudiosManager.Init(audioManager, eventSystemService);
            }
        }

        public void StartListeningToGameEvents()
        {
            foreach (IGameAudiosManager subGameAudiosManager in _subGameAudiosManagersList)
            {
                subGameAudiosManager.StartListeningToGameEvents();
            }
        }

        public void StopListeningToGameEvents()
        {
            foreach (IGameAudiosManager subGameAudiosManager in _subGameAudiosManagersList)
            {
                subGameAudiosManager.StopListeningToGameEvents();
            }
        }
    }
}