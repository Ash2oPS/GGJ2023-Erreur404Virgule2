using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CS_GameManager : MonoBehaviour
{
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
    [SerializeField] private CS_BunnySpot[] _allBunnySpots;

    private CS_Player _patatePlayer, _carottePlayer;
    private CS_ScoreUI _scoreUI;

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
    }

    private void Start()
    {
        _patateScore = _startingScore;
        _carotteScore = _startingScore;

        SetScoreUI(_startingScore, 0);
        SetScoreUI(_startingScore, 1);

        _patatePlayer.SetColliderSize();
        _carottePlayer.SetColliderSize();

        StartCoroutine(BunnySpotsManagement());
        StartCoroutine(BunnyValuesEvolution());
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