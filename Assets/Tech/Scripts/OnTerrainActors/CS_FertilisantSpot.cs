using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CS_FertilisantSpot : CS_Piege
{
    [SerializeField] private CS_AddBuff _normalPrefab;
    [SerializeField] private CS_AddBuffSuper _superPrefab;
    [SerializeField] private CS_AddBuffMega _megaPrefab;

    private CS_GameManager _gm;

    private void Awake()
    {
        _gm = FindObjectOfType<CS_GameManager>();
    }

    /// <summary>
    /// 0 - normal, 1 - super, 2 - mega
    /// </summary>
    public void SpawnFertilisant(int index)
    {
        GameObject go = null;

        switch (index)
        {
            case 0:
                go = _normalPrefab.gameObject;
                break;

            case 1:
                go = _superPrefab.gameObject;
                break;

            case 2:
                go = _megaPrefab.gameObject;
                break;
        }

        _isEnabled = true;
        GameObject newGo = Instantiate(go, transform);
        newGo.transform.localPosition = Vector3.zero;
    }

    protected override void OnPlayerInteraction(CS_Player player)
    {
        _isEnabled = false;
        _gm.RemoveFertilisant();
    }
}