using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    [CreateAssetMenu(fileName = "DestructiblePropConfig_NAME", 
        menuName = ScriptableObjectsHelper.COMBATSYSTEM_PATH + "DestructiblePropConfig")]
    public class DestructiblePropConfig : ScriptableObject
    {
        [Header("HEALTH")] 
        [SerializeField, Range(0, 100)] private int _maxHealth = 1;
        [SerializeField, Range(0.0f, 1.0f)] private float _knockbackResistance = 0.0f;
        
        [Header("VIEW")] 
        [SerializeField] private ViewConfigData _viewConfig;
        
        [Header("AUDIO")] 
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _destroyedSound;
        

        [System.Serializable]
        public class ViewConfigData
        {
            [Header("Death")] 
            [SerializeField] private TweenPunchConfig _deathPunchScale;
            [SerializeField] private TweenConfig _deathRotation;
        
            [Header("Take Damage")] 
            [SerializeField] private TweenPunchConfig _takeDamagePunchScale;
            
            public TweenPunchConfig DeathPunchScale => _deathPunchScale;
            public TweenConfig DeathRotation => _deathRotation;
            public TweenPunchConfig TakeDamagePunchScale => _takeDamagePunchScale;
        }
        
        
        public int MaxHealth => _maxHealth;
        public float KnockbackResistance => _knockbackResistance;
        public ViewConfigData ViewConfig => _viewConfig;


        public void PlayDestroyedSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_destroyedSound, source);
        }

    }
}