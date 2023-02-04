using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CS_Character : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _groundedSprite, _hopSprite, _preBigHopSprite, _bigHopSprite;
    [SerializeField] private AnimationCurve _bigHopCurve;
    private bool _canHop = true;

    private Coroutine _currentCor;

    private bool _hasToBigHop, _isBigHopping;
    private float _baseY, _bigHopT, _timer;

    private void Start()
    {
        _currentCor = StartCoroutine(HopCoroutine());
        _baseY = _sr.transform.localPosition.y;
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

    public void SetSpriteDir(int dir)
    {
        if (dir == -1)
        {
            _sr.transform.localEulerAngles = new Vector3(30, 0, 0);
        }
        else if (dir == 1)
        {
            _sr.transform.localEulerAngles = new Vector3(-30, 180, 0);
        }
    }

    private IEnumerator HopCoroutine()
    {
        while (true)
        {
            int bigHopRandom = Random.Range(0, 5);
            bool _isBigHop = bigHopRandom == 4;
            float delay = Random.Range(0.4f, 2.4f);
            yield return new WaitForSeconds(delay);

            if (!_isBigHop)
            {
                ChangeSprite(_hopSprite);
                yield return new WaitForSeconds(0.1f);
                ChangeSprite(_groundedSprite);
            }
            else
            {
                ChangeSprite(_preBigHopSprite);
                yield return new WaitForSeconds(0.2f);
                ChangeSprite(_bigHopSprite);
                _hasToBigHop = true;
                yield return new WaitForSeconds(0.25f);
                ChangeSprite(_groundedSprite);
            }
        }
    }

    private void Update()
    {
        if (!_hasToBigHop && !_isBigHopping)
            return;

        if (_hasToBigHop)
        {
            _hasToBigHop = false;
            _isBigHopping = true;
            _bigHopT = 0;
            _timer = 0;
        }

        _timer += Time.deltaTime;

        _bigHopT = Mathf.Lerp(0, 1, _timer / 0.25f);

        float posY = _bigHopCurve.Evaluate(_bigHopT);

        _sr.transform.localPosition = new Vector3(0, _baseY + posY, 0);
    }

    private void ChangeSprite(Sprite sprite)
    {
        _sr.sprite = sprite;
    }
}