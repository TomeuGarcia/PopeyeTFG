using DG.Tweening;
using NaughtyAttributes;
using Popeye.Scripts.TextUtilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Popeye.Modules.WorldElements.Tutorial
{
    [RequireComponent(typeof(TriggerOnceGroup))]
    public class TextWorldTriggerable : MonoBehaviour, IWorldTriggerable
    {
        [SerializeField] private TriggerOnceGroup _triggerOnceGroup;
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private TextContent _textContent;

        [SerializeField, Range(0.0f, 5.0f)] private float _timeToFadeOnActivate = 1.0f;
        [SerializeField, Range(0.0f, 5.0f)] private float _timeToFadeOnDeactivate = 1.0f;
        private void Awake()
        {
            _triggerOnceGroup.Init(this);
            SetTextContent();
            _text.alpha = 0.0f;
        }

        public void Activate()
        {
            _text.DOFade(1.0f, _timeToFadeOnActivate);
        }

        public void Deactivate()
        {
            _text.DOFade(0.0f, _timeToFadeOnDeactivate);
        }

        [Button]
        private void SetTextContent()
        {
            _text.SetContent(_textContent);
        }
    }
}


