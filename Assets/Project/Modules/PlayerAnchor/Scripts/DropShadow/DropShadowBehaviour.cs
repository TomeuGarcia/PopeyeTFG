using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.DropShadow
{
    public class DropShadowBehaviour : MonoBehaviour
    {
        [SerializeField] private DropShadowConfig _dropShadowConfig;
        [SerializeField] private Transform _ownerTransform;
        private bool _transitioning = false;
        
        
        private void Update()
        {
            UpdateShadow();
        }


        private void UpdateShadow()
        {
            if (Physics.Raycast(_ownerTransform.position, Vector3.down,  out RaycastHit hit,
                    100, _dropShadowConfig.ObstacleLayerMask, QueryTriggerInteraction.Ignore))
            {
                SetPosition(hit.point, hit.normal);
                SetRotation(hit.normal);
                if (!_transitioning) SetSize(hit.distance);
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }

        private void SetPosition(Vector3 position, Vector3 normal)
        {
            transform.position = position + (normal * _dropShadowConfig.DisplacementFromFloor);
        }
        private void SetRotation(Vector3 normal)
        {
            transform.up = normal;
        }
        private void SetSize(float distance)
        {
            transform.localScale = Vector3.one * _dropShadowConfig.GetSizeFromDistance(distance);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            DoTransition(_dropShadowConfig.HideDuration, true, Vector3.zero, Vector3.one);
        }
        public void Hide()
        {
            DoTransition(_dropShadowConfig.HideDuration, false, transform.localScale, Vector3.zero);
        }

        private void DoTransition(float duration, bool endActive, Vector3 startScale, Vector3 endScale)
        {
            _transitioning = true;
            
            DOTween.To(
                () => startScale,
                (scale) =>
                {
                    startScale = scale;
                    transform.localScale = scale;
                },
                endScale,
                duration
            ).OnComplete(() =>
            {
                gameObject.SetActive(endActive);
                _transitioning = false;
            });
        }
        
        
    }
}