using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CS_GameManager : MonoBehaviour
{
    [Header("---Parameters---")]
    [SerializeField] private int _startingScore = 1;

    private CS_Player _patatePlayer, _carottePlayer;
    private CS_ScoreUI _scoreUI;

    private int _patateScore, _carotteScore;
    public int PatateScore => _patateScore;
    public int CarotteScore => _carotteScore;

    private void Awake()
    {
        _patatePlayer = FindObjectOfType<CS_PatatePlayer>();
        _carottePlayer = FindObjectOfType<CS_CarottePlayer>();
        _scoreUI = FindObjectOfType<CS_ScoreUI>();
    }

    private void Start()
    {
        _patateScore = _startingScore;
        _carotteScore = _startingScore;

        SetScoreUI(_startingScore, 0);
        SetScoreUI(_startingScore, 1);

        _patatePlayer.SetColliderSize();
        _carottePlayer.SetColliderSize();
    }

    public void SetScoreUI(int score, int player)
    {
        if (player == 0)
            _scoreUI.SetScorePatate(score);
        else
            _scoreUI.SetScoreCarotte(score);
    }

    public void AddCharacterUI(int player)
    {
        if (player == 0)
        {
            _scoreUI.PlayPatateUpAnimation();
            _patateScore++;
            SetScoreUI(_patateScore, player);
        }
        else
        {
            _scoreUI.PlayCarotteUpAnimation();
            _carotteScore++;
            SetScoreUI(_carotteScore, player);
        }
    }

    public void RemoveCharacterUI(int player)
    {
    }
}