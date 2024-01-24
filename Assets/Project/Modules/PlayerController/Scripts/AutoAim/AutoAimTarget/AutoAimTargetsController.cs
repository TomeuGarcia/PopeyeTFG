using System;
using Project.Modules.PlayerController.Testing.AutoAim.Scripts;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetsController
    {
        private IAutoAimTargetFinder _autoAimTargetFinder;
        private IAutoAimTargetToDataConverter _autoAimTargetToDataConverter;
        
        private AutoAimTargetData[] _autoAimTargetsData;
        
        
        public void Configure(IAutoAimTargetFinder autoAimTargetFinder,
                              IAutoAimTargetToDataConverter autoAimTargetToDataConverter)
        {
            _autoAimTargetFinder = autoAimTargetFinder;
            _autoAimTargetToDataConverter = autoAimTargetToDataConverter;
        }
        
        
        public bool Update()
        {
             bool foundTargets = _autoAimTargetFinder.GetAutoAimTargets(out IAutoAimTarget[] autoAimTargets);
             if (!foundTargets)
             {
                 return false;
             }
             
             _autoAimTargetsData = _autoAimTargetToDataConverter.Convert(autoAimTargets);
             SortByAngularPosition();

             return true;
        }

        public AutoAimTargetData[] GetAimTargetsData()
        {
            return _autoAimTargetsData;
        }

        private void SortByAngularPosition()
        {
            Array.Sort(_autoAimTargetsData, 
                (a, b) => 
                    a.AngularPosition < b.AngularPosition ? 0 : 1);
        }
    }
}