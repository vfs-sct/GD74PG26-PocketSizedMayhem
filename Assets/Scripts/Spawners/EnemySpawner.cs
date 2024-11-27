using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NewNpcBehavior;

public class EnemySpawner : Spawner
{
    [Header("Civilian Type Weights")]
    [SerializeField] private int _easyCivilianWeight = 1;
    [SerializeField] private int _mediumCivilianWeight = 1;
    [SerializeField] private int _hardCivilianWeight = 1;
    [SerializeField] private int _negativeCivilianWeight = 1;
    [SerializeField] private NPCObjectPool _npcPool;

    public List<float> spawncount;
    public List<float> times;

    [SerializeField]private int _iteration;
    [SerializeField]private int _iterationCount;
    private int _spawnWeightTotal;

    private void Awake()
    {
        //times = new List<float>();
    }

    private void Start()
    {
        _iteration = 0;
        _spawnWeightTotal = _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight;
        _iterationCount = times.Count;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(times[_iteration] - Time.timeSinceLevelLoad);
        for (int i = 0; i < spawncount[_iteration]; i++)
        {
            Debug.Log(this.gameObject.name + ": "+ i);
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
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.EASY);
        }
        else if (selection >= _easyCivilianWeight && selection < _easyCivilianWeight + _mediumCivilianWeight)
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.NORMAL);
        }
        else if (selection >= _easyCivilianWeight + _mediumCivilianWeight && selection < _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight )
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.HARD);
        }
        else
        {
            civilian = NPCObjectPool.instance.GetPooledObject(TypeDifficulty.HARD);
        }
        if (civilian != null)
        {
            civilian.transform.position = position + offset;
            civilian.SetActive(true);
            _npcPool.AddToCivilianList(civilian);
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
