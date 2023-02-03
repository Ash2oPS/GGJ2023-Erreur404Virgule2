using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CS_ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _patateScoreTMP, _carotteScoreTMP;
    [SerializeField] private Animation _patateAnimation, _carotteAnimation;

    public void SetScorePatate(int score)
    {
        _patateScoreTMP.text = score.ToString();
    }

    public void SetScoreCarotte(int score)
    {
        _carotteScoreTMP.text = score.ToString();
    }

    public void PlayPatateUpAnimation()
    {
        _patateAnimation.Play("A_PatateScoreUp");
    }

    public void PlayCarotteUpAnimation()
    {
        _patateAnimation.Play("A_CarotteScoreUp");
    }
}