using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int _ComoboCount;
    public int comboCount => _ComoboCount;

    /// <summary>
    /// �� ĳ���͸� �����ϰ� �ִ� �÷��̾� ��Ʈ�ѷ��� ��Ÿ���ϴ�.
    /// </summary>
    private PlayerController _PlayerController;

    /// <summary>
    /// �̵� ����� ����ϴ� ������Ʈ�Դϴ�.
    /// </summary>
    private PlayerMovement _MovementComponent;

    /// <summary>
    /// Animator ������Ʈ�� �����ϱ� ���� ������Ʈ�Դϴ�.
    /// </summary>
    private PlayerCharacterAnimController _AnimController;

    private PlayerCharacterAttack _AttackComponent;


    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ����� �������ϰ� ������ ��Ÿ���ϴ�.
    /// </summary>
    public bool isControlled => _PlayerController != null;

    private void Awake()
    {
        _MovementComponent = GetComponent<PlayerMovement>();
        _AnimController = GetComponentInChildren<PlayerCharacterAnimController>();
        _AttackComponent = GetComponent<PlayerCharacterAttack>();

    }

    private void Start()
    {
        BindAnimationEvents();

        BindAttackEvents();
    }

    private void Update()
    {
        UpdateAnimControllerParam();
    }

    private void BindAnimationEvents()
    {
        _AnimController.onNextAttackCheckStarted += _AttackComponent.OnStartNextCheckStarted;
        _AnimController.onNextAttackCheckFinished += _AttackComponent.OnNextAttackCheckFinished;
        _AnimController.onAttackStarted += _AnimController_onAttackStarted;
        _AnimController.onAttackFinished += _AttackComponent.OnAttackFinished;
    }

    private void BindAttackEvents()
    {
        _AttackComponent.onAttackStarted += ()=> _MovementComponent.AllowMovementInput(false);
        _AttackComponent.onAttackFinished += ()=> _MovementComponent.AllowMovementInput(true);

        _AnimController.onAttackAreaEnabled += _AttackComponent.EnableAttackArea;
        _AnimController.onAttackAreaDisabled += _AttackComponent.DisableAttackArea;

        _MovementComponent.onDirectionChaged += _AttackComponent.UpdateDirection;
    }

    private void _AnimController_onAttackStarted()
    {
        //throw new System.NotImplementedException();
    }


    /// <summary>
    /// �ִ� ��Ʈ�ѷ� �Ķ���� ���� �����մϴ�.
    /// </summary>
    private void UpdateAnimControllerParam()
    {
        _AnimController.isMove = 
            _MovementComponent.velocity.sqrMagnitude > 0.0f;
        
        _AnimController.IsRight = _MovementComponent.isRight;

        _AnimController.isGrounded = _MovementComponent.isGrounded;

        _AnimController.comboCount = _AttackComponent.comboCount;

    }

    /// <summary>
    /// �� ĳ������ ������ ���۵� �� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="playerController">������ �����ϴ� �÷��̾� ��Ʈ�ѷ� ��ü�� ���޵˴ϴ�.</param>
    public virtual void OnControlStarted(PlayerController playerController)
    {
        _PlayerController = playerController;
    }

    /// <summary>
    /// �� ĳ������ ������ ������ �� ȣ��˴ϴ�.
    /// </summary>
    public virtual void OnControlFinished()
    {
        _PlayerController = null;
    }

    public void OnMovementInput(Vector2 axisValue) => _MovementComponent.OnMovementInput(axisValue);
    public void OnJumpInput() => _MovementComponent.OnJumpInput();

    public void OnAttackInput() => _AttackComponent.OnAttackKeyPressed();
}

