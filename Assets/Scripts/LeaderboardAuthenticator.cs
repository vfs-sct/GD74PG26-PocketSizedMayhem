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
    [SerializeField] private List<string> _leaderboardScoresParsed;
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
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        string scoresText = JsonConvert.SerializeObject(scoresResponse);
        string[] separatingStrings = {"playerName\":\"", "score\":", "#","},{" };
        string[] separatingStrings2 = { "#", "score\":",".0},{\""};
        _leaderboardScores = scoresText.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
        Console.Clear();
        if (_leaderboardScores.Length > 1 )
        {
            for (int i = 1; i < _leaderboardScores.Length+1; i += 4)
            {
                _leaderboardScoresParsed.Add(_leaderboardScores[i]+": " +_leaderboardScores[i+2]);
            }
            _leaderboardScoresParsed[_leaderboardScoresParsed.Count - 1] = _leaderboardScoresParsed[_leaderboardScoresParsed.Count - 1].Substring(0, _leaderboardScoresParsed[_leaderboardScoresParsed.Count - 1].Length-3);
        }
    }
}
