using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandeler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    public event Action<Vector2> OnMovementInputOccured; 
    public event Action<Vector2> OnMovementInputRelease;

    public void RetreiveMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnMovementInputOccured?.Invoke(ctx.ReadValue<Vector2>());
        }

        if (ctx.canceled)
        {
            OnMovementInputRelease?.Invoke(ctx.ReadValue<Vector2>());
        }
    }
}
