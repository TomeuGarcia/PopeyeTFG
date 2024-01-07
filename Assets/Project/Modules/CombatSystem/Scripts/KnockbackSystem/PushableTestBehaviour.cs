using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class PushableTestBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
        public Vector3 Position => transform.position;
    }
}