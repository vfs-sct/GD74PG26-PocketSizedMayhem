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

    private bool _inPrison;

    private void Start()
    {
        _inPrison = false;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAnimator = GetComponent<Animator>();
        _shelter = _primaryTarget;
    }

    private void Update()
    {
        if (_primaryTarget != null && _navMeshAgent != null &&  _navMeshAgent.isOnNavMesh )
        {
            _navMeshAgent.destination = _primaryTarget.transform.position;
        }
        if(_inPrison && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = true;
        }
        else if(_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = false;
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
        else if(other.gameObject.layer.Equals(15))
        {
            GameManager.AddPoint();
            _inPrison = true;
            PlayerStats.CriminalCaptured++;
        }
        else if (other.gameObject.layer.Equals(11))
        {
            _navMeshAgent.enabled = true;
        }
        if (other.gameObject == _primaryTarget)
        {
            _primaryTarget = _shelter;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(15))
        {
            GameManager.LosePoint();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(16))
        {
            _enemyAnimator.SetTrigger("Attack");
            _navMeshAgent.isStopped = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer.Equals(16))
        {
            _enemyAnimator.SetTrigger("Attack");
            _navMeshAgent.isStopped = false;
        }
    }
}
