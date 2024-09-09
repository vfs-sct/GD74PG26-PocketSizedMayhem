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
    [SerializeField] protected List<GameObject> _civilianList;

    [SerializeField] protected float _detectionRadius;
   
    protected NavMeshAgent _navMeshAgent;
    
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _primaryTarget = _shelter;
    }

    private void Update()
    {
        _navMeshAgent.destination = _primaryTarget.transform.position;
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
        if (other.gameObject.layer.Equals("Civilian"))
        {
            _civilianList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals("Civilian") && other.gameObject != _targetCivilian)
        {
            _civilianList.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals("Civilian"))
        {
            _primaryTarget = _shelter;
        }
    }
}
