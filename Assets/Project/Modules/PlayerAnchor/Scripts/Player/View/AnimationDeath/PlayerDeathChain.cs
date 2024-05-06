using System.Collections;
using Cysharp.Threading.Tasks;
using Popeye.InverseKinematics.Bones;
using Popeye.InverseKinematics.FABRIK;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AnimationDeath
{
    [System.Serializable]
    public class PlayerDeathChain
    {
        [SerializeField] private TweenEaseConfig _toTargetEase;
        
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private Transform _chainTarget;

        public void ResetState()
        {
            _chainTarget.position = _boneChain.Bones[0].Position;
            _boneChain.Hide();
        }

        public IEnumerator MoveToTarget(Transform target)
        {
            _boneChain.Show();
        
            Timer moveTimer = new Timer(_toTargetEase.Duration);


            Vector3 originPosition = _chainTarget.position;

            while (!moveTimer.HasFinished())
            {            
                moveTimer.Update(Time.deltaTime);
                
                float t = _toTargetEase.EaseCurve.Evaluate(moveTimer.GetCounterRatio01());
                
                _chainTarget.position = Vector3.LerpUnclamped(originPosition, target.position, t);                
                
                yield return null;
            }            
        }
        
    }
}