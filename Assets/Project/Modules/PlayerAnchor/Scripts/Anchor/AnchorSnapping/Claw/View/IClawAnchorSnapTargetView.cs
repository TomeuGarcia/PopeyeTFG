using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IClawAnchorSnapTargetView
    {
        void PlayAimedAnimation();
        void StopAimedAnimation();
        void PlayGrabAnimation(float delay);
        void PlayUsedAnimation();
        void PlayPulledAnimation();
        
        
    }
}