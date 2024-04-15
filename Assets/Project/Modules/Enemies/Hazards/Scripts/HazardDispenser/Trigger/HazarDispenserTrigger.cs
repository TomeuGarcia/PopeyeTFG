using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HazarDispenserTrigger : MonoBehaviour
    {
        [SerializeField] private ObjectTypeTriggerEnter _triggerEnter;
        [SerializeField] private InterfaceReference<IHazardDispenser, MonoBehaviour>[] _hazardDispensers;

        private bool _isAlreadyWaitingToReenable;

        private void OnEnable()
        {
            _triggerEnter.OnGameObjectEnters += OnValidGameObjectEntersEvent;
        }
        private void OnDisable()
        {
            _triggerEnter.OnGameObjectEnters -= OnValidGameObjectEntersEvent;
        }


        private void OnValidGameObjectEntersEvent(GameObject gameObject)
        {
            foreach (var hazardDispenserReference in _hazardDispensers)
            {
                IHazardDispenser hazardDispenser = hazardDispenserReference.Value;
                if (hazardDispenser.CanDispense())
                {
                    hazardDispenser.StartDispensingHazard();
                    ReenableAfterCanDispense(hazardDispenser).Forget();
                }
            }
        }

        private async UniTaskVoid ReenableAfterCanDispense(IHazardDispenser hazardDispenser)
        {
            if (_isAlreadyWaitingToReenable) return;

            _triggerEnter.Disable();
            _isAlreadyWaitingToReenable = true;

            await UniTask.WaitUntil(hazardDispenser.CanDispense);

            _triggerEnter.Enable();
            _isAlreadyWaitingToReenable = false;
        }
        
    }
}