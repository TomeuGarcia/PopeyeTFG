using System;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private MeshRenderer _mesh;
        private Material _meshMaterial;

        private int _tiredId;
        
        private void Awake()
        {
            _meshMaterial = _mesh.material;
            _tiredId = Shader.PropertyToID("_IsTired");
            
            
            EndTired();
        }


        public void StartTired()
        {
            _meshMaterial.SetFloat(_tiredId, 1.0f);
        }

        public void EndTired()
        {
            _meshMaterial.SetFloat(_tiredId, 0.0f);
        }
    }
}