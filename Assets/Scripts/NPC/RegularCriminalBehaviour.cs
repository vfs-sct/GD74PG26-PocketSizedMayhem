using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using CharacterMovement;
using UnityEngine;
using UnityEngine.AI;

public class RegularCriminalBehaviour : CharacterMovement3D
{
    [field: SerializeField] public EventReference AttackSFX { get; set; }
    
    [SerializeField] protected GameObject _shelter;
    [SerializeField] protected GameObject _targetCivilian;
    [SerializeField] protected GameObject _primaryTarget;

    [SerializeField] protected float _detectionRadius;
   
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected Animator _enemyAnimator;
    
    private void Start()
    {
        _shelter = _primaryTarget;
        if (!AttackSFX.IsNull)
        {
            RuntimeManager.PlayOneShot(AttackSFX, this.gameObject.transform.position);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, _primaryTarget.transform.position) < 2f)
        {
            _enemyAnimator.SetTrigger("Attack");
            Stop();
            SetLookPosition(_primaryTarget.transform.position);
        }
        else
        {
            MoveTo(_primaryTarget.transform.position); 
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
        if(other.gameObject.layer.Equals(15))
        {
            GameManager.AddPoint();
            PlayerStats.CriminalCaptured++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(15))
        {
            GameManager.LosePoint();
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
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
