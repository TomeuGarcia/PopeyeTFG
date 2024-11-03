using DG.Tweening;
using NaughtyAttributes;
using Popeye.Scripts.TextUtilities;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Core.Services.ServiceLocator;
using TMPro;
using UnityEngine;

namespace Popeye.Modules.WorldElements.Tutorial
{
    [RequireComponent(typeof(TriggerOnceGroup))]
    public class MessageDisplayWorldTriggerable : MonoBehaviour, IWorldTriggerable
    {
        [Header("TRIGGER")]
        [SerializeField] private TriggerOnceGroup _triggerOnceGroup;
        
        [Header("TEXT")]
        [SerializeField] private TextDisplayConfig _textDisplayConfig;
        [SerializeField] private VideoDisplayConfig _videoDisplayConfig;
        private IInformationDisplayService _informationDisplayService;

        private bool HasVideoToDisplay => _videoDisplayConfig != null;
        
        private void Start()
        {
            _triggerOnceGroup.Init(this);
            _informationDisplayService = ServiceLocator.Instance.GetService<IInformationDisplayService>();
        }

        public void Activate()
        {
            _informationDisplayService.TextDisplayer.StartShowing(_textDisplayConfig);
            if (HasVideoToDisplay)
            {
                _informationDisplayService.VideoDisplayer.StartShowing(_videoDisplayConfig);
            }            
        }

        public void Deactivate()
        {
            _informationDisplayService.TextDisplayer.StopShowing(_textDisplayConfig);
            if (HasVideoToDisplay)
            {
                _informationDisplayService.VideoDisplayer.StopShowing(_videoDisplayConfig);
            }
        }

    }
}


