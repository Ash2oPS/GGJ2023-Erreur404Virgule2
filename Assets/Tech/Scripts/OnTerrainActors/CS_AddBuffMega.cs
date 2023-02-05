using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AddBuffMega : CS_OnTerrainActor
{
    protected override void OnPlayerInteraction(CS_Player player)
    {
        int numberOfScore = player.Score;

        for (int i = 0; i < numberOfScore; i++)
        {
            player.AddCharacter();
        }

        Destroy(gameObject);
    }
}