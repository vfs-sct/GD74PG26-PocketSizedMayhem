using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected int _spawnCount;
    [SerializeField] protected int _minSpawnInterval;
    [SerializeField] protected int _maxSpawnInterval;

    [SerializeField] protected GameObject _spawnObject;

    protected float targetTime;
    // Start is called before the first frame update
    void Start()
    {
        targetTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0)
        {
            SpawnObject();
            targetTime = Random.Range(_minSpawnInterval, _maxSpawnInterval);
        }
    }

    public virtual void SpawnObject()
    {
        Instantiate(_spawnObject, this.transform.position, Quaternion.Euler(0, 0, 0));
    }
}
