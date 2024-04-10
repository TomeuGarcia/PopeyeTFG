using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.IDSystem
{
    // Commented because we don't want generic IDs for now
    /*
    [CreateAssetMenu(fileName = "ID_NAME", 
        menuName = ScriptableObjectsHelper.ID_ASSETS_PATH + "ID Asset")]
        */
    public class IDAsset : ScriptableObject, ID
    {
        [Header("Uid")]
        [SerializeField] private string _id = Guid.NewGuid().ToString();
        public Guid Id { get; private set;}
        
        
        void OnValidate()
        {
            Id = Guid.Parse(_id);
        }
    
        void Awake()
        {
            OnValidate();
        }

        public void ResetId()
        {
            _id = Guid.NewGuid().ToString();
        }
    }
}