using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NewNpcBehavior;

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
    private float _elapsedTime;
    private float previousValue;
    private bool mouseLogoAppear = false;
    private bool shake = false;
    private bool scale = false;
    [SerializeField] NPCObjectPool _npcObjectPool;
    void Awake()
    {
        Cursor.visible = false;
        _elapsedTime = 0;
        PlayerStats.GameTime = _gameTime;
        PlayerStats.Hunger = _startHunger;
        PlayerStats.Points = _startPoint;
        PlayerStats.EasyCivilianKilled = 0;
        PlayerStats.MediumCivilianKilled = 0;
        PlayerStats.HardCivilianKilled = 0;
        PlayerStats.NegativeCivilianKilled = 0;
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
        _poinText.text = "Score: " + PlayerStats.Points.ToString();
        //
        _hungerFillBar.fillAmount = PlayerStats.Hunger/ _startHunger;
        if (PlayerStats.Hunger==0)
        {
            _mouse.enabled = true;
            if(!shake)
            {
                Tween.ShakeLocalRotation(_mouse.gameObject.transform, strength: new Vector3(0, 0, 15), duration: 15, frequency: 5);
                
                shake = true;
                if(!scale)
                {
                    scale = true;
                    StartCoroutine(ScaleMouse());
                }
                
            }
        }
        else
        {
            shake = false;
            _mouse.enabled = false;
        }
        PlayerStats.Hunger = Mathf.Clamp(PlayerStats.Hunger,0,_startHunger);
    }
    IEnumerator ScaleMouse()
    {
        Tween.Scale(_mouse.gameObject.transform, 1, 2, duration: 1, ease: Ease.Default);
        yield return new WaitForSeconds(1f);
        Tween.Scale(_mouse.gameObject.transform, 2, 1, duration: 1, ease: Ease.Default);
        yield return new WaitForSeconds(1f);
        StartCoroutine(ScaleMouse());
    }
}
