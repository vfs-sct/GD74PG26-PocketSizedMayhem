using CharacterMovement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _shelter;
    [SerializeField] private GameObject _shelterChargePoint;
    [SerializeField] private GameObject _stunBar;

    [SerializeField] private float _hitForce;
    [SerializeField] private int _chargeCycle;

    private IEnumerator _currentState;
    private int _chargeAnimCycle;
    private void Start()
    {
        ChangeState(GoTowardsShelterState());
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator GoTowardsShelterState()
    {
        _navMeshAgent.SetDestination(_shelterChargePoint.transform.position);
        float distance = Vector3.Distance(transform.position, _shelterChargePoint.transform.position);
        while (distance > _navMeshAgent.stoppingDistance)
        {
            distance = Vector3.Distance(transform.position, _shelterChargePoint.transform.position);
            yield return null;
        }
        ChangeState(ChargeState());
    }

    private IEnumerator ChargeState()
    {
        this.gameObject.transform.LookAt(_shelter.transform.position);
        _chargeAnimCycle = 0;
        _animator.SetTrigger("Charge");
        while (_chargeAnimCycle < _chargeCycle)
        {
            yield return null;
        }
        ChangeState(ChargeRunState());
    }

    
    private IEnumerator ChargeRunState()
    {
        _navMeshAgent.SetDestination(_shelter.transform.position);
        _animator.SetTrigger("Attack");
        yield return null;
    }

    private IEnumerator FallFromHitState()
    {
        CameraShake.Instance.Shake(10f, 1f);
        _animator.SetTrigger("Fall");
        _navMeshAgent.enabled = false;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return null;
    }
    private IEnumerator StandUpState()
    {
        _animator.SetTrigger("Stand");
        yield return null;
    }
    private IEnumerator WalkBackState()
    {
        _animator.SetTrigger("WalkBack");
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_shelterChargePoint.transform.position);
        float distance = Vector3.Distance(transform.position, _shelterChargePoint.transform.position);
        while (distance > _navMeshAgent.stoppingDistance)
        {
            distance = Vector3.Distance(transform.position, _shelterChargePoint.transform.position);
            yield return null;
        }
        ChangeState(ChargeState());
    }
    
    private void IncraseChargeCycle()
    {
        _chargeAnimCycle++;
    }
    private void WalkBackTrigger()
    {
        ChangeState(WalkBackState());
    }
    private void StandTrigger()
    {
        ChangeState(StandUpState());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            if(_stunBar.GetComponent<BossStunBar>().GetStunDuration()<=0)
            {
                _animator.SetTrigger("Stun");
            }
            _stunBar.GetComponent<BossStunBar>().IncreaseStun();
        }
        else if (other.gameObject.layer.Equals(17))
        {
            this.GetComponent<Animator>().enabled = false;
        }
        else if (other.gameObject.layer.Equals(16))
        {
            ChangeState(FallFromHitState());
        }
    }
}


