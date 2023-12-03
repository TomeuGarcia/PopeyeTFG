using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public interface IHealthTarget
    {
        public void Heal(float healAmount);
        public void HealToMax();
    }
}


