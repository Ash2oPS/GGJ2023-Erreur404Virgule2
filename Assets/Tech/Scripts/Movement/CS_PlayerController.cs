using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode[] _inputs;

    private Vector2[] _directions = new Vector2[4] {
        new Vector2(0,1) ,
        new Vector2(1,0) ,
        new Vector2(0,-1) ,
        new Vector2(-1,0)
    };

    [SerializeField] private CS_Movement _movement;

    private Vector2 _currentDirection;

    private void Start()
    {
        if (_inputs.Length != 4)
        {
            Debug.LogWarning("Attention loser, t'as pas bien rempli les inputs. Pis faut que �a soit :" +
                "0 -> up, 1 -> right, 2 -> down, 3 -> left");
            return;
        }
    }

    private void Update()
    {
        _currentDirection = Vector2.zero;

        for (int i = 0; i < _inputs.Length; i++)
        {
            GetInput(i);
        }

        CallRegisterMove();
    }

    private void GetInput(int index)
    {
        if (Input.GetKey(_inputs[index]))
        {
            if (index == 0)
                _currentDirection = new Vector2(_currentDirection.x, 1);
            else if (index == 2)
                _currentDirection = new Vector2(_currentDirection.x, -1);
            if (index == 1)
                _currentDirection = new Vector2(1, _currentDirection.y);
            else if (index == 3)
                _currentDirection = new Vector2(-1, _currentDirection.y);
        }
    }

    private void CallRegisterMove()
    {
        _movement.RegisterMove(_currentDirection);
    }
}