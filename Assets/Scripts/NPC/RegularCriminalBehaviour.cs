using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RegularCriminalBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _shelter;
    [SerializeField] private GameObject _targetCivilian;

    [SerializeField] private List<GameObject> _civilianList;
    

    private NavMeshAgent _navMeshAgent;
    private GameObject _primaryTarget;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _primaryTarget = _shelter;
    }

    private void Update()
    {
        _navMeshAgent.destination = _primaryTarget.transform.position;
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
