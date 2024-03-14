using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "TweenConfig_TYPE__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenConfig")]
    public class TweenConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _value = new Vector3(0.5f, 0.5f, 0);
        [SerializeField, Range(0.01f, 5.0f)] private float _duration = 0.3f;
        [SerializeField] private Ease _ease = Ease.Linear;
        
        public Vector3 Value => _value;
        public float Duration => _duration;
        public Ease Ease => _ease;

    }
}