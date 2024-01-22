using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAttack : MonoBehaviour
{
    [Header("# 공격 영역 관련")]
    public Vector3 m_AttackAreaSize;

    public Vector3 m_AttackAreaOffset;

    private int _TargetComboCount;
    
    private int _ComboCount;
    public int comboCount => _ComboCount;




    private bool _NextAttackInputCheck;

    private bool _IsAttack;

    /// <summary>
    /// 공격 영역이 활성화되었음을 나타냅니다.
    /// </summary>
    private bool _AttackAreaIsEnabled;

    private bool _IsRight;

    public event System.Action onAttackStarted;
    public event System.Action onAttackFinished;

    public event System.Action onAttackAreaEnabled;
    public event System.Action onAttackAreaDisabled;




    #region DEBUG
    private DrawGizmoCubeInfo _AttackAreaDrawInfo;
    #endregion


    private void Update()
    {
        if (_AttackAreaIsEnabled)
        {
            CheckAttackArea();
        }

    }

    private void CheckAttackArea()
    {
        Vector3 offset = m_AttackAreaOffset;
        offset.x *= (_IsRight) ? 1 : -1;
        Vector3 center = transform.position;

        PhysicsExt.OverlapBox(
            out _AttackAreaDrawInfo,
            center + offset,
            m_AttackAreaSize * 0.5f,
            Quaternion.identity,
            0);
    }




    public void OnAttackKeyPressed()
    {
        if (_IsAttack)
        {
            if (_NextAttackInputCheck) ++_TargetComboCount;
            //_TargetComboCount %= 4;
            _TargetComboCount = Mathf.Clamp(_TargetComboCount, 0, 3);
        }
        else
        {
            ++_TargetComboCount;
            ++_ComboCount;

        }

        
    }


    public void OnStartNextCheckStarted()
    {
        _NextAttackInputCheck = true;
        onAttackStarted?.Invoke();
    }

    public void OnNextAttackCheckFinished()
    {
        _NextAttackInputCheck = false;
        if (_TargetComboCount == _ComboCount)
        {

        }

        onAttackFinished?.Invoke();
    }



    public void AnimEvent_EnableAttackArea()
    {
        onAttackAreaEnabled?.Invoke();
    }

    private void AnimEvent_DisableAttackArea(int layerIndex)
    {
        onAttackAreaDisabled?.Invoke();
    }






    public void OnAttackStarted()
    {
        _IsAttack = true;
    }



    public void OnAttackFinished()
    {
        if (_TargetComboCount == _ComboCount)
        {
            _ComboCount = 0;
            _TargetComboCount = 0;

            _IsAttack = false;
        }
        else
        {
            ++_ComboCount;

        }
    }


    public void UpdateDirection(bool isRight)
    {
        _IsRight = isRight;
    }


    public void EnableAttackArea() => _AttackAreaIsEnabled = true;

    public void DisableAttackArea() => _AttackAreaIsEnabled = false;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_AttackAreaIsEnabled)
        {
            PhysicsExt.DrawGizmoOverlapBox(in _AttackAreaDrawInfo);
        }
    }
#endif

}
