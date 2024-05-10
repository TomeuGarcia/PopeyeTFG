using System.Collections;
using System.Collections.Generic;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private BossShooting _bossShooting;
    [SerializeField] private BossShooting _bossShooting2;
    [Header("ACCEPT TYPES")] 
    [SerializeField] private ObjectTypeAsset _playerType;
    private void OnTriggerEnter(Collider other)
    {
        if (AcceptsOtherCollider(other))
        {
            _bossShooting.StartShooting();
            //_bossShooting2.StartShooting();
        }
    }

    private bool AcceptsOtherCollider(Collider other)
    {
        if (!other.TryGetComponent(out IObjectType otherObjectType)) return false;
        return otherObjectType.IsOfType(_playerType);
    }
    
}
