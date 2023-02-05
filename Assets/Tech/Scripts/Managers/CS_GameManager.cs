using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class CS_GameManager : MonoBehaviour
{
    [Header("---Music---")]
    [SerializeField] private AudioClip _musique;

    [Header("---Parameters---")]
    [SerializeField] private int _startingScore = 1;

    [SerializeField]
    private float _tempsMinimumAvantQuunLapinSePointe = 3.6f,
    _tempsMaximumAvantQuunLapinSePointe = 4.8f;

    [SerializeField]
    private float _tempsMinimumPendantLequelUnLapinResterDansUnTerrier = 7f,
    _tempsMaximumPendantLequelUnLapinResterDansUnTerrier = 11f;

    [Tooltip("Toutes les dix secondes, _tempsMinimumAvantQuunLapinSePointe et _tempsMaximumAvantQuunLapinSePointe" +
        "vont être multipliés par ça.")]
    [SerializeField] private float _facteurDEvolutionDesValeursDeTempsAvantQuunLapinSePointe = 0.85f;

    [Tooltip("Toutes les dix secondes, _tempsMinimumPendantLequelUnLapinResterDansUnTerrier et _tempsMaximumPendantLequelUnLapinResterDansUnTerrier" +
    "vont être multipliés par ça.")]
    [SerializeField] private float _facteurDEvolutionDesValeursDeTempsPendantLequelUnLapinResterDansUnTerrier = 0.9f;

    [Header("---References---")]
    [SerializeField] private TextMeshProUGUI _winText;

    [SerializeField] private CS_BunnySpot[] _allBunnySpots;

    [SerializeField] private AudioSource _audioSource;

    private CS_Player _patatePlayer, _carottePlayer;
    private CS_ScoreUI _scoreUI;
    private CS_CountDown _countdown;

    private int _patateScore, _carotteScore;
    public int PatateScore => _patateScore;
    public int CarotteScore => _carotteScore;

    private bool _isPlaying = true;
    public bool IsPlaying => _isPlaying;

    private void Awake()
    {
        _patatePlayer = FindObjectOfType<CS_PatatePlayer>();
        _carottePlayer = FindObjectOfType<CS_CarottePlayer>();
        _scoreUI = FindObjectOfType<CS_ScoreUI>();
        _countdown = FindObjectOfType<CS_CountDown>();
    }

    private void Start()
    {
        StopGame();

        StartCoroutine(StartGameCoroutine());

        for (int i = 0; i < _startingScore - 1; i++)
        {
            _patatePlayer.AddCharacter();
            _carottePlayer.AddCharacter();
        }

        if (_startingScore < 2)
        {
            _patatePlayer.SetColliderSize();
            _carottePlayer.SetColliderSize();
        }

        _patateScore = _startingScore;
        _carotteScore = _startingScore;
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(1);
        _countdown.SetNumber(0);
        yield return new WaitForSecondsRealtime(1);
        _countdown.SetNumber(1);
        yield return new WaitForSecondsRealtime(1);
        _countdown.SetNumber(2);
        yield return new WaitForSecondsRealtime(1);
        StartGame();
    }

    private void StopGame(bool isThereAWinner = false, bool isPotatoes = false)
    {
        _isPlaying = false;

        if (isThereAWinner)
        {
            Time.timeScale = 0;
            _winText.gameObject.SetActive(false);
            if (isPotatoes)
            {
                _winText.text = "Les Patates ont gagné !";
            }
            else
            {
                _winText.text = "Les Carottes ont gagné !";
            }
            _audioSource.Stop();
        }
    }

    private void StartGame()
    {
        Debug.Log("ca demarre");

        Time.timeScale = 1;
        _isPlaying = true;
        _winText.gameObject.SetActive(false);
        _audioSource.PlayMusicWithFactor(_musique);

        StartCoroutine(BunnySpotsManagement());
        StartCoroutine(BunnyValuesEvolution());
    }

    public void SetScoreUI(int score, int player)
    {
        if (player == 0)
            _scoreUI.SetScorePatate(score);
        else
            _scoreUI.SetScoreCarotte(score);

        if (_patateScore == 0)
        {
            StopGame(true, true);
        }
        if (_carotteScore == 0)
        {
            StopGame(true, false);
        }
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
        if (player == 0)
        {
            _patateScore--;
            SetScoreUI(_patateScore, player);
        }
        else
        {
            _carotteScore--;
            SetScoreUI(_carotteScore, player);
        }
    }

    private IEnumerator BunnySpotsManagement()
    {
        while (_isPlaying)
        {
            float delay = Random.Range(_tempsMinimumAvantQuunLapinSePointe, _tempsMaximumAvantQuunLapinSePointe);
            yield return new WaitForSeconds(delay);
            int index = GetRandomBunnySpot();
            EnableBunnySpot(index);
            delay = Random.Range(_tempsMinimumPendantLequelUnLapinResterDansUnTerrier, _tempsMaximumPendantLequelUnLapinResterDansUnTerrier);
            yield return new WaitForSeconds(delay);
            RemoveBunnyFromSpot(index);
        }
    }

    private IEnumerator BunnyValuesEvolution()
    {
        while (_isPlaying)
        {
            yield return new WaitForSeconds(10);
            _tempsMinimumAvantQuunLapinSePointe *= _facteurDEvolutionDesValeursDeTempsAvantQuunLapinSePointe;
            _tempsMaximumAvantQuunLapinSePointe *= _facteurDEvolutionDesValeursDeTempsAvantQuunLapinSePointe;
            _tempsMinimumPendantLequelUnLapinResterDansUnTerrier *= _facteurDEvolutionDesValeursDeTempsPendantLequelUnLapinResterDansUnTerrier;
            _tempsMaximumPendantLequelUnLapinResterDansUnTerrier *= _facteurDEvolutionDesValeursDeTempsPendantLequelUnLapinResterDansUnTerrier;
        }
    }

    private int GetRandomBunnySpot()
    {
        return Random.Range(0, _allBunnySpots.Length);
    }

    private void EnableBunnySpot(int index)
    {
        _allBunnySpots[index].SetBunnyHere();

        int i = 0;

        foreach (var item in _allBunnySpots)
        {
            if (i != index)
            {
                item.RemoveBunnyFromHere();
            }

            i++;
        }
    }

    private void RemoveBunnyFromSpot(int index)
    {
        _allBunnySpots[index].RemoveBunnyFromHere();
    }
}