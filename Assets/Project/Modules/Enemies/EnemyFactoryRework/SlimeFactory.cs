using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Modules.Enemies.Slime;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class SlimeFactory
    {
        private Dictionary<SlimeSizeID, ObjectPool> _slimeSizeToPool;
        private Dictionary<SlimeSizeID, SlimeChildSpawnData> _slimeSizeToNextSize;
        
        [System.Serializable]
        public struct SlimeChildSpawnData
        {
            public SlimeSizeID slimeSizeId;
            public int childsToSpawn;
        }

        public SlimeFactory(Dictionary<SlimeSizeID, ObjectPool> slimeSizeToPool, 
            Dictionary<SlimeSizeID, SlimeChildSpawnData> slimeSizeToNextSize)
        {
            _slimeSizeToPool = slimeSizeToPool;
            _slimeSizeToNextSize = slimeSizeToNextSize;
        }

        public SlimeMediator CreateNew(SlimeSizeID slimeSizeID, SlimeMindEnemy ownerMind,Vector3 position,Quaternion rotation)
        {
            SlimeMediator slimeMediator = _slimeSizeToPool[slimeSizeID].Spawn<SlimeMediator>(position, rotation);
            InitializeSlimeMediator(slimeMediator, ownerMind.GetParticlePool(), ownerMind, slimeSizeID, ownerMind.GetPlayerTransform());

            return slimeMediator;
        }

        private void InitializeSlimeMediator(SlimeMediator slimeMediator,ObjectPool particlesPool,SlimeMindEnemy ownerMind,SlimeSizeID slimeSizeID,Transform playerTransform)
        {
            slimeMediator.SetObjectPool(particlesPool);
            slimeMediator.InitAfterSpawn();
            slimeMediator.SetSlimeMind(ownerMind);
            slimeMediator.SetSlimeFactory(this);
            slimeMediator.SetSlimeSize(slimeSizeID);
            slimeMediator.SetPlayerTransform(playerTransform);
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

                InitializeSlimeMediator(slimeMediator,parentSlimeMediator.GetObjectPool(),ownerMind,slimechildSpawnData.slimeSizeId,parentSlimeMediator.PlayerTransform);
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