using System;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectoryEndSpot : MonoBehaviour
    {
        [SerializeField] private Transform _spotTransform;
        [SerializeField, Range(0.0f, 100.0f)] private float _followSpeed = 80.0f;
        [SerializeField, Range(0.0f, 5.0f)] private float _stopFollowDistance = 0.1f;
        [SerializeField] private TrajectoryEndSpotView _view;

        private Vector3 _toTarget;
        private bool _updatePosition;


        private void Awake()
        {
            _view.Configure();
        }

        private void Update()
        {
            if (_updatePosition)
            {
                Vector3 moveDisplacement = 
                    _toTarget.normalized * (_followSpeed * Time.deltaTime * _toTarget.sqrMagnitude);
                
                //_spotTransform.position += moveDisplacement;
                _spotTransform.position += _toTarget;
                _toTarget = Vector3.zero; // will need to remove this if we want to move with acceleration;
            }
        }

        public void MatchSpot(Vector3 position, Vector3 lookDirection, bool isValid)
        {
            SetValidState(isValid);

            _toTarget = position - _spotTransform.position;
            _toTarget = Vector3.ClampMagnitude(_toTarget, 10.0f);
            
            _updatePosition = _toTarget.magnitude > _stopFollowDistance;

            if (Vector3.Dot(lookDirection, Vector3.up) > 0.95f)
            {
                _spotTransform.rotation = Quaternion.identity;
            }
            else
            {
                Vector3 right = Vector3.Cross(lookDirection, Vector3.up).normalized;
                Vector3 forward = Vector3.Cross(lookDirection, right).normalized;
                Quaternion look = Quaternion.LookRotation(forward, lookDirection);
                _spotTransform.rotation = look;
            }
            
        }

        public void Show()
        {
            _view.Show();
        }
        public void Hide()
        {
            _view.Hide();
            _updatePosition = false;
        }

        private void SetValidState(bool isValid)
        {
            _view.SetValid(isValid);
        }
    }
}