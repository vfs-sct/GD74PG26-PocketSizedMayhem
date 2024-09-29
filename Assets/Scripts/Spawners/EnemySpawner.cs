using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class EnemySpawner : Spawner
{
    [SerializeField] private GameObject _shelter;

    private EnemyManager _enemyManager;
    private NavMeshAgent _navAgent;
    public List<float> spawncount;
    public List<float> times;
    private float timer = 0;
    public int iteration = 0;
    private void Awake()
    {
        times = new List<float>();
    }
    private void Start()
    {
        _enemyManager = FindFirstObjectByType<EnemyManager>();
    }
    private void Update()
    {
        if (iteration<times.Count)
        {
            if ((int)timer == times[iteration])
            {
                StartCoroutine(SpawnWave());
            }
        }
        timer += Time.deltaTime;
    }
    public override void SpawnObject()
    {
        
            base.SpawnObject();
            _spawnedObject.GetComponent<RegularCriminalBehaviour>().SetTarget(_shelter);
            _enemyManager.AddToEnemyList(_spawnedObject);   
    }
    IEnumerator SpawnWave()
    {

        for (int i = 0; i < spawncount[iteration]; i++)
        {
            SpawnObject();
        }
        iteration++;
        yield return null;
    }
}
