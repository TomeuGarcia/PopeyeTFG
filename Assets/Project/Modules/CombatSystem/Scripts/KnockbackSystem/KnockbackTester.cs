using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Timers;
using Project.PhysicsMovement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class KnockbackTester : MonoBehaviour
    {
        [SerializeField] private PhysicsTweenerBehaviour _physicsTweener;
        [SerializeField] private PushableTestBehaviour _pushableTestBehaviour;
        [SerializeField] private Vector3 _push;
        [SerializeField] private Vector3 _position;
        [SerializeField] private float _duration;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                EnqueuePushObject_Displace(_pushableTestBehaviour, _duration, _push);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                EnqueuePushObject_Position(_pushableTestBehaviour, _duration, _position);
            }
        }


        private void EnqueuePushObject_Displace(PushableTestBehaviour pushableTestBehaviour, float duration, Vector3 push)
        {
            Vector3 startPosition = pushableTestBehaviour.Position;
            Vector3 endPosition = startPosition + push;

            _physicsTweener.AddObject(new PhysicsTweenObject(pushableTestBehaviour.Rigidbody, duration, 
                startPosition, endPosition));
            
            // TODO notify PushableTestBehaviour start push
        }
        
        private void EnqueuePushObject_Position(PushableTestBehaviour pushableTestBehaviour, float duration, Vector3 position)
        {
            Vector3 startPosition = pushableTestBehaviour.Position;
            Vector3 endPosition = position;
            
            _physicsTweener.AddObject(new PhysicsTweenObject(pushableTestBehaviour.Rigidbody, duration, 
                startPosition, endPosition));
            
            // TODO notify PushableTestBehaviour start push
        }
    }
}