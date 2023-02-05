using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerPrefs : MonoBehaviour
{
    [SerializeField] private int _numberOfGames = 3;
    public int NumberOfGames => _numberOfGames;

    private int _currentNumberOfGames;
    public int CurrentNumberOfGames => _currentNumberOfGames;

    private int _currentGlobalPatateScore, _currentGlobalCarotteScore;
    private int CurrentGlobalPatateScore => _currentGlobalPatateScore;
    private int CurrentGlobalCarotteScore => _currentGlobalCarotteScore;

    private float _soundFactor = .4f, _musicFactor = .4f;
    public float SoundFactor => _soundFactor;
    public float MusicFactor => _musicFactor;

    private static CS_PlayerPrefs _instance;

    public static CS_PlayerPrefs Instance => _instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetSoundFactor(float value)
    {
        _soundFactor = value;
    }

    public void SetMusicFactor(float value)
    {
        _musicFactor = value;
    }

    public void SetGlobalPatateScore(int value)
    {
        _currentGlobalPatateScore = value;
    }

    public void SetGlobalCarotteScore(int value)
    {
        _currentGlobalCarotteScore = value;
    }

    public void SetCurrentNumberOfGames(int value)
    {
        _currentNumberOfGames = value;
    }
}