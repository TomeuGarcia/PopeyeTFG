using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class FlatStraightProjectileViewConfig
    {
        [SerializeField] private TweenPunchConfig _startShootScalePunch;
        [SerializeField] private TweenPunchConfig _objectContactScalePunch;
        [SerializeField] private TweenConfig _disappearScale;
        
        public TweenPunchConfig StartShootScalePunch => _startShootScalePunch;
        public TweenPunchConfig ObjectContactScalePunch => _objectContactScalePunch;
        public TweenConfig DisappearScale => _disappearScale;
    }
}