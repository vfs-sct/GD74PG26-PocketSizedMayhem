using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIComponents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _pointText;
    [SerializeField] private float _gameTime = 100;
    [SerializeField] private float _increaseAmount = 10;
    [SerializeField] private float _decreaseAmount = 10;

    private float _startTime;
    private float _elapsedTime;
    private float _point = 0;


    void Start()
    {
        _startTime = Time.time;
        _pointText.text = "Point:" + _point;
    }

    void Update()
    {
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
        PlayerStats.Points = _point;
        _pointText.text = "" + PlayerStats.Points;
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
        _point += _increaseAmount;
    }
    public void OnDecreasePoint()
    {
        _point -= _decreaseAmount;
    }
    public void OnLoadWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }
    public void OnLoadLoseScreen()
    {
        SceneManager.LoadScene("LoseScreen");
    }
    public void OnRestartScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
