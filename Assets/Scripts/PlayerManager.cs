using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    

    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private PlayerUI[] _playersUI;
    [SerializeField] private Transform[] _playerSpawn;
    [SerializeField] private CanvasGroup _mainMenuCanva;
    [SerializeField] private CanvasGroup endMenuCanva;
    [SerializeField] private AnimatorController[] _animatorControllers;
    
    private List<Player> _players = new();
    public IReadOnlyCollection<Player> PlayersList => _players;

    public event Action OnGameStart;
    public event Action OnGameEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _playerInputManager.onPlayerJoined += RegisterPlayer;
    }

    private void OnDestroy()
    {
        _playerInputManager.onPlayerJoined -= RegisterPlayer;
    }
    

    private void RegisterPlayer(PlayerInput player)
    {
        _players.Add(player.GetComponent<Player>());
        _playersUI[_players.Count - 1].Init(_players[^1]);
        _players[^1].transform.position = _playerSpawn[_players.Count - 1].position;
        _players[^1].Movement.Animator1.runtimeAnimatorController = _animatorControllers[_players.Count - 1];

        if (_players.Count >= 2)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        OnGameStart?.Invoke();
    }
    
    
}
