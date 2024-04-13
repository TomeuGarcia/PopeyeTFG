using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAnimatorEvents : MonoBehaviour
    {
        private readonly List<IPlayerFootstepsListener> _footstepsListeners = new (1);
        

        public void AddFootstepsListener(IPlayerFootstepsListener footstepsListener)
        {
            _footstepsListeners.Add(footstepsListener);
        }
        

        private void OnLeftFootstep()
        {
            foreach (IPlayerFootstepsListener footstepsListener in _footstepsListeners)
            {
                footstepsListener.OnLeftFootstep();
            }
        }
        public void OnRightFootstep()
        {
            foreach (IPlayerFootstepsListener footstepsListener in _footstepsListeners)
            {
                footstepsListener.OnRightFootstep();
            }
        }
        
    }
}