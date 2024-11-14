using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDebug : MonoBehaviour
{
    [Header("Debug Control Amounts")]
    [SerializeField] private float _timeChange;
    [SerializeField] private float _pointChange;


    // F3
    public void OnIncreasePoint()
    {
        PlayerStats.Points += _pointChange;
    }
    // F4
    public void OnDecreasePoint()
    {
        PlayerStats.Points -= _pointChange;
    }
    // F5
    public void OnRestartScene()
    {
        SceneManager.LoadScene("GameScene - M3");
    }
    //F6
    public void OnWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
