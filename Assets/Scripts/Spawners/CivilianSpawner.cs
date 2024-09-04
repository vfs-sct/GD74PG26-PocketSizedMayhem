using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianSpawner : Spawner
{
    [SerializeField] private GameObject _destination;

    private NavMeshAgent _navAgent;
    private void Start()
    {
        _navAgent = _spawnObject.GetComponent<NavMeshAgent>();
        _spawnObject.GetComponent<CivilianBehaviour>().SetDestionation(_destination);
        
    }
    public override void SpawnObject()
    {
        base.SpawnObject();
    }
    
}
