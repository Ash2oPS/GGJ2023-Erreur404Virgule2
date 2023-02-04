using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CS_Player : MonoBehaviour
{
    protected int _score = 1;

    [SerializeField] protected List<CS_Character> _currentPlayerControllers;
    [SerializeField] protected float _baseColliderRadius = 0.7f, _radiusScaleParameter = 1;
    [SerializeField] protected CharacterController _cc;
    [SerializeField] protected MeshRenderer _debugColliderSphere;
    [SerializeField] protected CS_Character _characterPrefab;
    [SerializeField] protected CS_Indicators _indicators;

    protected CS_GameManager _gm;

    public int Score => _score;

    private void Awake()
    {
        _gm = FindObjectOfType<CS_GameManager>();
    }

    private void Start()
    {
        CharacterListInitialization();

        SetColliderSize();
    }

    protected void CharacterListInitialization()
    {
        _currentPlayerControllers = new List<CS_Character>();

        CS_Character[] _startPlayerControllers = transform.GetComponentsInChildren<CS_Character>();

        foreach (var item in _startPlayerControllers)
        {
            _currentPlayerControllers.Add(item);
        }
    }

    public void SetColliderSize()
    {
        _cc.radius = _baseColliderRadius * (1f + Mathf.Pow(_score, _radiusScaleParameter));
        _cc.center = new Vector3(0, _cc.radius, 0);
        _debugColliderSphere.transform.localScale = Vector3.one * _cc.radius * 2;
        _debugColliderSphere.transform.localPosition = new Vector3(0, _cc.radius, 0);
        _indicators.SetColliderIndicatorRadius(_cc.radius);
    }

    public void AddCharacter()
    {
        _score++;

        SetColliderSize();

        CS_Character newCharacter = Instantiate(_characterPrefab, transform);
        _currentPlayerControllers.Add(newCharacter);

        Vector2 posAsVector2 = GetRandomInVector2();

        Vector3 pos = new Vector3(
            posAsVector2.x,
            0,
            posAsVector2.y
            );

        pos *= _cc.radius;

        newCharacter.transform.localPosition = pos;

        if (this is CS_PatatePlayer)
            _gm.AddCharacterUI(0);
        else if (this is CS_CarottePlayer)
            _gm.AddCharacterUI(1);
    }

    private Vector2 GetRandomInVector2()
    {
        Vector2 output = Vector2.zero;

        while (output == Vector2.zero || output.magnitude > 1)
        {
            output = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
                );
        }

        return output;
    }

    public void RemoveCharacter(CS_Character pc)
    {
        if (!_currentPlayerControllers.Contains(pc))
        {
            Debug.LogError($"Erreur - CS_Player - {transform.name} - RemoveCharacter({pc.name}) - " +
                $"Le PlayerController renseigné ne fait pas partie de la list _currentPlayerControllers.");
            return;
        }

        CS_Character tempPc = pc;

        _currentPlayerControllers.Remove(pc);
        Destroy(tempPc.gameObject);
    }
}