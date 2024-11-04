using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [Header("Civilian Type Weights")]
    [SerializeField] private int _easyCivilianWeight = 1;
    [SerializeField] private int _mediumCivilianWeight = 1;
    [SerializeField] private int _hardCivilianWeight = 1;
    [SerializeField] private int _negativeCivilianWeight = 1;

    public List<float> spawncount;
    public List<float> times;

    private int _iteration = 0;
    private int _iterationCount;
    private int _spawnWeightTotal;

    private void Awake()
    {
        //times = new List<float>();
    }

    private void Start()
    {
        _spawnWeightTotal = _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight;
        _iterationCount = times.Count;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(1);
        _iterationCount = times.Count;
        yield return new WaitForSeconds(times[_iteration] - Time.time);
        for (int i = 0; i < spawncount[_iteration]; i++)
        {
           
            SpawnCivilian();
        }
        _iteration++;
        if(_iteration < _iterationCount)
        {
            StartCoroutine(SpawnWave());
        }
    }
    private void SpawnCivilian()
    {
        Vector3 position = transform.position;
        Vector3 offset = Vector3.ClampMagnitude(new Vector3(Random.Range(-_radius, _radius), 0f, Random.Range(-_radius, _radius)), _radius);
        int selection = Random.Range(0, _spawnWeightTotal);
        GameObject civilian;

        if (selection >= 0 && selection < _easyCivilianWeight )
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.EASY);
        }
        else if (selection >= _easyCivilianWeight && selection < _easyCivilianWeight + _mediumCivilianWeight)
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.MEDIUM);
        }
        else if (selection >= _easyCivilianWeight + _mediumCivilianWeight && selection < _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight )
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.HARD);
        }
        else
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.NEGATIVE);
        }
        if (civilian != null)
        {
            civilian.transform.position = position + offset;
            civilian.SetActive(true);
        }
    }

    public enum NPCType
    {
        EASY,
        MEDIUM,
        HARD,
        NEGATIVE
    }

}
