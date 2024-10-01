using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected int _spawnCount;
    [SerializeField] protected int _minSpawnInterval;
    [SerializeField] protected int _maxSpawnInterval;
    [SerializeField] protected float _radius = 1f;

    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected GameObject _spawnedObject;

    protected float targetTime;
    // Start is called before the first frame update
    void Start()
    {
        targetTime = Random.Range(_minSpawnInterval, _maxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0)
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                SpawnObject();
            }
            targetTime = Random.Range(_minSpawnInterval, _maxSpawnInterval);
        }
    }

    public virtual void SpawnObject()
    {
        Vector3 position = transform.position;
        Vector3 offset = Vector3.ClampMagnitude(new Vector3(Random.Range(-_radius, _radius), 0f, Random.Range(-_radius, _radius)), _radius);
        _spawnedObject = Instantiate(_prefab, position + offset, Quaternion.Euler(0, 0, 0));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
