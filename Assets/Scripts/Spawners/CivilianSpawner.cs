using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianSpawner : Spawner
{
    [SerializeField] private GameObject _shelter;

    private CivilianManager _civilianManager;
    private NavMeshAgent _navAgent;

    private void Start()
    {
        _civilianManager = FindFirstObjectByType<CivilianManager>();
        _navAgent = _prefab.GetComponent<NavMeshAgent>();
        _prefab.GetComponent<CivilianBehaviour>().SetDestionation(_shelter);
    }
    public override void SpawnObject()
    {
        base.SpawnObject();
        _civilianManager.AddToCivilianList(_spawnedObject);
    }
    
}
