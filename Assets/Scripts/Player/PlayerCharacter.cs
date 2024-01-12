using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private PlayerController _PlayerController;

    private PlayerMovement _MovementComponent;

    public bool isControlled => _PlayerController != null;


    private void Awake()
    {
        _MovementComponent = GetComponent<PlayerMovement>();
    }



    public void OnControlStarted(PlayerController playerController)
    {
        _PlayerController = playerController;
    }

    public virtual void OnControlFinished()
    {
        _PlayerController = null;
    }

    public void OnMovementInoput(Vector2 axisValue) => _MovementComponent.OnMovementInput(axisValue);

    public void OnJumpInput() => _MovementComponent.OnJumpInput();

}
