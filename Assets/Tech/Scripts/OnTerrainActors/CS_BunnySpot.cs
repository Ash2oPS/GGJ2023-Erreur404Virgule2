using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BunnySpot : CS_Piege
{
    [SerializeField] private SkinnedMeshRenderer _bunnyRenderer;
    [SerializeField] private Animation _anim;

    private CS_GameManager _gm;

    private void Awake()
    {
        _gm = FindObjectOfType<CS_GameManager>();
    }

    public void SetBunnyHere()
    {
        SetEnabled();
    }

    public override void SetEnabled()
    {
        _anim.Play("A_TerrierMove");
    }

    public void BunnyApper()
    {
        _isEnabled = true;
        _bunnyRenderer.enabled = true;
    }

    public void RemoveBunnyFromHere()
    {
        if (!_isEnabled)
        {
            return;
        }

        _bunnyRenderer.enabled = false;
        _isEnabled = false;
    }

    protected override void OnDisablement()
    {
        _gm.RemoveBunnyFromSpot(this, true);
    }
}