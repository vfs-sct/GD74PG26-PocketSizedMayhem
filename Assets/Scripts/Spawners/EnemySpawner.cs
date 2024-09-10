using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : Spawner
{
    [SerializeField] private GameObject _shelter;

    private EnemyManager _enemyManager;
    private NavMeshAgent _navAgent;

    private void Start()
    {
        _enemyManager = FindObjectOfType<EnemyManager>();
    }
    public override void SpawnObject()
    {
        base.SpawnObject();
        Debug.Log(_enemyManager);
        _enemyManager.AddToEnemyList(_spawnedObject);
    }
}
