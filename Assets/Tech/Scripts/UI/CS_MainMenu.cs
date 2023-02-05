using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CS_MainMenu : MonoBehaviour
{
    private bool _optionsDisplayed;
    [SerializeField] private GameObject _options;
    [SerializeField] private Button _engrenageButton, _playButton;

    public void StartGame()
    {
        SceneManager.LoadScene("S_Etienne");
    }

    public void ToggleOptions()
    {
        _optionsDisplayed = !_optionsDisplayed;

        if (_optionsDisplayed)
        {
            _engrenageButton.interactable = false;
            _playButton.interactable = false;
            _options.SetActive(true);
            return;
        }

        _engrenageButton.interactable = true;
        _playButton.interactable = true;
        _options.SetActive(false);
    }
}