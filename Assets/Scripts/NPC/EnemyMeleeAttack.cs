using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider _knifeCollider;
    [SerializeField] private NavMeshAgent _navmeshAgent;
    private float _speed;
    void Start()
    {
        _knifeCollider.enabled = false;
        _speed = _navmeshAgent.speed;
    }

    private void Attack()
    {
        _knifeCollider.enabled = true;
    }
    private void AttackEnd()
    {
        _knifeCollider.enabled = false;
    }
    private void StopMoving()
    {
        _navmeshAgent.speed = 0;
    }
    private void ContinueMoving()
    {
        _navmeshAgent.speed = _speed;
    }


}
