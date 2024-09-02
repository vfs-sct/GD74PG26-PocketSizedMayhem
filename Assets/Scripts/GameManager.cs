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
    [SerializeField] private float gameTime = 100;

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
        if (_elapsedTime > 0)
        {
            int minutes = (int)((gameTime - _elapsedTime) / 60) % 60;
            int seconds = (int)((gameTime - _elapsedTime) % 60);
            _timerText.text = "Remaining Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            SceneManager.LoadScene("LoseScreen");
        }
        _elapsedTime = Time.time - _startTime;
    }
}
