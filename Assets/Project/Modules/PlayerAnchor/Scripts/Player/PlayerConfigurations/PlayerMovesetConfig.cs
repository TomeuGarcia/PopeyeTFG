using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    [CreateAssetMenu(fileName = "PlayerMovesetConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerMovesetConfig")]
    public class PlayerMovesetConfig : ScriptableObject
    {
        [Header("DASH")]
        [SerializeField] private Vector3 _dashExtraDisplacement = new Vector3(0.0f, 1.0f, 0.5f);
        [SerializeField] private Vector3 _snapExtraDisplacement = new Vector3(0.0f, 1.0f, 1.0f);
        
        public Vector3 DashExtraDisplacement => _dashExtraDisplacement;
        public Vector3 SnapExtraDisplacement => _snapExtraDisplacement;

        
        [Header("ROLL")]
        [SerializeField, Range(0.0f, 20.0f)] private float _rollDistance = 6.0f;
        public float RollDistance => _rollDistance;
        
        

        [Header("STAMINA COSTS")] 
        [SerializeField, Range(0, 100)] private int _anchorThrowStaminaCost = 0;
        [SerializeField, Range(0, 100)] private int _anchorPullStaminaCost = 0;
        [SerializeField, Range(0, 100)] private int _anchorPickUpStaminaCost = 0;
        [SerializeField, Range(0, 100)] private int _anchorDashStaminaCost = 20;
        [SerializeField, Range(0, 100)] private int _rollStaminaCost = 20;
        [SerializeField, Range(0, 100)] private int _anchorKickStaminaCost = 20;
        [SerializeField, Range(0, 100)] private int _anchorSpinPerSecondStaminaCost = 10;

        public int AnchorThrowStaminaCost => _anchorThrowStaminaCost;
        public int AnchorPullStaminaCost => _anchorPullStaminaCost;
        public int AnchorPickUpStaminaCost => _anchorPickUpStaminaCost;
        public int AnchorDashStaminaCost => _anchorDashStaminaCost;
        public int RollStaminaCost => _rollStaminaCost;
        public int AnchorKickStaminaCost => _anchorKickStaminaCost;
        public int AnchorSpinPerSecondStaminaCost => _anchorSpinPerSecondStaminaCost;
    }
}