using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private List<GameObject> _civilians;
    [SerializeField] private List<Transform> _topSpawnPoints;
    [SerializeField] private List<Transform> _leftSpawnPoints;
    [SerializeField] private List<Transform> _bottomSpawnPoints;
    [SerializeField] private List<Transform> _rightSpawnPoints;

    [Header("Min-Max")]
    [SerializeField] private int _minSpawn;
    [SerializeField] private int _maxSpawn;

    [Header("Civilian Type Weights")]
    [SerializeField] private int _easyCivilianWeight;
    [SerializeField] private int _mediumCivilianWeight;
    [SerializeField] private int _hardCivilianWeight;
    [SerializeField] private int _negativeCivilianWeight;

    [Header("Spawn Location Weights")]
    [SerializeField] private int _topWeight;
    [SerializeField] private int _leftWeight;
    [SerializeField] private int _bottomWeight;
    [SerializeField] private int _rightWeight;

    private int _spawnCount;
    private int _spawnWeightTotal;
    private int _spawnPointWeightTotal;

    private int _easyCivilianCount;
    private int _mediumCivilianCount;
    private int _hardCivilianCount;
    private int _negativeCivilianCount;

    void Start()
    {
        _spawnCount = Random.Range(_minSpawn, _maxSpawn);
        _spawnWeightTotal = _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight;

        _topWeight *= _topSpawnPoints.Count;
        _leftWeight *= _leftSpawnPoints.Count;
        _bottomWeight *= _bottomSpawnPoints.Count;
        _rightWeight *= _rightSpawnPoints.Count;

        _spawnPointWeightTotal = _topWeight + _leftWeight + _bottomWeight + _rightWeight;
        SpawnAtPoint();
    }

    private void SpawnAtPoint()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            int selection = Random.Range(0, _spawnPointWeightTotal);

            if (selection > 0 && selection < _topWeight)
            {
               _topSpawnPoints.Remove(SpawnCivilians(_topSpawnPoints[Random.Range(0, _topSpawnPoints.Count)]));
               _topWeight = (_topWeight * (_topSpawnPoints.Count) / (_topSpawnPoints.Count + 1));
            }
            else if (selection >= _topWeight && selection < _leftWeight + _topWeight) 
            {
                _leftSpawnPoints.Remove(SpawnCivilians(_leftSpawnPoints[Random.Range(0, _leftSpawnPoints.Count)]));
                _leftWeight = ((_leftWeight * _leftSpawnPoints.Count) / (_leftSpawnPoints.Count + 1));
            }
            else if (selection >= _leftWeight + _topWeight && selection < _leftWeight + _topWeight + _bottomWeight)
            {
                _bottomSpawnPoints.Remove(SpawnCivilians(_bottomSpawnPoints[Random.Range(0, _bottomSpawnPoints.Count)]));
                _bottomWeight = ((_bottomWeight  * _bottomSpawnPoints.Count ) / (_bottomSpawnPoints.Count + 1));
            }
            else if (selection >= _leftWeight + _topWeight + _bottomWeight && selection < _spawnPointWeightTotal)
            {
                _rightSpawnPoints.Remove(SpawnCivilians(_rightSpawnPoints[Random.Range(0, _rightSpawnPoints.Count)]));
                _rightWeight = (_rightWeight * (_rightSpawnPoints.Count) / (_rightSpawnPoints.Count + 1));
            }

            _spawnPointWeightTotal = _topWeight + _leftWeight + _bottomWeight + _rightWeight;
        }
    }

    private Transform SpawnCivilians(Transform point)
    {
        int selection = Random.Range(0, _spawnWeightTotal);

        if (selection >= 0 && selection < _easyCivilianWeight && _easyCivilianWeight!=0)
        {
            Instantiate(_civilians[0], point.position, _civilians[0].transform.rotation);
            _easyCivilianWeight--;
        }
        else if (selection >= _easyCivilianWeight && selection < _easyCivilianWeight + _mediumCivilianWeight && _mediumCivilianWeight != 0)
        {
            Instantiate(_civilians[1], point.position, _civilians[1].transform.rotation);
            _mediumCivilianWeight--;
        }
        else if (selection >= _easyCivilianWeight + _mediumCivilianWeight && selection < _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight && _hardCivilianWeight != 0)
        {
            Instantiate(_civilians[2], point.position, _civilians[2].transform.rotation);
            _hardCivilianWeight--;
        }
        else if (selection >= _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight && selection < _spawnWeightTotal && _negativeCivilianWeight != 0)
        {
            Instantiate(_civilians[3], point.position, _civilians[3].transform.rotation);
            _negativeCivilianWeight--;
        }

        _spawnWeightTotal = _easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight;

        return point;
    }
}
