using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HazardDispenserView : MonoBehaviour, IHazardDispenserView
    {
        [Header("COMPONENTS")]
        [SerializeField] private Transform _spitterHolder;
        private HazardDispenserViewConfig _config;
        
        public void Configure(HazardDispenserViewConfig config)
        {
            _config = config;
        }

        public void PlayPrepareDispensingAnimation(float duration)
        {
            _spitterHolder.DOComplete();
            _spitterHolder.DOBlendableLocalRotateBy(_config.PrepareRotation.Value, duration)
                .SetEase(_config.PrepareRotation.Ease);
            _spitterHolder.DOScale(_config.PrepareScale.Value, duration)
                .SetEase(_config.PrepareScale.Ease);
        }

        public void PlayDispenseAnimation()
        {
            _spitterHolder.DOComplete();
            //_spitterHolder.localScale = Vector3.one;
            _spitterHolder.DOLocalRotateQuaternion(Quaternion.identity, _config.DispenseScalePunch.Duration)
                .SetEase(Ease.InOutSine);
            _spitterHolder.PunchScale(_config.DispenseScalePunch);
        }

        public async UniTask PlayReadyToDispenseAnimation()
        {
            _spitterHolder.PunchRotation(_config.ReadyRotationPunch);
            await _spitterHolder.DOScale(Vector3.one, _config.ReadyRotationPunch.Duration)
                .SetEase(Ease.InOutSine)
                .AsyncWaitForCompletion();
        }
    }
}