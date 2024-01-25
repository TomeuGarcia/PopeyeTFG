using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimTargetFinder_DirectReferences : IAutoAimTargetFinder
    {
        private IAutoAimTarget[] _autoAimTargets;

        
        
        public void Configure(Transform referencesParent)
        {
            _autoAimTargets = new IAutoAimTarget[referencesParent.childCount];
            
            for (int i = 0; i < referencesParent.childCount; ++i)
            {
                _autoAimTargets[i] = referencesParent.GetChild(i).GetComponent<IAutoAimTarget>();
            }  
        }
        
        
        public bool GetAutoAimTargetsData(out IAutoAimTarget[] targets)
        {
            targets = _autoAimTargets;
            return targets.Length > 0;
        }
    }
}