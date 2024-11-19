using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WinScreenSpawner : MonoBehaviour
{
    const string LeaderboardId = "High_Score";
    [field: SerializeField] public EventReference CivilianSpawnSFX { get; set; }

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
        Cursor.visible = true;
        AddScore();
        StartCoroutine(SpawmRagdolls());
    }

    IEnumerator SpawmRagdolls()
    {
        for (int i = 0; i < 600; i++)
        {
            GameObject obj = Instantiate(_easyCivilianRagdoll, _spawnLocation.position, _easyCivilianRagdoll.transform.rotation);
            _easyCivilianText.text = "Easy Civilian Killed: " + ++easyKilled;
            RuntimeManager.PlayOneShot(CivilianSpawnSFX, this.gameObject.transform.position);
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

    public async void AddScore()
    {
        try
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, PlayerStats.Points);

        }
        catch (LeaderboardsException exception)
        {
            Debug.LogError($"[Unity Leaderboards] {exception.Reason}: {exception.Message}");
        }

    }
}
