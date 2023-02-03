using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_GameViewMode
{
    averageScreen, singleScreen
}

public class CS_GameViewManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _averageScreenObjects, _singleScreenObjects;

    private E_GameViewMode _currentGameView;

    public E_GameViewMode CurrentGameView => _currentGameView;

    public void SwitchGameViewMode()
    {
        if (_currentGameView == E_GameViewMode.averageScreen)
        {
            _currentGameView = E_GameViewMode.singleScreen;
        }
        else
        {
            _currentGameView = E_GameViewMode.averageScreen;
        }

        ApplyGameViewMode();
    }

    public void ApplyGameViewMode()
    {
        if (_currentGameView == E_GameViewMode.averageScreen)
        {
            foreach (var item in _averageScreenObjects)
            {
                item.SetActive(true);
            }
            foreach (var item in _singleScreenObjects)
            {
                item.SetActive(false);
            }
        }
        else
        {
            foreach (var item in _averageScreenObjects)
            {
                item.SetActive(false);
            }
            foreach (var item in _singleScreenObjects)
            {
                item.SetActive(true);
            }
        }
    }
}