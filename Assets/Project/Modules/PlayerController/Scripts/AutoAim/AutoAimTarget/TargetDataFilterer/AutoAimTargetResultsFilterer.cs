using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetResultsFilterer : IAutoAimTargetResultsFilterer
    {
        private AutoAimTargetResultFiltererConfig _config;
        
        public void Configure(AutoAimTargetResultFiltererConfig config)
        {
            _config = config;
        }
        
        public AutoAimTargetResult[] Filter(AutoAimTargetResult[] targetsResults, Vector3 targeterPosition)
        {
            List<AutoAimTargetResult> filteredTargetResults = new List<AutoAimTargetResult>(targetsResults.Length);

            
            for (int i = 0; i < targetsResults.Length; ++i)
            {
                AutoAimTargetResult targetToCheck = targetsResults[i];

                bool canBeAdded = true;
                for (int j = 0; j < filteredTargetResults.Count; ++j)
                {
                    AutoAimTargetResult alreadyFilteredTarget = filteredTargetResults[j];

                    if (!TargetsOccludeEachOther(targetToCheck, alreadyFilteredTarget))
                    {
                        continue;
                    }
                    
                    canBeAdded = false;

                    if (IsFirstTargetClosestToTargeter(alreadyFilteredTarget, targetToCheck, targeterPosition))
                    {
                        continue;
                    }
                    
                    filteredTargetResults[j] = targetToCheck;
                    break;
                }

                if (canBeAdded)
                {
                    filteredTargetResults.Add(targetToCheck);
                }
            }


            return filteredTargetResults.ToArray();
        }
        
        
        private bool TargetsOccludeEachOther(AutoAimTargetResult targetA, AutoAimTargetResult targetB)
        {
            return Mathf.Abs(targetA.AngularPosition - targetB.AngularPosition) <
                   ((targetA.HalfAngularTargetRegion + targetA.HalfAngularTargetRegion) / 2); // average half
                   //_config.AngularDistanceToDiscard; // fixed angular distance
        }

        private bool IsFirstTargetClosestToTargeter(AutoAimTargetResult firstTarget, AutoAimTargetResult secondTarget,
            Vector3 targeterPosition)
        {
            return (targeterPosition - firstTarget.Position).sqrMagnitude <
                   (targeterPosition - secondTarget.Position).sqrMagnitude;
        }
        
    }
}