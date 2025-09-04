using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    [SerializeField] private GameObject _sumoPrefab;
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private PlayerUI[] _playersUI;
    
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
        Debug.Log("qdqzdsqd");
        _players.Add(player.GetComponent<Player>());
        _playersUI[^1].Init(_players[^1]);
    }
    
    
}
