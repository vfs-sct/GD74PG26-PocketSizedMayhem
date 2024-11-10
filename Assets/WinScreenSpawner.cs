using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _easyCivilianRagdoll;
    [SerializeField] private GameObject _mediumCivilianRagdoll;
    [SerializeField] private GameObject _hardCivilianRagdoll;
    [SerializeField] private GameObject _negativeCivilianRagdoll;
    [SerializeField] private Transform _spawnLocation;
    void Start()
    {
        StartCoroutine(SpawmRagdolls());
    }

    IEnumerator SpawmRagdolls()
    {
        for (int i = 0; i < PlayerStats.EasyCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_easyCivilianRagdoll, _spawnLocation.position, _easyCivilianRagdoll.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerStats.MediumCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_mediumCivilianRagdoll, _spawnLocation.position, _mediumCivilianRagdoll.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerStats.HardCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_hardCivilianRagdoll, _spawnLocation.position, _hardCivilianRagdoll.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerStats.NegativeCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_negativeCivilianRagdoll, _spawnLocation.position, _negativeCivilianRagdoll.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
