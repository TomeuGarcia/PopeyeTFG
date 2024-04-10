using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.Core.Transitions
{
    [CreateAssetMenu(fileName = "TransitionConfig_NAME", 
        menuName = ScriptableObjectsHelper.CORE_ASSETS_PATH + "TransitionConfig")]
    public class TransitionConfig : ScriptableObject
    {
        [SerializeField, Range(0.001f, 10.0f)] private float _transitionInDuration = 0.2f;
        [SerializeField] private Ease _transitionInEase = Ease.InOutSine;
        
        [Space(20)]
        [SerializeField, Range(0.001f, 10.0f)] private float _midpointDuration = 0.5f;
        
        [Space(20)]
        [SerializeField, Range(0.001f, 10.0f)] private float _transitionOutDuration = 0.2f;
        [SerializeField] private Ease _transitionOutEase = Ease.InOutSine;


        public float TransitionInDuration => _transitionInDuration;
        public Ease TransitionInEase => _transitionInEase;

        public float MidpointDuration => _midpointDuration;
        
        public float TransitionOutDuration => _transitionOutDuration;
        public Ease TransitionOutEase => _transitionOutEase;

        
        public void SetMidpointDuration(float midpointDuration)
        {
            _midpointDuration = midpointDuration;
        }
        
        
        
    }
}