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
    public class TextWorldTriggerable : MonoBehaviour, IWorldTriggerable
    {
        [Header("TRIGGER")]
        [SerializeField] private TriggerOnceGroup _triggerOnceGroup;
        
        [Header("TEXT")]
        [SerializeField] private TextDisplayConfig _textDisplayConfig;
        private ITextDisplayer _textDisplayer;
        
        private void Start()
        {
            _triggerOnceGroup.Init(this);

            _textDisplayer = ServiceLocator.Instance.GetService<IInformationDisplayService>().TextDisplayer;
        }

        public void Activate()
        {
            _textDisplayer.StartShowing(_textDisplayConfig);
        }

        public void Deactivate()
        {
            _textDisplayer.StopShowing(_textDisplayConfig);
        }

    }
}


