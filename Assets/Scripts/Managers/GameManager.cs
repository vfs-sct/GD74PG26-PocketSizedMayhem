using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIComponents;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _gameTime = 300;
    [SerializeField] private float _startHunger = 50;
    [SerializeField] private float _startPoint = 0;
    [SerializeField] private float _regenSpeed;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _poinText;
    [SerializeField] Image _hungerFillBar;

    private float _elapsedTime;
    void Awake()
    {
        _elapsedTime = 0;

        PlayerStats.GameTime = _gameTime;
        PlayerStats.Hunger = _startHunger;
        PlayerStats.Points = _startPoint;
    }

    void Update()
    {
        if (_gameTime - _elapsedTime > 0)
        {
            int minutes = (int)((_gameTime - _elapsedTime) / 60) % 60;
            int seconds = (int)((_gameTime - _elapsedTime) % 60);
            _timerText.text = "Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            _timerText.text = "Time: " + 0;
            SceneManager.LoadScene("WinScreen");
        }
        if(PlayerStats.Hunger<20)
        {
            PlayerStats.Hunger += Time.deltaTime * _regenSpeed;
        }
        _poinText.text = "Point: " + PlayerStats.Points.ToString();
        _hungerFillBar.fillAmount = PlayerStats.Hunger / 100;
    }

    public void OnIncreaseTime()
    {
        _elapsedTime += 10;
    }

    public void OnDecreaseTime()
    {
        _elapsedTime -= 10;
    }

    public void OnIncreasePoint()
    {
        PlayerStats.Points += 10;
    }

    public void OnDecreasePoint()
    {
        PlayerStats.Points -= 10;
    }

    public void OnRestartScene()
    {
        SceneManager.LoadScene("GameScene - M3");
    }
}
