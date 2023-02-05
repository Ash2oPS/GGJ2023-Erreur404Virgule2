using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CS_Character : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _groundedSprite, _hopSprite, _preBigHopSprite, _bigHopSprite;
    [SerializeField] private Sprite _deadByHitSprite, _deadBySploucthSprite, _deadByBurnSprite, _deadByCutSprite;
    [SerializeField] private AnimationCurve _bigHopCurve;
    [SerializeField] private Animation _animation;
    [SerializeField] private string _animationFileName;

    public CS_Player _myPlayer;

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

    /// <summary>
    /// whatDeath : 0 => Hit, 1 => Sploutch, 2 => Burn, 3 => Cut
    /// </summary>
    public void DieFromCringe(int whatDeath)
    {
        StartCoroutine(DieCoroutine(whatDeath));
    }

    private IEnumerator DieCoroutine(int whatDeath)
    {
        Sprite deathSprite = null;

        switch (whatDeath)
        {
            case 0:
                deathSprite = _deadByHitSprite;
                break;

            case 1:
                deathSprite = _deadBySploucthSprite;
                break;

            case 2:
                deathSprite = _deadByBurnSprite;
                break;

            case 3:
                deathSprite = _deadByCutSprite;
                break;
        }

        transform.parent = null;
        SetCanHop(false);

        _animation.Play(_animationFileName);

        _sr.sprite = deathSprite;
        yield return new WaitForSeconds(0.45f);
        _sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.35f);
        _sr.color = Color.white;
        yield return new WaitForSeconds(0.35f);
        _sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.15f);
        _sr.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        _sr.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.1f);
        _sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    public void BeingHeld(int index)
    {
        StartCoroutine(HeldCoroutine(index));
    }

    private IEnumerator HeldCoroutine(int index)
    {
        float timer = 0;
        Vector3 basePos = transform.localPosition;
        Vector3 destPos = new Vector3(0, 2 + index, 0);

        while (timer < 1)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, 1);

            transform.localPosition = Vector3.Lerp(basePos, destPos, timer);

            yield return new WaitForEndOfFrame();
        }
    }

    public void BeingThrown()
    {
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

        if (transform.position.y <= -5)
        {
            _myPlayer.RemoveCharacter(this, 0);
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        _sr.sprite = sprite;
    }
}