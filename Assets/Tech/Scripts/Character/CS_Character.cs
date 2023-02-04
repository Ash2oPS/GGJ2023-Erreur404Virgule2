using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Character : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _groundedSprite, _hopSprite;
    private bool _canHop = true;

    private Coroutine _currentCor;

    private void Start()
    {
        _currentCor = StartCoroutine(HopCoroutine());
    }

    public void SetCanHop(bool canHop)
    {
        _canHop = canHop;

        if (_canHop)
        {
            _currentCor = StartCoroutine(HopCoroutine());
        }
        else
        {
            StopCoroutine(_currentCor);
            _currentCor = null;
        }
    }

    private IEnumerator HopCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(0.9f, 3.3f);
            yield return new WaitForSeconds(delay);
            ChangeSprite(true);
            yield return new WaitForSeconds(0.1f);
            ChangeSprite(false);
        }
    }

    private void ChangeSprite(bool hop)
    {
        _sr.sprite = hop ? _hopSprite : _groundedSprite;
    }
}