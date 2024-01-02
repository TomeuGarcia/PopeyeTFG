using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class AnchorToggleablePressurePlate : AnchorPressurePlate
    {

        protected override bool CanBeTriggered(DamageHit damageHit)
        {
            /*
            if (!_collider.bounds.Contains(damageHit.Position))
            {
                return false;
            }
            */

            return damageHit.Damage > 10;
        }

        protected override void OnTakeAnchorHit()
        {
            if (_isTriggered)
            {
                PlayUntriggerAnimation();
                _isTriggered = false;

                DeactivateWorldInteractors();

            }
            else
            {
                PlayTriggerAnimation();
                _isTriggered = true;

                ActivateWorldInteractors();
            }



        }



    }
}