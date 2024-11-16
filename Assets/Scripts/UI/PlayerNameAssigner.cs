using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNameAssigner : MonoBehaviour
{
    [SerializeField] private GameObject _nameInputField;
    [SerializeField] private GameObject _errorField;
    public void SetPlayerName()
    {
        if (string.IsNullOrWhiteSpace(_nameInputField.GetComponent<TMP_InputField>().text))
        {
            PlayerStats.SignedIn = false;
            _errorField.SetActive(true);
            return;
        }
        else
        {
            PlayerStats.PlayerName = _nameInputField.GetComponent<TMP_InputField>().text;
            PlayerStats.SignedIn = true;
            UpdateName();
            _errorField.SetActive(false);
            SceneManager.LoadScene("GameScene - M3");
        }
        
    }
    public bool NameChecker()
    {
        if(PlayerStats.PlayerName!= null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async void UpdateName()
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(PlayerStats.PlayerName);
    }

    public void SkipEnterName()
    {
        if (PlayerStats.SignedIn)
        {
            SceneManager.LoadScene("GameScene - M3");
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
