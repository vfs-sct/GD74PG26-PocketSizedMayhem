using CharacterMovement;
using PrimeTween;
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
    [SerializeField] private float workoutTime;
    [SerializeField] private int _chargeCycle;
    [SerializeField] private int _chargeAnimCycle;
    [SerializeField] private GameObject _prison;
    private IEnumerator _currentState;
    private bool _canHit;

    private void Start()
    {
        _canHit = true;
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
        _canHit = true;
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
    private IEnumerator PrisonedState()
    {
        _animator.SetTrigger("Prisoned");
        yield return null;
    }
    private IEnumerator WorkoutState()
    {
        yield return new WaitForSeconds(workoutTime);
        ChangeState(JumpState());
    }
    private IEnumerator JumpState()
    {
        _chargeAnimCycle = 0;
        _animator.SetTrigger("Jump");
        while (_chargeAnimCycle < 5)
        {
            yield return null;
        }
        _animator.SetTrigger("WalkShelter");
        ChangeState(GoTowardsShelterState());
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
    private void ShakePrison()
    {
        Tween.PunchLocalPosition(_prison.transform, strength: Vector3.up * _hitForce, duration: 2f, frequency: 1);
        Tween.ShakeLocalRotation(_prison.transform, strength:  new Vector3(15, 15, 15) , duration: 2, frequency: 1);
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
            if(_canHit)
            {
                ChangeState(FallFromHitState());
                _canHit = false;
            }
        }
        else if(other.gameObject.layer.Equals(15))
        {
            _navMeshAgent.enabled = false;
            ChangeState(PrisonedState());
        }
    }
}


