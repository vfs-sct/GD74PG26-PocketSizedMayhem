using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemySpawner;

public class SwitchBuilding : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private List<GameObject> _civilians;
    [SerializeField] private List<Transform> _topSpawnPoints;
    [SerializeField] private List<Transform> _leftSpawnPoints;
    [SerializeField] private List<Transform> _bottomSpawnPoints;
    [SerializeField] private List<Transform> _rightSpawnPoints;

    [Header("Civilian Type Weights")]
    [SerializeField] private float _easyCivilianWeight;
    [SerializeField] private float _mediumCivilianWeight;
    [SerializeField] private float _hardCivilianWeight;
    [SerializeField] private float _negativeCivilianWeight;

    [Header("Spawn Location Weights")]
    [SerializeField] private float _topWeight;
    [SerializeField] private float _leftWeight;
    [SerializeField] private float _bottomWeight;
    [SerializeField] private float _rightWeight;

    private int _spawnCount;
    private float _spawnWeightTotal;
    private float _spawnPointWeightTotal;

    [SerializeField] private GameObject _unShattered;
    [SerializeField] private GameObject _shattered;
    [SerializeField] private CivilianFill _civilianFillTracker;
    private void Start()
    {
        _spawnWeightTotal = (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight);
        _easyCivilianWeight = (_easyCivilianWeight / _spawnWeightTotal) * 100;
        _mediumCivilianWeight = (_mediumCivilianWeight / _spawnWeightTotal) * 100;
        _hardCivilianWeight = (_hardCivilianWeight / _spawnWeightTotal) * 100;
        _negativeCivilianWeight = (_negativeCivilianWeight / _spawnWeightTotal) * 100;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(_unShattered.activeInHierarchy && other.tag == "Mallet")
        {
            _unShattered.SetActive(false);
            _shattered.SetActive(true);
            _spawnCount = _civilianFillTracker.GetCivilianCount();
            
            SpawnAtPoint();
        }
    }

    public void SpawnAtPoint()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            float selection = Random.Range(0, _spawnPointWeightTotal);

            if (selection >= 0 && selection < _topWeight && _topWeight != 0)
            {
                _topSpawnPoints.Remove(SpawnCivilians(_topSpawnPoints[Random.Range(0, _topSpawnPoints.Count)]));
                _topWeight = (_topWeight * (_topSpawnPoints.Count) / (_topSpawnPoints.Count + 1));
            }
            else if (selection >= _topWeight && selection < _leftWeight + _topWeight && _leftWeight != 0)
            {
                _leftSpawnPoints.Remove(SpawnCivilians(_leftSpawnPoints[Random.Range(0, _leftSpawnPoints.Count)]));
                _leftWeight = ((_leftWeight * _leftSpawnPoints.Count) / (_leftSpawnPoints.Count + 1));
            }
            else if (selection >= _leftWeight + _topWeight && selection < _leftWeight + _topWeight + _bottomWeight && _bottomWeight != 0)
            {
                _bottomSpawnPoints.Remove(SpawnCivilians(_bottomSpawnPoints[Random.Range(0, _bottomSpawnPoints.Count)]));
                _bottomWeight = ((_bottomWeight * _bottomSpawnPoints.Count) / (_bottomSpawnPoints.Count + 1));
            }
            else if (selection >= _leftWeight + _topWeight + _bottomWeight && selection < _spawnPointWeightTotal && _rightWeight != 0)
            {
                _rightSpawnPoints.Remove(SpawnCivilians(_rightSpawnPoints[Random.Range(0, _rightSpawnPoints.Count)]));
                _rightWeight = (_rightWeight * (_rightSpawnPoints.Count) / (_rightSpawnPoints.Count + 1));
            }

            _spawnPointWeightTotal = _topWeight + _leftWeight + _bottomWeight + _rightWeight;
        }
    }

    private Transform SpawnCivilians(Transform point)
    {
        float selection = Random.Range(0, 100);
        GameObject civilian;
        if (selection >= 0 && selection < _easyCivilianWeight )
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.EASY);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        else if (selection >= _easyCivilianWeight  && selection < (_easyCivilianWeight + _mediumCivilianWeight) )
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.MEDIUM);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        else if (selection >= (_easyCivilianWeight + _mediumCivilianWeight)  && selection < (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight) )
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.HARD);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        else
        {
            civilian = NPCObjectPool.instance.GetPooledObject(NPCType.NEGATIVE);
            civilian.SetActive(true);
            civilian.transform.position = point.position;
            civilian.transform.rotation = point.rotation;
        }
        civilian.GetComponent<NewNpcBehavior>().BuildingSpawn();
        civilian.GetComponent<NavMeshAgent>().enabled = false;
        civilian.GetComponent<CivilianDeath>().enabled = false;
        civilian.GetComponent<Rigidbody>().AddForce(civilian.transform.forward*500+Vector3.up*1000);
        return point;
    }
}
