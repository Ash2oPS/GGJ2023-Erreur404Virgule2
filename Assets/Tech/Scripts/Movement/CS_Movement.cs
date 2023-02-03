using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Movement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private CharacterController _cc;

    private Vector2 _inputDirection;
    private float yDir;

    public void RegisterMove(Vector2 direction)
    {
        _inputDirection = direction;

        if (_inputDirection.magnitude > 1)
            _inputDirection.Normalize();
    }

    private void FixedUpdate()
    {
        CalculateGravity();
        Move();
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
        Vector3 trueDirection = new Vector3(_inputDirection.x * _walkSpeed * Time.deltaTime,
            yDir,
            _inputDirection.y * _walkSpeed * Time.deltaTime);

        _cc.Move(trueDirection);
    }
}