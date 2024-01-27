using System;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetingController
    {
        private IAutoAimTargetFinder _targetFinder;
        private IAutoAimTargetToResultConverter _targetToResultConverter;
        private IAutoAimTargetResultsFilterer _targetResultsFilterer;

        private Transform _targeter;
        
        private AutoAimTargetResult[] _targetResults;

        private Vector3 TargeterPosition => _targeter.position;
        
        
        public void Configure(IAutoAimTargetFinder targetFinder, IAutoAimTargetToResultConverter targetToResultConverter,
            IAutoAimTargetResultsFilterer targetResultsFilterer, Transform targeter)
        {
            _targetFinder = targetFinder;
            _targetToResultConverter = targetToResultConverter;
            _targetResultsFilterer = targetResultsFilterer;
            _targeter = targeter;
        }
        
        
        public bool Update(Vector3 forwardDirection, Vector3 rightDirection)
        {
             bool foundTargets = _targetFinder.GetAutoAimTargetsData(out IAutoAimTarget[] autoAimTargets);
             if (!foundTargets)
             {
                 return false;
             }

             _targetResults = _targetToResultConverter.Convert(autoAimTargets, forwardDirection, rightDirection);
             _targetResults = _targetResultsFilterer.Filter(_targetResults, TargeterPosition);
             
             SortByAngularPosition();

             return true;
        }

        public AutoAimTargetResult[] GetAimTargetsData()
        {
            return _targetResults;
        }
        

        private void SortByAngularPosition()
        {
            Array.Sort(_targetResults, 
                (a, b) => 
                    a.AngularPosition < b.AngularPosition ? 0 : 1);
        }
    }
}