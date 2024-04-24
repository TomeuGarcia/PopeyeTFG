using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedAnimationController : MonoBehaviour
{
    
    [SerializeField] private Animator _animator;
    
    private const string MOVE_ANIMATOR_PARAMETER = "IsMoving";
    private const string STUN_ANIMATOR_PARAMETER = "IsStunned";
    private const string ATTACK_ANIMATOR_PARAMETER = "IsAttacking";
    
    
    public void PlayMove()
    {
        _animator.SetBool(STUN_ANIMATOR_PARAMETER, false);
        _animator.SetBool(ATTACK_ANIMATOR_PARAMETER, false);
        _animator.SetBool(MOVE_ANIMATOR_PARAMETER, true);
        
    }
    
    public void PlayAttack()
    {
        _animator.SetBool(MOVE_ANIMATOR_PARAMETER, false);
        _animator.SetBool(STUN_ANIMATOR_PARAMETER, false);
        _animator.SetBool(ATTACK_ANIMATOR_PARAMETER, true);
    }
    
    public void PlayStun()
    {
        _animator.SetBool(MOVE_ANIMATOR_PARAMETER, false);
        _animator.SetBool(ATTACK_ANIMATOR_PARAMETER, false);
        _animator.SetBool(STUN_ANIMATOR_PARAMETER, true);
    }
}
