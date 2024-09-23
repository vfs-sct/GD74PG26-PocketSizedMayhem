using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianBehaviour : MonoBehaviour
{
    [field: SerializeField] public EventReference SpawnSFX { get; set; }
    
    [SerializeField] private GameObject _destination;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    private bool _inShelter;
    
    private void Start()
    {
        _inShelter = false;
        if (!SpawnSFX.IsNull)
        {
            RuntimeManager.PlayOneShot(SpawnSFX, this.gameObject.transform.position);
        }
    }

    private void Update()
    {
        if (_navMeshAgent != null && _destination != null && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.destination = _destination.transform.position;
        }
        if (_inShelter && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = true;
        }
        else if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = false;
        }
    }

    public void SetDestionation(GameObject newDestination)
    {
        _destination = newDestination;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(11))
        {
            _navMeshAgent.enabled = true;
        }
    }
}
