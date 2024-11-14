using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
    [SerializeField] private float _gameTime = 300;
    [SerializeField] private float _startHunger = 50;
    [SerializeField] private float _startPoint = 0;
    [SerializeField] private float _regenSpeed;
    [SerializeField] private float _hungerRegenThreshold;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _poinText;
    [SerializeField] Image _hungerFillBar;
    [SerializeField] Image _mouse;
    private float t;
    private float _elapsedTime;
    private float previousValue;
    private bool mouseLogoAppear = false;
    void Awake()
    {
        _elapsedTime = 0;
        t = 0;
        PlayerStats.GameTime = _gameTime;
        PlayerStats.Hunger = _startHunger;
        PlayerStats.Points = _startPoint;
    }

    void Update()
    {
        // Convert time minutes and seconds
        if (_gameTime - _elapsedTime > 0)
        {
            int minutes = (int)((_gameTime - _elapsedTime) / 60) % 60;
            int seconds = (int)((_gameTime - _elapsedTime) % 60);

            _timerText.text = "Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
            _elapsedTime += Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
        }
        // Hunger regen below certain hunger threshold
        if (PlayerStats.Hunger < _hungerRegenThreshold)
        {
            PlayerStats.Hunger += Time.deltaTime * _regenSpeed;
        }
        // Update point text and hunger fill bar
        _poinText.text = "Point: " + PlayerStats.Points.ToString();
        //
        _hungerFillBar.fillAmount = PlayerStats.Hunger/100;
        if (!mouseLogoAppear && PlayerStats.Hunger==0)
        {
            mouseLogoAppear = true;
            StartCoroutine(MakeLogoAppear());
        }
    }
    IEnumerator MakeLogoAppear()
    {
        _mouse.enabled = true;
        yield return new WaitForSeconds(5f);
        _mouse.enabled = false;
    }

}
