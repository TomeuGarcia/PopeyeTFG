using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Popeye.Modules.Enemies.Hazards
{
    public class HazardDispenser : MonoBehaviour, IHazardDispenser
    {
        
        [SerializeField] private HazardDispenserConfig _config;
        [SerializeField] private AHazardSpawner _hazardSpawner;
        [SerializeField] private Transform _hazardSpawn;
        [SerializeField] private InterfaceReference<IHazardDispenserView, MonoBehaviour> _view;
        private IHazardDispenserView View => _view.Value;
        private IHazardDispenserAudio Audio => _config.HazardDispenserAudio;
        
        

        private bool _isPreparingDispense;
        private bool _isOnCooldown;


        private void Start()
        {
            View.Configure(_config.ViewConfig);
        }

        public bool CanDispense()
        {
            return !_isPreparingDispense && !_isOnCooldown;
        }

        public void StartDispensingHazard()
        {
            DispenseHazard().Forget();
        }


        private async UniTaskVoid DispenseHazard()
        {
            _isPreparingDispense = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_config.DelayBeforeDispensing));
            Audio.PlayPrepareSound(gameObject);
            View.PlayPrepareDispensingAnimation(_config.TelegraphBeforeDispensing);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.TelegraphBeforeDispensing));
            
            
            _isPreparingDispense = false;
            _isOnCooldown = true;

            Audio.PlayDispenseSound(gameObject);
            View.PlayDispenseAnimation();
            _hazardSpawner.Spawn(_hazardSpawn.position, _hazardSpawn.rotation);

            
            await UniTask.Delay(TimeSpan.FromSeconds(_config.CooldownAfterDispensing));
            
            
            await View.PlayReadyToDispenseAnimation();
            _isOnCooldown = false;
        }
        
        
        
    }
}