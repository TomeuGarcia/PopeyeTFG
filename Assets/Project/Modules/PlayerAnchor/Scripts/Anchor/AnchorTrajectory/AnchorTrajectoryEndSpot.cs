using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
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
                _spotTransform.position += _toTarget.normalized * (_followSpeed * Time.deltaTime * _toTarget.magnitude);
            }
        }

        public void MatchSpot(Vector3 position, Vector3 hitNormal, bool isValid)
        {
            SetValidState(isValid);
            if (Vector3.Distance(_spotTransform.position, position) < _stopFollowDistance)
            {
                _updatePosition = false;
                return;
            }

            _updatePosition = true;
            _toTarget = position - _spotTransform.position;

            if (Vector3.Dot(hitNormal, Vector3.up) > 0.95f)
            {
                _spotTransform.rotation = Quaternion.identity;
            }
            else
            {
                Vector3 right = Vector3.Cross(hitNormal, Vector3.up).normalized;
                Vector3 forward = Vector3.Cross(hitNormal, right).normalized;
                Quaternion look = Quaternion.LookRotation(forward, hitNormal);
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