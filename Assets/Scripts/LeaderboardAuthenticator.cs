using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms.Impl;
using Unity.Collections.LowLevel.Unsafe;
using System;
using Unity.Services.Leaderboards.Models;
public class LeaderboardAuthenticator : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "High_Score";

    [SerializeField] private string _playerName;
    [SerializeField] private double _playerScore;
    [SerializeField] private string[] _leaderboardScores;
    [SerializeField] private string[] _leaderboardScoresParsed;
    [SerializeField] private Dictionary<string, string> _leaderboardSeperated = new Dictionary<string, string>();
    async void Awake()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();
        await SignInAnonymously();
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + _playerName);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await AuthenticationService.Instance.UpdatePlayerNameAsync(_playerName);
        AddScore();
    }

    public async void AddScore()
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, _playerScore);
        GetScores();
    }

    public async void GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(LeaderboardId);
        string scoresText = JsonConvert.SerializeObject(scoresResponse);
        string[] separatingStrings = {"playerName\":\""};
        string[] separatingStrings2 = { "#", "score\":",".0},{\""};
        _leaderboardScores = scoresText.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
        Console.Clear();
        foreach (string word in _leaderboardScores)
        {  
            _leaderboardScoresParsed = word.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            if(_leaderboardScoresParsed.Length>=2)
            {
                _leaderboardSeperated.Add(_leaderboardScoresParsed[0], _leaderboardScoresParsed[1]);
                Debug.Log(_leaderboardSeperated);
            }
        }
    }
}
