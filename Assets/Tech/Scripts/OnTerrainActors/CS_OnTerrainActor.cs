using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CS_OnTerrainActor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CS_Player player))
            OnPlayerInteraction(player);
    }

    protected virtual void OnPlayerInteraction(CS_Player player)
    {
    }
}