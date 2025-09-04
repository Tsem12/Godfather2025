using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private GameObject _sumoPrefab;
    
    void Start()
    {
        // PlayerInput prout = PlayerInput.Instantiate(_sumoPrefab,  pairWithDevice: Keyboard.current);
        // PlayerInput prout2 = PlayerInput.Instantiate(_sumoPrefab, pairWithDevice: Keyboard.current);
        //
        // prout.SwitchCurrentActionMap("Player1");
        // prout2.SwitchCurrentActionMap("Player2");
    }
}
