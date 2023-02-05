using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Piege : CS_OnTerrainActor
{
    [SerializeField] private int _numberOfLegumesToRemove;

    [Range(0, 3)]
    [Tooltip("whatDeath : 0 => Hit, 1 => Sploutch, 2 => Burn, 3 => Cut")]
    [SerializeField] private int _whatDeath;

    [SerializeField] private AudioSource _audioScource;
    [SerializeField] private AudioClip _activationSound;

    [SerializeField] protected bool _isEnabled;

    public bool IsEnabled => _isEnabled;

    public virtual void SetEnabled()
    {
        _isEnabled = true;
    }

    protected override void OnPlayerInteraction(CS_Player player)
    {
        if (!_isEnabled)
            return;

        StartCoroutine(TrapActivation(player));
    }

    private IEnumerator TrapActivation(CS_Player player)
    {
        _audioScource.PlaySoundWithFactor(_activationSound);

        for (int i = 0; i < _numberOfLegumesToRemove; i++)
        {
            player.RemoveCharacter(_whatDeath);
            yield return new WaitForSeconds(0.05f);
        }
        OnDisablement();
    }

    protected virtual void OnDisablement()
    {
        _isEnabled = false;
    }
}