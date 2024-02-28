using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Popeye.Modules.Enemies
{
   public class ProximityTargetGetterBehaviour : AEnemy
   {
      [Header("PROXIMITY TARGET GETTER")]
      [SerializeField] private UnityTargetableEvent onTargetFound = new UnityTargetableEvent();
      [SerializeField] private UnityEvent onTargetLost = new UnityEvent();

      public TargetableBehaviour CurrentTarget { get; private set; }
      public bool HasTarget => CurrentTarget != null;

      private void OnTriggerEnter(Collider other)
      {
         if (HasTarget)
         {
            return;
         }

         if (!other.TryGetComponent(out TargetableBehaviour target))
         {
            return;
         }

         CurrentTarget = target;
         onTargetFound.Invoke(CurrentTarget);
      }

      private void OnTriggerExit(Collider other)
      {
         if (!HasTarget)
         {
            return;
         }

         if (!other.TryGetComponent(out TargetableBehaviour target))
         {
            return;
         }

         if (CurrentTarget != target)
         {
            return;
         }

         CurrentTarget = null;
         onTargetLost.Invoke();
      }

      public void Die()
      {
         InvokeOnDeathComplete();
      }

      internal override void Init()
      {
         throw new NotImplementedException();
      }

      internal override void Release()
      {
         throw new NotImplementedException();
      }

      public override void SetPatrollingWaypoints(Transform[] waypoints)
      {
         throw new NotImplementedException();
      }

      public override void DieFromOrder()
      {
         throw new NotImplementedException();
      }
   }
}
