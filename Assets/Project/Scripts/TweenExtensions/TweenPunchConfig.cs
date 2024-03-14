using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "TweenPunch_TYPE__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenPunchConfig")]
    public class TweenPunchConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _value = new Vector3(0.5f, 0.5f, 0);
        [SerializeField, Range(0.01f, 5.0f)] private float _duration = 0.3f;
        [SerializeField, Range(0, 10)] private int _vibrato = 7;
        [SerializeField, Range(0.0f, 1.0f)] private float _elasticity = 1f;
        [SerializeField] private Ease _ease = Ease.Linear;
        
        public Vector3 Value => _value;
        public float Duration => _duration;
        public int Vibrato => _vibrato;
        public float Elasticity => _elasticity;
        public Ease Ease => _ease;

    }
}