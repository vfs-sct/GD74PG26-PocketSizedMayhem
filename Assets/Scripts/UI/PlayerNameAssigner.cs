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
            _errorField.GetComponent<TextMeshProUGUI>().text = "Name cannot contain white space!";
            return;
        }
        else if (_nameInputField.GetComponent<TMP_InputField>().text.Length < 3 || _nameInputField.GetComponent<TMP_InputField>().text.Length > 20)
        {
            PlayerStats.SignedIn = false;
            _errorField.SetActive(true);
            _errorField.GetComponent<TextMeshProUGUI>().text = "Name must be between 3 and 20 characters!";
            return;
        }
        else if (!System.Text.RegularExpressions.Regex.IsMatch(_nameInputField.GetComponent<TMP_InputField>().text, "^[a-zA-Z0-9 _-]+$"))
        {
            PlayerStats.SignedIn = false;
            _errorField.SetActive(true);
            _errorField.GetComponent<TextMeshProUGUI>().text = "Name cannot contain invalid characters!";
            return;
        }
        else
        {
            PlayerStats.PlayerName = _nameInputField.GetComponent<TMP_InputField>().text;
            PlayerStats.SignedIn = true;
            UpdateName();
            _errorField.SetActive(false);
            SceneManager.LoadScene("GameScene");
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
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
