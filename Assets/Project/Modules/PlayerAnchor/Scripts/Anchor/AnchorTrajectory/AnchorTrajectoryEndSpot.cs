using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectoryEndSpot : MonoBehaviour
    {
        [SerializeField] private Transform _spotTransform;

        
        public void MatchSpot(Vector3 position, Vector3 hitNormal)
        {
            _spotTransform.position = position;
            
            if (Vector3.Dot(hitNormal, Vector3.up) > 0.95f)
            {
                _spotTransform.rotation = Quaternion.identity;
            }
            else
            {
                Vector3 right = Vector3.Cross(hitNormal, Vector3.up).normalized;
                Quaternion look = Quaternion.LookRotation(hitNormal, right);
                _spotTransform.rotation = look;
            }
            
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}