using System;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargettingController
    {
        private IAutoAimTargetFinder _autoAimTargetFinder;
        private IAutoAimTargetToResultConverter _autoAimTargetToResultConverter;
        private IAutoAimTargetResultsFilterer _autoAimTargetResultsFilterer;

        private Transform _targeter;
        
        private AutoAimTargetResult[] _autoAimTargetResults;

        private Vector3 TargeterPosition => _targeter.position;
        
        
        public void Configure(IAutoAimTargetFinder autoAimTargetFinder, IAutoAimTargetToResultConverter autoAimTargetToResultConverter,
            IAutoAimTargetResultsFilterer autoAimTargetResultsFilterer, Transform targeter)
        {
            _autoAimTargetFinder = autoAimTargetFinder;
            _autoAimTargetToResultConverter = autoAimTargetToResultConverter;
            _autoAimTargetResultsFilterer = autoAimTargetResultsFilterer;
            _targeter = targeter;
        }
        
        
        public bool Update(Vector3 forwardDirection, Vector3 rightDirection)
        {
             bool foundTargets = _autoAimTargetFinder.GetAutoAimTargetsData(out IAutoAimTarget[] autoAimTargets);
             if (!foundTargets)
             {
                 return false;
             }

             _autoAimTargetResults = _autoAimTargetToResultConverter.Convert(autoAimTargets, forwardDirection, rightDirection);
             _autoAimTargetResults = _autoAimTargetResultsFilterer.Filter(_autoAimTargetResults, TargeterPosition);
             
             SortByAngularPosition();

             return true;
        }

        public AutoAimTargetResult[] GetAimTargetsData()
        {
            return _autoAimTargetResults;
        }
        

        private void SortByAngularPosition()
        {
            Array.Sort(_autoAimTargetResults, 
                (a, b) => 
                    a.AngularPosition < b.AngularPosition ? 0 : 1);
        }
    }
}