using System;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectoryEndSpot : MonoBehaviour
    {
        [SerializeField] private Transform _spotTransform;
        [SerializeField] private TrajectoryEndSpotView _view;
        
        
        private void Awake()
        {
            _view.Configure();
        }

        private void OnDestroy()
        {
            _view.Finish();
        }
        

        public void MatchSpot(Vector3 position, Vector3 lookDirection, bool isValid)
        {
            SetValidState(isValid);

            _spotTransform.position = position + (lookDirection * 0.1f);

            _spotTransform.up = lookDirection;
            
            _view.SetTrajectoryContactPosition(position);
        }

        public void Show()
        {
            _view.Show();
        }
        public void Hide()
        {
            _view.Hide();
        }

        private void SetValidState(bool isValid)
        {
            _view.SetValid(isValid);
        }
    }
}