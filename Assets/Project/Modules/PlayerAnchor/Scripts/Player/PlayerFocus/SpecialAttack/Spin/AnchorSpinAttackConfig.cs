using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spin
{
    [CreateAssetMenu(fileName = "AnchorSpinAttackConfig", 
        menuName = ScriptableObjectsHelper.PLAYERSPECIALATTACKS_ASSETS_PATH + "AnchorSpinAttackConfig")]
    public class AnchorSpinAttackConfig : ScriptableObject
    {
        [Header("SPIN LOOPS")]
        [SerializeField] private int _numberOfLoops = 2;
        
        [Header("DURATIONS")]
        [SerializeField] private float _totalDuration = 1.0f;
        [SerializeField] private float _startPositioningDuration = 0.2f;
        [SerializeField] private float _endPositioningDuration = 0.15f;
        
        [Header("DISTANCES")]
        [SerializeField] private float _startSpinDistance = 4.0f;
        [SerializeField] private float _endSpinDistance = 7.0f;
        
        public int NumberOfLoops => _numberOfLoops;
        public float TotalDuration => _totalDuration;
        public float StartPositioningDuration => _startPositioningDuration;
        public float EndPositioningDuration => _endPositioningDuration;
        
        public float StartSpinDistance => _startSpinDistance;
        public float EndSpinDistance => _endSpinDistance;
    }
}