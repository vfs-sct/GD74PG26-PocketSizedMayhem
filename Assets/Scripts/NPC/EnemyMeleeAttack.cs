using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _capsuleCollider;

    void Start()
    {
        _capsuleCollider.enabled = false;
    }

    private void Attack()
    {
        _capsuleCollider.enabled = true;
    }
    private void AttackEnd()
    {
        _capsuleCollider.enabled = false;
    }

}
