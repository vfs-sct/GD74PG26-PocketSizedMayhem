using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuilding : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private List<GameObject> _civilians;
    [SerializeField] private List<Transform> _topSpawnPoints;
    [SerializeField] private List<Transform> _leftSpawnPoints;
    [SerializeField] private List<Transform> _bottomSpawnPoints;
    [SerializeField] private List<Transform> _rightSpawnPoints;

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

    [SerializeField] private GameObject _unShattered;
    [SerializeField] private GameObject _shattered;
    [SerializeField] private CivilianFill _civilianFillTracker;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mallet")
        {
            _unShattered.SetActive(false);
            _shattered.SetActive(true);
            _spawnCount = _civilianFillTracker.GetCivilianCount();
            _spawnWeightTotal = (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight) * _spawnCount;
            SpawnAtPoint();
        }
    }

    public void SpawnAtPoint()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            int selection = Random.Range(0, _spawnPointWeightTotal);

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
        int selection = Random.Range(0, _spawnWeightTotal);
        GameObject civilian;

        if (selection >= 0 && selection < _easyCivilianWeight * _spawnCount && _easyCivilianWeight != 0)
        {
            civilian = Instantiate(_civilians[0], point.position, point.rotation);
            _easyCivilianWeight-=((_spawnWeightTotal) / _spawnCount);
        }
        else if (selection >= _easyCivilianWeight * _spawnCount && selection < (_easyCivilianWeight + _mediumCivilianWeight) * _spawnCount && _mediumCivilianWeight != 0)
        {
            civilian = Instantiate(_civilians[1], point.position, point.rotation);
            _mediumCivilianWeight -= ((_spawnWeightTotal) / _spawnCount);
        }
        else if (selection >= (_easyCivilianWeight + _mediumCivilianWeight) * _spawnCount && selection < (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight) * _spawnCount && _hardCivilianWeight != 0)
        {
            civilian = Instantiate(_civilians[2], point.position, point.rotation);
            _hardCivilianWeight -= ((_spawnWeightTotal) / _spawnCount);
        }
        else
        {
            civilian = Instantiate(_civilians[3], point.position, point.rotation);
            _negativeCivilianWeight -= ((_spawnWeightTotal) / _spawnCount);
        }
        civilian.GetComponent<NewNpcBehavior>().BuildingSpawn();
        _spawnWeightTotal = (_easyCivilianWeight + _mediumCivilianWeight + _hardCivilianWeight + _negativeCivilianWeight) * _spawnCount;
        civilian.GetComponent<Rigidbody>().AddForce(civilian.transform.forward*1000+Vector3.up*1000);
        return point;
    }
}
