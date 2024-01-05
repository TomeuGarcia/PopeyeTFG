using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public interface IEnemyAnimator
    {
        public  void PlayTakeDamage();

        public  void PlayDeath();

        public void PlayMove();
    }
}
