using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _civilianRagdoll;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private float _objectCount;
    void Start()
    {
        StartCoroutine(SpawmRagdolls());
    }

    IEnumerator SpawmRagdolls()
    {
        for (int i = 0; i < _objectCount; i++)
        {
            GameObject obj = Instantiate(_civilianRagdoll, _spawnLocation.position, _civilianRagdoll.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
