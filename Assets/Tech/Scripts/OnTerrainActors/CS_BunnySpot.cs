using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BunnySpot : CS_Piege
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _bunnySprite;

    private CS_GameManager _gm;

    private void Awake()
    {
        _gm = FindObjectOfType<CS_GameManager>();
    }

    public void SetBunnyHere()
    {
        _isEnabled = true;
        _sr.sprite = _bunnySprite;
    }

    public void RemoveBunnyFromHere()
    {
        if (!_isEnabled)
        {
            return;
        }

        _sr.sprite = null;
        _isEnabled = false;
    }

    protected override void OnDisablement()
    {
        _gm.RemoveBunnyFromSpot(this, true);
    }
}