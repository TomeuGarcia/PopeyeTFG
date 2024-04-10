using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public class PopeyePlayerPlaceSpotUpdater : MonoBehaviour
    {
        [SerializeField] private PopeyePlayerPlaceSpot _popeyePlayerPlaceSpot;
        
        private void Awake()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            
            Destroy(this);
        }
        

#if UNITY_EDITOR
        private void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                _popeyePlayerPlaceSpot.UpdateView();
            }
        }
#endif
        
    }
}