using System.Collections;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spin
{
    public class AnchorSpinView : MonoBehaviour
    {
        [Header("TRAIL")]
        [SerializeField] private Transform _anchorSpinTrailHolder;
        [SerializeField] private TrailRenderer _anchorSpinTrail;
        
        [Header("PARTICLES")]
        [SerializeField] private Transform _centerChargeHolder;
        [SerializeField] private ParticleSystem _trailParticles;
        [SerializeField] private ParticleSystem _centerChargePreparationParticles;
        [SerializeField] private ParticleSystem _centerChargeParticles;
        [SerializeField] private ParticleSystem _frictionSparksParticles;
        [SerializeField] private AnimationCurve _sparksAmount;
        private ParticleSystem.EmissionModule _sparksEmission;

        private Coroutine _delayedEmissionStop;
        

        public void Configure(Transform spinOrigin)
        {
            SetHolderToSpinOrigin(spinOrigin, _centerChargeHolder);
            _sparksEmission = _frictionSparksParticles.emission;
        }

        private void SetHolderToSpinOrigin(Transform spinOrigin, Transform holder)
        {
            holder.parent = spinOrigin;
            holder.localPosition = Vector3.zero;
        }
        
        public void StartPreparationAnimation(Vector3 spinCenter)
        {
            _centerChargePreparationParticles.Play();
            _anchorSpinTrailHolder.position = spinCenter;
        }
        public void InterruptPreparationAnimation()
        {
            _centerChargePreparationParticles.Stop();
        }
        public void StartAnimation()
        {
            if (_delayedEmissionStop != null)
            {
                StopCoroutine(_delayedEmissionStop);
            }

            _anchorSpinTrail.Clear();
            _anchorSpinTrail.emitting = true;
            
            _centerChargePreparationParticles.Stop();
            _centerChargeParticles.Play();
            _frictionSparksParticles.Play();
            _trailParticles.Play();
        }
        public void FinishAnimation()
        {
            _delayedEmissionStop = StartCoroutine(DelayedEmissionStop());
            
            _centerChargeParticles.Stop();
        }
        public void UpdateAnimation(Vector3 spinCenter, Quaternion spinRotation, float spinRadius, float spinT)
        {
            Vector3 spinOffset = (Vector3.forward * spinRadius) + (Vector3.left * 0.5f);
            Vector3 position = spinCenter + (spinRotation * spinOffset); 
        
            _anchorSpinTrailHolder.position = position;
            
            _sparksEmission.rateOverDistance = _sparksAmount.Evaluate(spinT);
        }

        private IEnumerator DelayedEmissionStop()
        {
            yield return new WaitForSeconds(0.75f);
            
            _frictionSparksParticles.Stop();
            _trailParticles.Stop();
            
            _anchorSpinTrail.emitting = false;
            _anchorSpinTrail.Clear();

            _delayedEmissionStop = null;
        }
        
    }
}