using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _easyCivilianRagdoll;
    [SerializeField] private GameObject _mediumCivilianRagdoll;
    [SerializeField] private GameObject _hardCivilianRagdoll;
    [SerializeField] private GameObject _negativeCivilianRagdoll;
    [SerializeField] private Transform _spawnLocation;

    [SerializeField] private TextMeshProUGUI _easyCivilianText;
    [SerializeField] private TextMeshProUGUI _mediumCivilianText;
    [SerializeField] private TextMeshProUGUI _hardCivilianText;
    [SerializeField] private TextMeshProUGUI _negativeCivilianText;
    [SerializeField] private TextMeshProUGUI _totalPoint;

    private int easyKilled = 0;
    private int mediumKilled = 0;
    private int hardKilled = 0;
    private int negativeKilled = 0;
    void Start()
    {
        StartCoroutine(SpawmRagdolls());
    }

    IEnumerator SpawmRagdolls()
    {
        for (int i = 0; i < PlayerStats.EasyCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_easyCivilianRagdoll, _spawnLocation.position, _easyCivilianRagdoll.transform.rotation);
            _easyCivilianText.text = "Easy Civilian Killed: " + ++easyKilled;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerStats.MediumCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_mediumCivilianRagdoll, _spawnLocation.position, _mediumCivilianRagdoll.transform.rotation);
            _mediumCivilianText.text = "Medium Civilian Killed: " + ++mediumKilled;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerStats.HardCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_hardCivilianRagdoll, _spawnLocation.position, _hardCivilianRagdoll.transform.rotation);
            _hardCivilianText.text = "Hard Civilian Killed: " + ++hardKilled;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerStats.NegativeCivilianKilled; i++)
        {
            GameObject obj = Instantiate(_negativeCivilianRagdoll, _spawnLocation.position, _negativeCivilianRagdoll.transform.rotation);
            _negativeCivilianText.text = "Negative Civilian Killed: " + ++negativeKilled;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        _totalPoint.text = "Total Points: " + PlayerStats.Points;
        yield return null;
    }
}
