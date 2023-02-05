using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_CountDown : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _sprites;

    public void SetNumber(int index)
    {
        _image.sprite = _sprites[index];
        _animation.Play();
    }
}