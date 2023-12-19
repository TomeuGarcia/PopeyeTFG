using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Unity.Mathematics;
using UnityEngine;

namespace Popeye.Modules.Enemies.VFX
{
    public class EnemyVisuals : MonoBehaviour
    {
        //This is the base script, any unique additions to these
        //effects must be done by creating a new unique script
        //that inherits from this one.

        [System.Serializable]
        public class OriginalMeshData
        {
            public MeshRenderer _mesh;
            [HideInInspector] public Material _originalMaterial;
        }

        [SerializeField] private GeneralEnemyVFXConfig _visualConfig;

        [SerializeField] [Tooltip("First mesh on the list will be the one that gets 'hurt'")]
        private List<OriginalMeshData> _originalMeshDatas = new();

        private void Awake()
        {
            foreach (var data in _originalMeshDatas)
            {
                data._originalMaterial = data._mesh.material;
            }
        }

        public void Configure()
        {
            _originalMeshDatas[0]._mesh.material.SetFloat("_Health", 1.0f);
        }

        public virtual void PlayHitEffects(float healthCoef01)
        {
            _originalMeshDatas[0]._mesh.material.SetFloat("_Health", healthCoef01);
            
            //smallest camera shake


            CircleEffect().Forget();
            FlashEffect().Forget();
        }

        private async UniTaskVoid FlashEffect()
        {
            foreach (var flash in _visualConfig._flashSequence)
            {
                foreach (var data in _originalMeshDatas)
                {
                    data._mesh.material = flash._flashMaterial;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(flash._waitTime));
            }

            foreach (var data in _originalMeshDatas)
            {
                for (int i = 0; i < data._mesh.materials.Length; i++)
                {
                    data._mesh.material = data._originalMaterial;
                }
            }
        }
        
        private async UniTaskVoid CircleEffect()
        {
            //TODO delete
            Transform player = ServiceLocator.Instance.GetService<IGameReferences>().GetPlayer();
            Vector3 spawnPos = transform.position + (player.position - transform.position) * 0.3f;
            Transform _circle = Instantiate(_visualConfig._circlePrefab, spawnPos, quaternion.identity).transform;
            GameObject _particles = Instantiate(_visualConfig._particlesPrefab, spawnPos, quaternion.identity);
            
            _circle.LookAt(player);
            _particles.transform.LookAt(player);
            
            _circle.gameObject.SetActive(true);
            _particles.gameObject.SetActive(true);
            
            _circle.gameObject.SetActive(true);
            MeshRenderer circleMR = _circle.gameObject.GetComponent<MeshRenderer>();
            circleMR.material.SetFloat("_Alpha", 1.0f);

            _circle.localScale = new Vector3(_visualConfig._circleInterpolateData._startScale, _visualConfig._circleInterpolateData._startScale, _visualConfig._circleInterpolateData._startScale);
            _circle.DOScale(new Vector3(_visualConfig._circleInterpolateData._endScale, _visualConfig._circleInterpolateData._endScale, _visualConfig._circleInterpolateData._endScale), _visualConfig._circleInterpolateData._totalTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_visualConfig._circleInterpolateData._fadeOutDelay));
            circleMR.material.DOFloat(0.0f, "_Alpha", _visualConfig._circleInterpolateData._fadeOutTime);
        
            await UniTask.Delay(TimeSpan.FromSeconds(_visualConfig._circleInterpolateData._fadeOutTime + 0.2f));
            _circle.gameObject.SetActive(false);
        }

        public virtual void PlayDeathEffects()
        {
            CircleEffect().Forget();
        }
    }
}
