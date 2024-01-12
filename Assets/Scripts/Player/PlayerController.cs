using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter _ControlledCharacter;

    private void OnMove(InputValue value)
    {
        Vector2 axisValue = value.Get<Vector2>();
        _ControlledCharacter?.OnMovementInoput(axisValue);


    }

    private void OnJump()
    {
        _ControlledCharacter?.OnJumpInput();
    }

    public void StartControlCharacter(PlayerCharacter controlCharacter)
    {
        if (_ControlledCharacter == controlCharacter) return;

        _ControlledCharacter = controlCharacter;

        _ControlledCharacter.OnControlStarted(this);
    }

    public void FinishControlCharacter()
    {
        if (_ControlledCharacter == null) return;

        _ControlledCharacter.OnControlFinished();
        _ControlledCharacter = null;
    }

}
