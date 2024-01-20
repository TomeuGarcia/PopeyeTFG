using System.Collections;
using Popeye.Core.Services.ServiceLocator;
using Project.Scripts.Time.TimeFunctionalities;
using UnityEngine;

namespace Project.Scripts.Time.TimeScale
{
    public class TimeScaleDebugBehaviour : MonoBehaviour
    {
        [SerializeField] private TimeScaleDebugConfig _config;
        private ITimeScaleManager _timeScaleManager;
        
        private void Start()
        {
#if UNITY_EDITOR
            _timeScaleManager = ServiceLocator.Instance.GetService<ITimeFunctionalities>().TimeScaleManager;
#else
            if (_config.IgnoreOnBuild)
            {
                Destroy(this);
            }
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(_config.NormalTimeKeyCode))
            {
                _timeScaleManager.SetTimeScale(_config.NormalTimeScale);
            }
            else if (Input.GetKeyDown(_config.SlowTimeKeyCode))
            {
                _timeScaleManager.SetTimeScale(_config.SlowTimeScale);
            }
            else if (Input.GetKeyDown(_config.FastTimeKeyCode))
            {
                _timeScaleManager.SetTimeScale(_config.FastTimeScale);
            }
        }
    }
}