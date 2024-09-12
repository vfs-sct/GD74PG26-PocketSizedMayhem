using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIComponents;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    const string LeaderboardId = "High_Score";

    [SerializeField] private GameObject _civilian;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _pointText;
    [SerializeField] private TextMeshProUGUI _objectiveText1;
    [SerializeField] private TextMeshProUGUI _objectiveText2;
    [SerializeField] private TextMeshProUGUI _objectiveText3;

    [SerializeField] private float _gameTime = 100;
    [SerializeField] private static float _increaseAmount = 10;
    [SerializeField] private static float _decreaseAmount = 10;

    [SerializeField] private int _civilianToSaved = 2;
    [SerializeField] private int _criminalsKilled = 12;
    [SerializeField] private int _criminalsCaptured = 2;

    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit _hit;

    private float _startTime;
    private float _elapsedTime;
    public static float _point;
    public GameObject shelter;

    void Start()
    {
        _startTime = Time.time;
        _point = 100;
        _pointText.text = "Point:" + _point;

        PlayerStats.CivilianSaved = 0;
        PlayerStats.CriminalKilled = 0;
        PlayerStats.CriminalCaptured = 0;
        PlayerStats.CriminalCaptured = 0;
        PlayerStats.Points = 100;
    }

    void Update()
    {
        _point =  PlayerStats.Points;
        _elapsedTime = Time.time - _startTime;
        if (_gameTime - _elapsedTime > 0)
        {
            int minutes = (int)((_gameTime - _elapsedTime) / 60) % 60;
            int seconds = (int)((_gameTime - _elapsedTime) % 60);
            _timerText.text = "Remaining Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            _timerText.text = "Remaining Time: " + 0;
            SceneManager.LoadScene("LoseScreen");
        }
        // Lose condition
        if (PlayerStats.Points <= 0 || shelter.GetComponent<ShelterHealth>()._currentHealth<=0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
        if(PlayerStats.CriminalKilled >= _criminalsKilled && PlayerStats.CriminalCaptured >= _criminalsCaptured && PlayerStats.CivilianSaved >= _civilianToSaved)
        {
            SceneManager.LoadScene("WinScreen");
        }

        // Win condition
        
        _pointText.text = "Point:" + PlayerStats.Points;
        _objectiveText1.text = "Civilians Saved - " + PlayerStats.CivilianSaved + "/" + _civilianToSaved;
        _objectiveText2.text = "Criminals Killed - " + PlayerStats.CriminalKilled + "/" + _criminalsKilled;
        _objectiveText3.text = "Criminals Captured - " + PlayerStats.CriminalCaptured + "/" + _criminalsCaptured;
    }

    public void OnIncreaseTime()
    {
        _gameTime += _increaseAmount;
    }
    public void OnDecreaseTime()
    {
        _gameTime -= _decreaseAmount;
    }
    public void OnIncreasePoint()
    {
        PlayerStats.Points += _increaseAmount;
    }
    public void OnDecreasePoint()
    {
        PlayerStats.Points -= _decreaseAmount;
    }
    public void OnLoadWinScreen()
    {
        AddScore();
        SceneManager.LoadScene("WinScreen");
    }
    public void OnLoadLoseScreen()
    {
        AddScore();
        SceneManager.LoadScene("LoseScreen");
    }
    public void OnRestartScene()
    {
        SceneManager.LoadScene("GameScene");
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

    public void OnSpawnCivilian()
    {
        _mousePos = Input.mousePosition;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out _hit))
        {
            return;
        }

        Instantiate(_civilian,_hit.point,Quaternion.Euler(0,0,0));
    }
    public static void AddPoint()
    {
        PlayerStats.Points += _increaseAmount;
    }
    public static void LosePoint()
    {
        PlayerStats.Points -= _increaseAmount;
    }
}
