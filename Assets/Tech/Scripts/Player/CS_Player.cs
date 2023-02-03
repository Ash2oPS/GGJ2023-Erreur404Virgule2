using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CS_Player : MonoBehaviour
{
    protected int _score = 1;

    [SerializeField] protected List<CS_Character> _currentPlayerControllers;
    [SerializeField] protected float _baseColliderRadius = 0.7f;
    [SerializeField] protected CharacterController _cc;
    [SerializeField] protected MeshRenderer _debugColliderSphere;
    [SerializeField] protected CS_Character _characterPrefab;

    protected CS_GameManager _gm;

    public int Score => _score;

    private void Awake()
    {
        _gm = FindObjectOfType<CS_GameManager>();
    }

    private void Start()
    {
        CharacterListInitialization();
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

    public void AddCharacter()
    {
        _score++;

        CS_Character newCharacter = Instantiate(_characterPrefab, transform);
        _currentPlayerControllers.Add(newCharacter);
        newCharacter.transform.localPosition = new Vector3(
            Random.Range(-2, 2),
            0,
            Random.Range(-2, 2)
            );

        if (this is CS_PatatePlayer)
            _gm.AddCharacterUI(0);
        else if (this is CS_CarottePlayer)
            _gm.AddCharacterUI(1);

        _cc.radius = _baseColliderRadius * _score;
        _debugColliderSphere.transform.localScale = Vector3.one * _cc.radius * 2;
        _debugColliderSphere.transform.localPosition = new Vector3(0, _cc.radius, 0);
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