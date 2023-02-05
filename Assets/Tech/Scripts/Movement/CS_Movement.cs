using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CS_Movement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10f, _dashSpeed = 30f, _beingPushedSpeed, _dashDelay = 1, _throwDelay = 1, _beingPushedDelay = 1;
    [SerializeField] private CharacterController _cc;
    [SerializeField] private CS_Indicators _indicators;
    [SerializeField] private Sprite _idleSprite, _dashSprite, _hurtSprite;
    [SerializeField] private float _tempsEntreChaqueLegumePorte = 1f;

    private Vector2 _lastDirection = new Vector2(0, -1);
    private Vector2 _beingPushedDir;
    private Vector2 _dashDir;

    private float durationBeingPushed = 0f;
    [SerializeField] private AnimationCurve beingPushedSpeedCurve;

    private float _timeSinceNotGrounded;

    private Vector2 _inputDirection;
    private float yDir;

    private bool _hasToDash, _isDashing;
    private bool _canDash => (!_isDashing && !_hasToDash && !_isHolding && !_isBeingPushed);
    public bool IsDashing => _isDashing;

    private bool _isHolding;
    private bool _boolHasToThrow => (!_isHolding && _numberOfLegumesHeld > 0);
    private bool _canHold => (!_isDashing && !_isBeingPushed);

    private bool _isBeingPushed;
    public bool IsBeingPushed => _isBeingPushed;

    private Coroutine _currentHoldingCoroutine;

    private void Start()
    {
        UpdateLastDir(_lastDirection);
    }

    #region Register Inputs

    public void RegisterMove(Vector2 direction)
    {
        _inputDirection = direction;

        if (_inputDirection.magnitude > 1)
            _inputDirection.Normalize();

        if (direction != Vector2.zero)
            UpdateLastDir(direction.normalized);
    }

    public void RegisterDash()
    {
        if (_canDash)
        {
            _hasToDash = true;
        }
    }

    private int _numberOfLegumesHeld = 0;
    [SerializeField] private float _delayBetweenNewHeldLegume;
    [SerializeField] List<CS_Character> charactersBeingHeld;

    public void RegisterHold()
    {
        _isHolding = true;

        _currentHoldingCoroutine = StartCoroutine(HoldingCoroutine());
    }

    private IEnumerator HoldingCoroutine()
    {
        if (charactersBeingHeld == null) charactersBeingHeld = new List<CS_Character>();
        charactersBeingHeld.Clear();

        bool isCarotte = TryGetComponent(out CS_CarottePlayer c);
        bool isPatate = TryGetComponent(out CS_PatatePlayer p);

        int _currentScore = 0;

        if (isCarotte)
            _currentScore = c.Score;
        else _currentScore = p.Score;

        while (_numberOfLegumesHeld < 3 && _currentScore > 1)
        {
            yield return new WaitForSeconds(_tempsEntreChaqueLegumePorte);
            _currentScore--;

            CS_Character oui = GetFarthestCharacter();

            charactersBeingHeld.Add(oui);

            if (isCarotte)
                c.RemoveCharacter(oui, -1);
            else p.RemoveCharacter(oui, -1);

            GetFarthestCharacter().SpawnParticle();
            GetFarthestCharacter().BeingHeld(_numberOfLegumesHeld + 1);
            _numberOfLegumesHeld++;
        }
    }

    public void RegisterStopHold()
    {
        Debug.Log("VOLE");
        StopCoroutine(_currentHoldingCoroutine);

        _currentHoldingCoroutine = null;

        _isHolding = false;

        if (_numberOfLegumesHeld > 0)
        {
            ThrowHeldCharacters();
        }
    }

    public void ThrowHeldCharacters()
    {
        Debug.Log("VOLE2");
        foreach (var legume in charactersBeingHeld)
        {
            legume.transform.parent = null;
            Debug.Log("VOLE3");
            MoveCharacter(legume);
        }
    }
    private void MoveCharacter(CS_Character character)
    {
        Vector2 actualDir = _inputDirection;

        Vector3 trueDirection = new Vector3(actualDir.x * _walkSpeed * 3f * Time.deltaTime,
            yDir * Time.deltaTime,
            actualDir.y * _walkSpeed * 3f * Time.deltaTime);

        StartCoroutine(CharacterFlying(trueDirection, character));
    }
    private IEnumerator CharacterFlying(Vector3 velocity, CS_Character character)
    {
        character.transform.position = new Vector3(character.transform.position.x, 0.3f, character.transform.position.z);
        while (true)
        {
            character.transform.position += velocity;
            yield return new WaitForEndOfFrame();
        }
    }

    private void Update()
    {
        //if (_hasToThrow)
        //{
        //}
    }

    #endregion Register Inputs

    #region Fixed Updates

    private void FixedUpdate()
    {
        CalculateGravity();
        Move();
        BeingPushed();
        Dash();
    }

    private void CalculateGravity()
    {
        if (_cc.isGrounded)
        {
            yDir = 0;
            return;
        }

        yDir -= 10 * Time.deltaTime;
    }

    private void Move()
    {
        if (_isBeingPushed)
            return;

        Vector2 actualDir = _inputDirection;

        if (_isDashing)
        {
            actualDir = Vector2.zero;
        }

        Vector3 trueDirection = new Vector3(actualDir.x * _walkSpeed * Time.deltaTime,
            yDir * Time.deltaTime,
            actualDir.y * _walkSpeed * Time.deltaTime);

        _cc.Move(trueDirection);
    }

    private void BeingPushed()
    {
        if (!_isBeingPushed)
            return;

        Vector3 dirToBeingPushed = new Vector3(_beingPushedDir.x, 0, _beingPushedDir.y);
        _cc.Move(dirToBeingPushed * _beingPushedSpeed * Time.deltaTime * beingPushedSpeedCurve.Evaluate(durationBeingPushed / _beingPushedDelay));
    }

    private void Dash()
    {
        if (!_hasToDash && !_isDashing)
            return;

        if (!_isDashing)
        {
            _hasToDash = false;
            StartCoroutine(DashCoroutine());
        }
        else if (!_hasToDash)
        {
            Vector3 dirToDash = new Vector3(_dashDir.x, 0, _dashDir.y);
            _cc.Move(dirToDash * _dashSpeed * Time.deltaTime);
        }
    }

    #endregion Fixed Updates

    private void StopDashDirection()
    {
        _dashDir = Vector2.zero;
    }

    private void UpdateLastDir(Vector2 dir)
    {
        if (_lastDirection.x != dir.x)
        {
            foreach (var item in GetComponentsInChildren<CS_Character>())
            {
                item.SetSpriteDir(Mathf.RoundToInt(dir.x));
            }
        }

        _lastDirection = dir;
        _indicators.SetOrientation(Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg);
    }

    #region Coroutines

    private IEnumerator DashCoroutine()
    {
        SetIsDashing(true);
        yield return new WaitForSeconds(_dashDelay);
        SetIsDashing(false);
    }

    private IEnumerator BeingPushedCoroutine()
    {
        _isBeingPushed = true;
        SetInBeingHurt(true);
        durationBeingPushed = 0f;

        foreach (var item in GetComponentsInChildren<CS_Character>())
        {
            item.SetPushed(true);
        }

        while (durationBeingPushed < _beingPushedDelay)
        {
            durationBeingPushed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        foreach (var item in GetComponentsInChildren<CS_Character>())
        {
            item.SetPushed(false);
        }

        _isBeingPushed = false;
        _beingPushedDir = Vector2.zero;
        SetInBeingHurt(false);
    }

    #endregion Coroutines

    #region Setters

    public void StartBeingPushed(Vector2 dir)
    {
        if (_isBeingPushed || _isDashing)
            return;

        _beingPushedDir = dir;
        StartCoroutine(BeingPushedCoroutine());
    }

    private void SetIsDashing(bool value)
    {
        _isDashing = value;
        _dashDir = _lastDirection;

        if (_isDashing)
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item.transform.name.Contains("Carotte") || item.transform.name.Contains("Patate"))
                    item.sprite = _dashSprite;
            }

            foreach (var item in GetComponentsInChildren<CS_Character>())
            {
                item.SetCanHop(false);
            }
            return;
        }

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            if (item.transform.name.Contains("Carotte") || item.transform.name.Contains("Patate"))
                item.sprite = _idleSprite;
        }

        foreach (var item in GetComponentsInChildren<CS_Character>())
        {
            item.SetCanHop(true);
        }
    }

    private void SetInBeingHurt(bool value)
    {
        if (value)
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item.transform.name.Contains("Carotte") || item.transform.name.Contains("Patate"))
                    item.sprite = _hurtSprite;
            }

            foreach (var item in GetComponentsInChildren<CS_Character>())
            {
                item.SetCanHop(false);
            }
            return;
        }

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            if (item.transform.name.Contains("Carotte") || item.transform.name.Contains("Patate"))
                item.sprite = _idleSprite;
        }

        foreach (var item in GetComponentsInChildren<CS_Character>())
        {
            item.SetCanHop(true);
        }
    }

    #endregion Setters

    #region Other Players Actions

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_dashDir == Vector2.zero || !_isDashing)
            return;

        if (hit.transform.TryGetComponent(out CS_Movement otherPlayer))
        {
            Push(otherPlayer);
        }
    }

    private void Push(CS_Movement otherPlayer)
    {
        otherPlayer.StartBeingPushed(_dashDir);
        StopDashDirection();
    }

    #endregion Other Players Actions

    private CS_Character GetFarthestCharacter()
    {
        float dist = 0f;
        CS_Character output = null;

        foreach (var item in GetComponentsInChildren<CS_Character>())
        {
            if (item.transform.localPosition.magnitude > dist && !item.IsHeld)
            {
                dist = item.transform.localPosition.magnitude;
                output = item;
            }
        }

        return output;
    }
}