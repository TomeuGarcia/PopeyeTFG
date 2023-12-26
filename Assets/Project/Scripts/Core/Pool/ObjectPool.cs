using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Popeye.Core.Pool
{
    public class ObjectPool
    {
        private readonly RecyclableObject _prefab;
        private readonly Transform _objectPoolParent;
        private readonly HashSet<RecyclableObject> _instantiateObjects;
        private Queue<RecyclableObject> _recycledObjects;

        public ObjectPool(RecyclableObject prefab, Transform objectPoolParent = null)
        {
            _prefab = prefab;
            _objectPoolParent = objectPoolParent;
            _instantiateObjects = new HashSet<RecyclableObject>();
        }

        public void Init(int numberOfInitialObjects)
        {
            _recycledObjects = new Queue<RecyclableObject>(numberOfInitialObjects);
            
            for (var i = 0; i < numberOfInitialObjects; i++)
            {
                var instance = InstantiateNewInstance(Vector3.zero, Quaternion.identity);
                instance.gameObject.SetActive(false);
                _recycledObjects.Enqueue(instance);
            }
        }

        private RecyclableObject InstantiateNewInstance(Vector3 position, Quaternion rotation)
        {
            var instance = Object.Instantiate(_prefab, position, rotation, _objectPoolParent);
            instance.Configure(this);
            return instance;
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation)
        {
            var recyclableObject = GetInstance(position, rotation);
            _instantiateObjects.Add(recyclableObject);
            recyclableObject.gameObject.SetActive(true);
            recyclableObject.Init();
            return recyclableObject.GetComponent<T>();
        }

        private RecyclableObject GetInstance(Vector3 position, Quaternion rotation)
        {
            if (_recycledObjects.Count > 0)
            {
                var recyclableObject = _recycledObjects.Dequeue();
                var transform = recyclableObject.transform;
                transform.position = position;
                transform.rotation = rotation;
                return recyclableObject;
            }
            
            Debug.LogWarning($"Not enough recycled objets for {_prefab.name} consider increase the initial number of objets");
            var instance = InstantiateNewInstance(position, rotation);
            return instance;
        }

        public void RecycleGameObject(RecyclableObject gameObjectToRecycle)
        {
            var wasInstantiated = _instantiateObjects.Remove(gameObjectToRecycle);
            Assert.IsTrue(wasInstantiated, $"{gameObjectToRecycle.name} was not instantiate on {_prefab.name} pool");
            
            gameObjectToRecycle.gameObject.SetActive(false);
            gameObjectToRecycle.Release();
            _recycledObjects.Enqueue(gameObjectToRecycle);
        }
    }
}