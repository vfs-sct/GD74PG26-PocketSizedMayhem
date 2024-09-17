using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _hitForce;
    [SerializeField] private GameObject _stunBar;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_target != null)
        {
            _navMeshAgent.destination = _target.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<RagdollOnOffController>(out RagdollOnOffController ragdollOnOffController))
        {
            Vector3 hitAngle = (other.transform.position - this.gameObject.transform.position).normalized;
            ragdollOnOffController.RagdollModeOn();
            ragdollOnOffController.DeathBounce();
            other.gameObject.GetComponent<Rigidbody>().AddForce(hitAngle * _hitForce);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Mallet")
        {
            Debug.Log("hehe");
            _stunBar.GetComponent<BossStunBar>()._stunDuration += 3;
        }
        else if (collision.gameObject.layer.Equals(17))
        {
            this.GetComponent<Animator>().enabled = false;
        }
    }
}
