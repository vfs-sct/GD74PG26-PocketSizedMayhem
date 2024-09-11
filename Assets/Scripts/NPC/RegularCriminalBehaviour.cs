using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.AI;

public class RegularCriminalBehaviour : MonoBehaviour
{
    [SerializeField] protected GameObject _shelter;
    [SerializeField] protected GameObject _targetCivilian;
    [SerializeField] protected GameObject _primaryTarget;

    [SerializeField] protected float _detectionRadius;
   
    protected NavMeshAgent _navMeshAgent;
    protected Animator _enemyAnimator;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAnimator = GetComponent<Animator>();
        _shelter = _primaryTarget;
    }

    private void Update()
    {
        if (_primaryTarget != null)
        {
            _navMeshAgent.destination = _primaryTarget.transform.position;
        }
    }

    public void SetTarget(GameObject target)
    {
        if(target != null)
        {
            _primaryTarget = target;
        }
    }

    public virtual bool HasTarget()
    {
        if(_primaryTarget != _shelter)
        {
            return true;
        }
        return false;
    }

    public float GetDetectionRadius()
    {
        return _detectionRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(6))
        {
            _enemyAnimator.SetTrigger("Attack");
        }
        if (other.gameObject == _primaryTarget)
        {
            _primaryTarget = _shelter;
            
        }
    }
}
