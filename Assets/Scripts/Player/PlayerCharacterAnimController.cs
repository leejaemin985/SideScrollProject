using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAnimController : MonoBehaviour
{
    private Animator _Animator;
    public SpriteRenderer _SpriteRenderer;

    public Animator animator => _Animator ??
        (_Animator ?? GetComponent<Animator>());


    public SpriteRenderer spriteRenderer => _SpriteRenderer ??
        (_SpriteRenderer = GetComponent<SpriteRenderer>());




    public event System.Action onNextAttackCheckStarted;
           
    public event System.Action onNextAttackCheckFinished;

    public event System.Action onAttackStarted;

    public event System.Action onAttackFinished;


    /// <summary>
	/// 공격 영역 활성화 이벤트
	/// </summary>
	public event System.Action onAttackAreaEnabled;

    /// <summary>
    /// 공격 영역 비활성화 이벤트
    /// </summary>
    public event System.Action onAttackAreaDisabled;


    public bool isMove
    {
        set => animator.SetFloat(Constants.ANIMPARAM_ISMOVE, value ? 1.0f : 0.0f);
    }
    public bool isGrounded
    {
        set => animator.SetBool(Constants.ANIMPARAM_ISGROUNDED, value);
    }

    public bool IsRight { set => spriteRenderer.flipX = !value; }

    public int comboCount { set => animator.SetInteger(Constants.ANIMPARAM_COMBOCOUNT, value); }






    #region Animation Event
    private void AnimEvent_StartNextAttackCheck()
    {
        onNextAttackCheckStarted?.Invoke();
    }
    private void AnimEvenmt_FinishNextAttackCheck()
    {
        onNextAttackCheckFinished?.Invoke();
    }

    private void AnimEvent_StartAttack()
    {
        onAttackStarted?.Invoke();
    }

    private void AnimEvent_FinishAttack()
    {
        onAttackFinished?.Invoke();
    }

    private void AnimEvent_EnableAttackArea()
    {
        onAttackAreaEnabled?.Invoke();
    }

    private void AnimEvent_DisableAttackArea()
    {
        onAttackAreaDisabled?.Invoke();
    }
    #endregion


}
