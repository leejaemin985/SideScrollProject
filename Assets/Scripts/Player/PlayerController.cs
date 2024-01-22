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
	/// ������ ������ ĳ���͸� �����մϴ�.
	/// </summary>
	/// <param name="controlCharacter"></param>
	public void StartControlCharacter(PlayerCharacter controlCharacter)
	{
		// �̹� ���޵� ĳ���͸� �������̶�� �Լ� ȣ�� ����
		if (_ControlledCharacter == controlCharacter) return;

		_ControlledCharacter = controlCharacter;

		// ĳ���� ������ �����մϴ�.
		_ControlledCharacter.OnControlStarted(this);
	}

	/// <summary>
	/// ������ �����ϴ�.
	/// </summary>
	public void FinishControlCharacter()
	{
		// ���� �������� ĳ���Ͱ� �������� �ʴ´ٸ� �Լ� ȣ�� ����
		if (_ControlledCharacter == null) return;

		_ControlledCharacter.OnControlFinished();
		_ControlledCharacter = null;
	}

}
