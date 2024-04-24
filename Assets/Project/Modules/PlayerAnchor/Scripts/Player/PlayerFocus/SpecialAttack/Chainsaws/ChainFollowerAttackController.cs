using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Chain;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Chainsaws
{
    public class ChainFollowerAttackController : MonoBehaviour, IPlayerSpecialAttackController
    {
        [SerializeField] private float _duration = 3.0f;
        [SerializeField] private AnimationCurve _translationEase = AnimationCurve.EaseInOut(0,0,1,1);
        [SerializeField] private TweenConfig _scaleUpTween;
        [SerializeField] private TweenConfig _scaleDownTween;


        [SerializeField] private DamageHitConfig _damageHitConfig;
        [SerializeField] private DamageTrigger[] _damageTriggers;
        [SerializeField] private Transform _translationTransform;
        
        
        private bool _isBeingPerformed = false;
        
        private IPlayerFocusSpender _focusSpender;
        private PlayerFocusAttackConfig _focusAttackConfig;
        private IAnchorMediator _anchorMediator;
        [SerializeField] private AnchorChain _anchorChain;

        private void Awake()
        {
            _translationTransform.localScale = Vector3.zero;
        }
        
        private void OnDestroy()
        {
            foreach (DamageTrigger damageTrigger in _damageTriggers)
            {
                damageTrigger.OnDamageDealt -= _anchorMediator.OnDamageDealt;
            }
        }

        
        public void Configure(IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IAnchorMediator anchorMediator)
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorMediator = anchorMediator;


            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            foreach (DamageTrigger damageTrigger in _damageTriggers)
            {
                damageTrigger.Configure(combatManager, new DamageHit(_damageHitConfig));
                damageTrigger.Deactivate();
                
                damageTrigger.OnDamageDealt += _anchorMediator.OnDamageDealt;

            }
        }
        
        
        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed() &&
                   !_anchorMediator.IsBeingCarried();
        }

        public bool SpecialAttackIsBeingPerformed()
        {
            return _isBeingPerformed;
        }

        public void StartSpecialAttack()
        {
            //_focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);
        
            DoSpecialAttack().Forget();
        }
        
        public bool SpecialAttackHasFinished()
        {
            return !_isBeingPerformed;
        }
        
        
        private async UniTaskVoid DoSpecialAttack()
        {
            _isBeingPerformed = true;
            
            foreach (DamageTrigger damageTrigger in _damageTriggers)
            {
                damageTrigger.Activate();
            }

            _translationTransform.Scale(_scaleUpTween);
            

            Timer translationTimer = new Timer(_duration);
            while (!translationTimer.HasFinished())
            {                
                translationTimer.Update(Time.deltaTime);

                float t = _translationEase.Evaluate(translationTimer.GetCounterRatio01());
                ComputeTranslation(t, out Vector3 position, out Quaternion rotation);
                _translationTransform.position = position;

                rotation = Quaternion.RotateTowards(_translationTransform.rotation, rotation, 200 * Time.deltaTime);
                _translationTransform.rotation = rotation;
                
                await UniTask.Yield();
            }

            await _translationTransform.Scale(_scaleDownTween)
                .AsyncWaitForCompletion();



            _isBeingPerformed = false;
            
            foreach (DamageTrigger damageTrigger in _damageTriggers)
            {
                damageTrigger.Deactivate();
            }            
        }


        private void ComputeTranslation(float translationT, out Vector3 position, out Quaternion rotation)
        {
            Vector3[] chainPositions = _anchorChain.GetChainPositions();
            Array.Reverse(chainPositions);
            
            int indicesAmount = chainPositions.Length - 2;

            float current = indicesAmount * translationT;
            int previousIndex = (int)current;
            int nextIndex = previousIndex + 1;

            float currentT = current % 1f;

            Vector3 previousPosition = chainPositions[previousIndex];
            Vector3 nextPosition = chainPositions[nextIndex];
            
            position = Vector3.LerpUnclamped(previousPosition, nextPosition, currentT);


            Vector3 direction = (position - previousPosition);
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            
            rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        
    }
}