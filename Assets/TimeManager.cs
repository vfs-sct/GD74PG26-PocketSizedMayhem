using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _gameTime = 100;

    private float _startTime;
    private float _elapsedTime;

    void Start()
    {
        _startTime = Time.time;
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
    }
}
