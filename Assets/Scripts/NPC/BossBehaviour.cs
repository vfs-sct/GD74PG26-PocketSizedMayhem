using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _hitForce;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _target = _enemyManager.ClosestCivilian(this.gameObject);
    }

    private void Update()
    {
        _navMeshAgent.destination = _target.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<RagdollOnOffController>(out RagdollOnOffController ragdollOnOffController ))
        {
            Vector3 hitAngle = (collision.transform.position - this.gameObject.transform.position).normalized;
            ragdollOnOffController.RagdollModeOn();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(hitAngle * _hitForce);
        }
    }
}
