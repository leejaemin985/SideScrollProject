using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int _ComoboCount;
    public int comboCount => _ComoboCount;

    /// <summary>
    /// 이 캐릭터를 조종하고 있는 플레이어 컨트롤러를 나타냅니다.
    /// </summary>
    private PlayerController _PlayerController;

    /// <summary>
    /// 이동 기능을 담당하는 컴포넌트입니다.
    /// </summary>
    private PlayerMovement _MovementComponent;

    /// <summary>
    /// Animator 컴포넌트를 제어하기 위한 컴포넌트입니다.
    /// </summary>
    private PlayerCharacterAnimController _AnimController;

    private PlayerCharacterAttack _AttackComponent;


    /// <summary>
    /// 플레이어 컨트롤러에게 조종당하고 있음을 나타냅니다.
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
    /// 애님 컨트롤러 파라미터 값을 갱신합니다.
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
    /// 이 캐릭터의 조종이 시작될 때 호출됩니다.
    /// </summary>
    /// <param name="playerController">조종을 시작하는 플레이어 컨트롤러 객체가 전달됩니다.</param>
    public virtual void OnControlStarted(PlayerController playerController)
    {
        _PlayerController = playerController;
    }

    /// <summary>
    /// 이 캐릭터의 조종이 끝났을 때 호출됩니다.
    /// </summary>
    public virtual void OnControlFinished()
    {
        _PlayerController = null;
    }

    public void OnMovementInput(Vector2 axisValue) => _MovementComponent.OnMovementInput(axisValue);
    public void OnJumpInput() => _MovementComponent.OnJumpInput();

    public void OnAttackInput() => _AttackComponent.OnAttackKeyPressed();
}

