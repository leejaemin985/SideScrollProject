using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerController : MonoBehaviour
{
	private PlayerCharacter _ControlledCharacter;

	private void OnMove(InputValue value)
	{
		Vector2 axisValue = value.Get<Vector2>();
		_ControlledCharacter?.OnMovementInput(axisValue);
	}

	private void OnJump()
	{
		_ControlledCharacter?.OnJumpInput();
	}


	private void OnAttack()
	{
		_ControlledCharacter?.OnAttackInput();
	}

	/// <summary>
	/// 조종을 시작할 캐릭터를 설정합니다.
	/// </summary>
	/// <param name="controlCharacter"></param>
	public void StartControlCharacter(PlayerCharacter controlCharacter)
	{
		// 이미 전달된 캐릭터를 조종중이라면 함수 호출 종료
		if (_ControlledCharacter == controlCharacter) return;

		_ControlledCharacter = controlCharacter;

		// 캐릭터 조종을 시작합니다.
		_ControlledCharacter.OnControlStarted(this);
	}

	/// <summary>
	/// 조종을 끝냅니다.
	/// </summary>
	public void FinishControlCharacter()
	{
		// 만약 조종중인 캐릭터가 존재하지 않는다면 함수 호출 종료
		if (_ControlledCharacter == null) return;

		_ControlledCharacter.OnControlFinished();
		_ControlledCharacter = null;
	}

}
