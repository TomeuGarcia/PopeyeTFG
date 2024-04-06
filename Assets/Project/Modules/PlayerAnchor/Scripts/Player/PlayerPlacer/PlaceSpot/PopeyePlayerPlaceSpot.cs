using System.Collections;
using AYellowpaper;
using NaughtyAttributes;
using Popeye.Scripts.EntityPlacing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    [RequireComponent(typeof(PopeyePlayerPlaceSpotInitializer))]
    public class PopeyePlayerPlaceSpot : MonoBehaviour
    {
        [Header("PREVIEWS CONFIGURATION")]
        [Expandable] [SerializeField] private PopeyePlayerPlaceSpotConfig _config;
        
        [Header("COMPONENTS")]
        [SerializeField] private WorldEntityPreviewComponents _playerPreview;
        [SerializeField] private WorldEntityPreviewComponents _anchorPreview;
        
        [Header("CONFIGURATION")]
        [SerializeField] private bool _startsCarryingAnchor = true;
        

        private IPlacePopeyePlayerEventChannelDispatcher _placeEventChannelDispatcher;
        private PopeyePlayerPlacingData _popeyePlayerPlacingData;
        
        
        
        private void Start()
        {
            
            //QueryPlacePlayerHere();
        }
        

        public void Configure(IPlacePopeyePlayerEventChannelDispatcher placeEventChannelDispatcher)
        {
            _placeEventChannelDispatcher = placeEventChannelDispatcher;

            _popeyePlayerPlacingData = new PopeyePlayerPlacingData
            {
                playerPosition = _playerPreview.PlaceTransform.position,
                playerRotation = _playerPreview.PlaceTransform.rotation,

                anchorPosition = _anchorPreview.PlaceTransform.position,
                anchorRotation = _anchorPreview.PlaceTransform.rotation,

                startCarryingAnchor = _startsCarryingAnchor
            };
            
            DestroyPreviews();
            //QueryPlacePlayerHere();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            QueryPlacePlayerHere();
        }
        
        private void QueryPlacePlayerHere()
        {
            
            _placeEventChannelDispatcher.RaiseEvent(_popeyePlayerPlacingData);
        }

        private void DestroyPreviews()
        {
            _playerPreview.DestroyView();
            _anchorPreview.DestroyView();
        }

        public void UpdateView()
        {
            if (_config && _playerPreview.HasAllReferences() && _anchorPreview.HasAllReferences())
            {
                _playerPreview.UpdateView(_config.PlayerViewData);
                _anchorPreview.UpdateView(_config.AnchorViewData);
            }
        }
    }
    
}