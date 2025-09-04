using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    [SerializeField] private GameObject _sumoPrefab;
    [SerializeField] private PlayerInputManager _playerInputManager;
    
    private List<Player> _players = new();
    public IReadOnlyCollection<Player> PlayersList => _players;

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
    }
    
    
}
