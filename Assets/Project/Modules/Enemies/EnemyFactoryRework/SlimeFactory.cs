using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.Enemies.Slime;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class SlimeFactory
    {
        private Dictionary<SlimeSizeID, ObjectPool> _slimeSizeToPool;
        private Dictionary<SlimeSizeID, SlimeChildSpawnData> _slimeSizeToNextSize;
        private readonly IFMODAudioManager _audioManager;

        [System.Serializable]
        public struct SlimeChildSpawnData
        {
            public SlimeSizeID slimeSizeId;
            public int childsToSpawn;
        }

        public SlimeFactory(Dictionary<SlimeSizeID, ObjectPool> slimeSizeToPool, 
            Dictionary<SlimeSizeID, SlimeChildSpawnData> slimeSizeToNextSize,
            IFMODAudioManager audioManager)
        {
            _slimeSizeToPool = slimeSizeToPool;
            _slimeSizeToNextSize = slimeSizeToNextSize;
            _audioManager = audioManager;
        }

        public SlimeMediator CreateNew(SlimeSizeID slimeSizeID, SlimeMindEnemy ownerMind,Vector3 position,Quaternion rotation)
        {
            SlimeMediator slimeMediator = _slimeSizeToPool[slimeSizeID].Spawn<SlimeMediator>(position, rotation);
            InitializeSlimeMediator(slimeMediator, ownerMind, slimeSizeID, ownerMind.GetPlayerTransform());

            return slimeMediator;
        }

        private void InitializeSlimeMediator(SlimeMediator slimeMediator, SlimeMindEnemy ownerMind,
            SlimeSizeID slimeSizeID, Transform playerTransform)
        {
            slimeMediator.InitAfterSpawn();
            slimeMediator.SetSlimeMind(ownerMind);
            slimeMediator.SetSlimeFactory(this);
            slimeMediator.SetAudioManager(_audioManager);
            slimeMediator.SetBoostDropFactory(ServiceLocator.Instance.GetService<IPowerBoostDropFactory>());
            slimeMediator.SetSlimeSize(slimeSizeID);
            slimeMediator.SetPlayerTransform(playerTransform);
            slimeMediator.SetEventSystem(ServiceLocator.Instance.GetService<IEventSystemService>());
        }
        public SlimeMediator[] CreateFromParent(SlimeMindEnemy ownerMind,SlimeMediator parentSlimeMediator,
            Vector3 position,Quaternion rotation)
        {
            SlimeChildSpawnData slimechildSpawnData = _slimeSizeToNextSize[parentSlimeMediator.SlimeSizeID];
            SlimeMediator[] slimes = new SlimeMediator[slimechildSpawnData.childsToSpawn];
            
            for (int i = 0; i < slimechildSpawnData.childsToSpawn; i++)
            {
                float angle = i * 360f / slimechildSpawnData.childsToSpawn;
                Vector3 spawnDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                SlimeMediator slimeMediator = _slimeSizeToPool[slimechildSpawnData.slimeSizeId]
                    .Spawn<SlimeMediator>(position, rotation);

                InitializeSlimeMediator(slimeMediator, ownerMind, 
                    slimechildSpawnData.slimeSizeId, parentSlimeMediator.PlayerTransform);
                
                slimeMediator.SpawningFromDivision(spawnDirection, parentSlimeMediator.GetPatrolType(),
                    parentSlimeMediator.GetPatrolWaypoints());

                ownerMind.AddSlimeToList(slimeMediator);
                slimes[i] = slimeMediator;
            }

            return slimes;
        }

        public bool CanSpawnNextSize(SlimeSizeID slimeSizeID)
        {
            return _slimeSizeToNextSize[slimeSizeID].childsToSpawn > 0;
        }
    }
}