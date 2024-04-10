using System;
using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class PhysicsCastRequirementsProcessor
    {
        private readonly IPhysicsCastRequirement[] _requirements;
        
        public PhysicsCastRequirementsProcessor()
        {
            _requirements = Array.Empty<IPhysicsCastRequirement>();
        }
        public PhysicsCastRequirementsProcessor(IPhysicsCastRequirement[] requirements)
        {
            _requirements = requirements;
        }

        public bool HitMeetsAllRequirements(RaycastHit hit)
        {
            foreach (IPhysicsCastRequirement requirement in _requirements)
            {
                if (!requirement.HitMeetsRequirement(hit))
                {
                    return false;
                }
            }

            return true;
        }
    }
}