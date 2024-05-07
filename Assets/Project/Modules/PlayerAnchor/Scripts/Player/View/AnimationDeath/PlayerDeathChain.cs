using System.Collections;
using Cysharp.Threading.Tasks;
using Popeye.InverseKinematics.Bones;
using Popeye.InverseKinematics.FABRIK;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AnimationDeath
{
    public class PlayerDeathChain : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [SerializeField] private FABRIKControllerBehaviour _FABRIKControllerBehaviour;
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private Transform _chainTarget;
        [SerializeField] private Transform _particlesHolder;
        [SerializeField] private ParticleSystem _chainTopParticles;
        
        [Header("CONFIG")]
        [SerializeField, Range(0.0f, 5.0f)] private float _delay = 0f;
        [SerializeField] private TweenEaseConfig _toTargetEase;

        [Header("CAMERA")] 
        [SerializeField] private CameraShakeConfig _appearEndShake;
        private ICameraFunctionalities _cameraFunctionalities;

        public float Delay => _delay;

        public void Init(ICameraFunctionalities cameraFunctionalities)
        {
            _cameraFunctionalities = cameraFunctionalities;
            
            _boneChain.StartInit();
            _FABRIKControllerBehaviour.StartInit(_boneChain, _chainTarget);
            
            _chainTopParticles.Stop();
        }

        public void ResetState(Transform goalTarget)
        {
            float chainDistance = _boneChain.BoneLength * _boneChain.NumberOfBones;
            Vector3 rootPosition = _boneChain.Bones[0].Position;
            Vector3 direction = (goalTarget.position - rootPosition).normalized;
            
            _chainTarget.position = rootPosition + (direction * chainDistance);
            _boneChain.Hide();
            
            _chainTopParticles.Stop();
        }

        public IEnumerator MoveToTarget(Transform goalTarget)
        {
            _boneChain.Show();
            _chainTopParticles.Play();
        
            Timer moveTimer = new Timer(_toTargetEase.Duration);


            Vector3 originPosition = _chainTarget.position;

            while (!moveTimer.HasFinished())
            {            
                moveTimer.Update(Time.deltaTime);

                float r = moveTimer.GetCounterRatio01();
                float t = _toTargetEase.EaseCurve.Evaluate(r);

                Vector3 toTarget = (goalTarget.position - originPosition).normalized;

                _particlesHolder.up = -toTarget;
                
                Vector3 currentPosition = Vector3.LerpUnclamped(originPosition, goalTarget.position, t);
                
                _chainTarget.position = currentPosition;            
                
                yield return null;
            }

            _cameraFunctionalities.CameraShaker.PlayShake(_appearEndShake);
        }

        public void ChangeToDeath()
        {
            
        }
        
    }
}