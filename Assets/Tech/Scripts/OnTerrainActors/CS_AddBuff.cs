using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AddBuff : CS_OnTerrainActor
{
    protected override void OnPlayerInteraction(CS_Player player)
    {
        player.AddCharacter();
    }
}