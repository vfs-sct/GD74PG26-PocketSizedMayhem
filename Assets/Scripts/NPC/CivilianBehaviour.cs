using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _destination;

    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.destination = _destination.transform.position;
        }
    }

    public void SetDestionation(GameObject newDestination)
    {
        _destination = newDestination;
    }
}
