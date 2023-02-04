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

    private Vector2 _lastDirection = new Vector2(0, -1);
    private Vector2 _beingPushedDir;
    private Vector2 _dashDir;

    private Vector2 _inputDirection;
    private float yDir;

    private bool _hasToDash, _isDashing;
    private bool _canDash => (!_isDashing && !_hasToDash && !_isThrowing && !_isBeingPushed);
    public bool IsDashing => _isDashing;

    private bool _hasToThrow, _isThrowing;
    private bool _canThrow => (!_isThrowing && !_hasToThrow && !_isDashing && !_isBeingPushed);
    public bool IsThrowing => _isThrowing;

    private bool _isBeingPushed;
    public bool IsBeingPushed => _isBeingPushed;

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

    public void RegisterThrow()
    {
        if (_canThrow)
        {
            _hasToThrow = true;
        }
    }

    #endregion Register Inputs

    #region Fixed Updates

    private void FixedUpdate()
    {
        CalculateGravity();
        Move();
        BeingPushed();
        Dash();
        Throw();
    }

    private void CalculateGravity()
    {
        if (_cc.isGrounded)
        {
            yDir = -1;
            return;
        }

        yDir -= 1 * Time.deltaTime;
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
            yDir,
            actualDir.y * _walkSpeed * Time.deltaTime);

        _cc.Move(trueDirection);
    }

    private void BeingPushed()
    {
        if (!_isBeingPushed)
            return;

        Debug.Log($"Being pushed {transform.name} {_beingPushedDir}");

        Vector3 dirToBeingPushed = new Vector3(_beingPushedDir.x, 0, _beingPushedDir.y);
        _cc.Move(dirToBeingPushed * _beingPushedSpeed * Time.deltaTime);
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

    private void Throw()
    {
        if (!_hasToThrow)
            return;

        _hasToThrow = false;
        StartCoroutine(ThrowCoroutine());
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

    private IEnumerator ThrowCoroutine()
    {
        SetIsThrowing(true);
        yield return new WaitForSeconds(_throwDelay);
        SetIsThrowing(false);
    }

    private IEnumerator BeingPushedCoroutine()
    {
        _isBeingPushed = true;
        SetInBeingHurt(true);
        yield return new WaitForSeconds(_beingPushedDelay);
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

    private void SetIsThrowing(bool value)
    {
        _isThrowing = value;

        if (_isThrowing)
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item.transform.name.Contains("Carotte") || item.transform.name.Contains("Patate"))
                    item.color = Color.green;
            }
            return;
        }

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            if (item.transform.name.Contains("Carotte") || item.transform.name.Contains("Patate"))
                item.color = Color.white;
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
                item.SetCanHop(true);
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
}