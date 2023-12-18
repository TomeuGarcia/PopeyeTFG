using Project.Scripts.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    [CreateAssetMenu(fileName = "PlayerMovesetConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerMovesetConfig")]
    public class PlayerMovesetConfig : ScriptableObject
    {
        [Header("DASH DISPlACEMENT")]
        [SerializeField] private Vector3 _dashExtraDisplacement = new Vector3(0.0f, 1.0f, 0.5f);
        [SerializeField] private Vector3 _snapExtraDisplacement = new Vector3(0.0f, 1.0f, 1.0f);
        
        public Vector3 DashExtraDisplacement => _dashExtraDisplacement;
        public Vector3 SnapExtraDisplacement => _snapExtraDisplacement;


        [Header("STAMINA COSTS")] 
        [SerializeField, Range(0, 100)] private int _anchorThrowStaminaCost = 0;
        [SerializeField, Range(0, 100)] private int _anchorPullStaminaCost = 0;
        [SerializeField, Range(0, 100)] private int _anchorPickUpStaminaCost = 0;
        [SerializeField, Range(0, 100)] private int _anchorDashStaminaCost = 20;
        [SerializeField, Range(0, 100)] private int _anchorKickStaminaCost = 20;
        [SerializeField, Range(0, 100)] private int _anchorSpinStaminaCost = 20;

        public int AnchorThrowStaminaCost => _anchorThrowStaminaCost;
        public int AnchorPullStaminaCost => _anchorPullStaminaCost;
        public int AnchorPickUpStaminaCost => _anchorPickUpStaminaCost;
        public int AnchorDashStaminaCost => _anchorDashStaminaCost;
        public int AnchorKickStaminaCost => _anchorKickStaminaCost;
        public int AnchorSpinStaminaCost => _anchorSpinStaminaCost;
    }
}