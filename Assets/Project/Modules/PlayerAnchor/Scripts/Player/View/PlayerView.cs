using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private MeshRenderer _mesh;
        private Material _meshMaterial;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _damagedColor;
        [SerializeField] private Color _healColor;
        
        private int _tiredId;
        private int _baseColorId;
        
        private void Awake()
        {
            _meshMaterial = _mesh.material;
            _tiredId = Shader.PropertyToID("_IsTired");
            _baseColorId = Shader.PropertyToID("_BaseColor");
            
            
            EndTired();
            SetMeshBaseColor(_normalColor);
        }


        public void StartTired()
        {
            _meshMaterial.SetFloat(_tiredId, 1.0f);
        }

        public void EndTired()
        {
            _meshMaterial.SetFloat(_tiredId, 0.0f);
        }

        public void PlayTakeDamageAnimation()
        {
            FlickBaseColor(3, 0.2f, _damagedColor).Forget();
        }

        public void PlayDeathAnimation()
        {
            SetMeshBaseColor(_damagedColor);
        }

        public async UniTask PlayHealAnimation()
        {
            await FlickBaseColor(2, 0.15f, _healColor);
        }

        private async UniTask FlickBaseColor(int numberOfFlicks, float flickDuration, Color flickColor)
        {
            flickDuration /= 2;
            for (int i = 0; i < numberOfFlicks; ++i)
            {
                SetMeshBaseColor(flickColor);
                await UniTask.Delay(TimeSpan.FromSeconds(flickDuration));
                SetMeshBaseColor(_normalColor);
                await UniTask.Delay(TimeSpan.FromSeconds(flickDuration));
            }
        }

        private void SetMeshBaseColor(Color color)
        {
            _meshMaterial.SetColor(_baseColorId, color);
        }
    }
}