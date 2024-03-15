using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "TweenColor__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenColorConfig")]
    public class TweenColorConfig : ScriptableObject
    {
        [SerializeField] private Color _value = Color.white;
        [SerializeField, Range(0.01f, 5.0f)] private float _duration = 0.3f;
        [SerializeField] private Ease _ease = Ease.Linear;
        
        public Color Value => _value;
        public float Duration => _duration;
        public Ease Ease => _ease;
    }
}