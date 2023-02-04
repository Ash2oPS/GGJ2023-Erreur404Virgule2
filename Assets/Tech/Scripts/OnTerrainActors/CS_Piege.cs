using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Piege : CS_OnTerrainActor
{
    [SerializeField] private int _numberOfLegumesToRemove;

    [Range(0, 3)]
    [Tooltip("whatDeath : 0 => Hit, 1 => Sploutch, 2 => Burn, 3 => Cut")]
    [SerializeField] private int _whatDeath;

    [SerializeField] private bool _isEnabled;

    protected override void OnPlayerInteraction(CS_Player player)
    {
        if (!_isEnabled)
            return;

        StartCoroutine(TrapActivation(player));
    }

    private IEnumerator TrapActivation(CS_Player player)
    {
        for (int i = 0; i < _numberOfLegumesToRemove; i++)
        {
            player.RemoveCharacter(_whatDeath);
            yield return new WaitForSeconds(0.05f);
        }

        _isEnabled = false;
    }
}