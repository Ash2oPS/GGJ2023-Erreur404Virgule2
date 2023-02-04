using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CS_Movement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10f, _dashSpeed = 30f, _dashDelay = 1, _throwDelay = 1;
    [SerializeField] private CharacterController _cc;
    [SerializeField] private CS_Indicators _indicators;

    private Vector2 _lastDirection = new Vector2(-1, 0);

    private Vector2 _inputDirection;
    private float yDir;

    private bool _hasToDash, _isDashing;
    private bool _canDash => (!_isDashing && !_hasToDash && !_isThrowing);
    public bool IsDashing => _isDashing;

    private bool _hasToThrow, _isThrowing;
    private bool _canThrow => (!_isThrowing && !_hasToThrow && !_isDashing);
    public bool IsThrowing => _isThrowing;

    public void RegisterMove(Vector2 direction)
    {
        _inputDirection = direction;

        if (_inputDirection.magnitude > 1)
            _inputDirection.Normalize();

        if (direction != Vector2.zero)
            UpdateLastDir(direction.normalized);
    }

    private void UpdateLastDir(Vector2 dir)
    {
        _lastDirection = dir;
        _indicators.SetOrientation(Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg);
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

    private void FixedUpdate()
    {
        CalculateGravity();
        Move();
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
            _cc.Move(_lastDirection * _dashSpeed * Time.deltaTime);
    }

    private void Throw()
    {
        if (!_hasToThrow)
            return;

        _hasToThrow = false;
        StartCoroutine(ThrowCoroutine());
    }

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

    private void SetIsDashing(bool value)
    {
        _isDashing = value;

        if (_isDashing)
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item.transform.name != "Shadow")
                    item.color = Color.red;
            }
            return;
        }

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            if (item.transform.name != "Shadow")
                item.color = Color.white;
        }
    }

    private void SetIsThrowing(bool value)
    {
        _isThrowing = value;

        if (_isThrowing)
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item.transform.name != "Shadow")
                    item.color = Color.green;
            }
            return;
        }

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            if (item.transform.name != "Shadow")
                item.color = Color.white;
        }
    }
}