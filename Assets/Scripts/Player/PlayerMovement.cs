using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// �浹ü ������ ���� ������Ʈ ������ �߰��Ǵ� �β�
    /// </summary>
    [Header("# ������Ʈ �е�")]
    public float m_SkinWidth;

    [Header("# ĳ���� �̵� ����")]
    /// <summary>
    /// �̵� �ӷ��Դϴ�.
    /// </summary>
    public float m_Speed;

    /// <summary>
    /// ���� ���Դϴ�.
    /// </summary>
    public float m_JumpPower;

    /// <summary>
    /// �߷¿� ����� �¼��Դϴ�.
    /// </summary>
    public float m_GravityMultiplier;

    [Header("# �浹 ����")]
    /// <summary>
    /// �̵��� ��Ͻ�ų ���̾ ��Ÿ���ϴ�.
    /// </summary>
    public LayerMask m_BlockMovementLayer;


    /// <summary>
    /// �Է� �� ���Դϴ�.
    /// </summary>
    private Vector3 _InputVelocity;

    /// <summary>
    /// ���� ����� �ӵ�
    /// </summary>
    private Vector3 _Velocity;

    /// <summary>
    /// ĳ���Ͱ� ���� ��������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsGrounded;

    /// <summary>
    /// ���� �Է��� �������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsJump;

    /// <summary>
    /// �������� �ٶ�	���� ������ ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsRight;

    /// <summary>
    /// �̵� �Է��� ����մϴ�.
    /// </summary>
    private bool _AllowMovementInput = true;

    /// <summary>
    /// ĳ���� ������ ��Ÿ���ϴ�.
    /// </summary>
    private BoxCollider _BoxCollider;

    public BoxCollider boxCollider => _BoxCollider ??
        (_BoxCollider = GetComponent<BoxCollider>());

    /// <summary>
    /// �ӵ��� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public Vector3 velocity => _Velocity;

    /// <summary>
    /// �������� �ٶ󺸰� ������ ��Ÿ���ϴ�.
    /// </summary>
    public bool isRight => _IsRight;

    /// <summary>
    /// ���� ��������� ��Ÿ���ϴ�.
    /// </summary>
    public bool isGrounded => _IsGrounded;


    /// <summary>
    /// ������ ����Ǿ��� ��� �߻��ϴ� �̺�Ʈ
    /// isRight: ������ ������ ��� True�� ���޵˴ϴ�.
    /// </summary>
    public event System.Action<bool> onDirectionChaged;


    #region DEBUG
    private DrawGizmoCubeInfo _CheckXDrawGizmoInfo;
    private DrawGizmoCubeInfo _CheckFloorDrawGizmoInfo;
    #endregion


    private void FixedUpdate()
    {
        // ��� �߻� ���� ��ġ
        Vector3 origin = transform.position + boxCollider.center;

        // ĳ���� ���� ũ��
        Vector3 halfSize = boxCollider.size * 0.5f;

        // X �� �ӵ� ���
        CalculateVelocityX(origin, halfSize);

        // Y �� �ӵ� ���
        CalculateVelocityY(origin, halfSize);

        // ������ �����մϴ�.
        UpdateDirection();

        // ���� �ӵ��� �̵��մϴ�.
        transform.position += _Velocity;
    }

    /// <summary>
    /// X �� �ӵ��� ����մϴ�.
    /// </summary>
    private void CalculateVelocityX(Vector3 origin, Vector3 halfSize)
    {
        float velocityX = _Velocity.x;

        // �Է� ���� �ӵ��� ��ȯ�մϴ�.
        velocityX = (_AllowMovementInput ? _InputVelocity.x : 0.0f) * m_Speed * Time.fixedDeltaTime;

        // X �� �������� �� �簢���� ���� ũ��
        Vector3 halfZYSizeWithSkin = halfSize - (Vector3.one * m_SkinWidth);
        halfZYSizeWithSkin.x = 0.0f;

        Vector3 direction = Vector3.right * Mathf.Sign(_InputVelocity.x);

        // �˻� ����
        float maxDistance = halfSize.x + Mathf.Abs(velocityX);

        RaycastHit hitResult;
        bool isHit = CheckCollision(
            origin,
            halfZYSizeWithSkin,
            direction,
            maxDistance,
            out hitResult,
            out _CheckXDrawGizmoInfo);

        if (isHit)
        {
            float distance = hitResult.distance - halfSize.x;

            // ���� ������ ��� ������ �̵��ϸ�, ���� ������� �ʵ��� �մϴ�.
            int sign = direction.x > 0.0f ? 1 : -1;
            velocityX = distance * sign;
        }

        _Velocity.x = velocityX;
    }

    private void CalculateVelocityY(Vector3 origin, Vector3 halfSize)
    {
        _IsGrounded = false;

        float gravity = Mathf.Abs(Physics.gravity.y) * m_GravityMultiplier * Time.fixedDeltaTime;
        float velocityY = _Velocity.y;

        // �߷� ����
        velocityY -= gravity;

        if (_IsJump)
        {
            _IsJump = false;
            velocityY = m_JumpPower;
        }


        // Y �� �������� �� ����� ���� ũ�⸦ ����մϴ�.
        Vector3 halfXZSizeWithSkin = halfSize - (Vector3.one * m_SkinWidth);
        halfXZSizeWithSkin.y = 0.0f;

        // ����
        Vector3 direction = Vector3.up * Mathf.Sign(velocityY);

        // �˻� ����
        float maxDistance = halfSize.y + Mathf.Abs(velocityY);

        RaycastHit hitResult;
        bool isHit = CheckCollision(
            origin,
            halfXZSizeWithSkin,
            direction,
            maxDistance,
            out hitResult,
            out _CheckFloorDrawGizmoInfo);

        if (isHit)
        {
            if (velocityY < 0.0f) _IsGrounded = true;

            float distance = hitResult.distance - halfSize.y;
            velocityY = distance * direction.y;
        }

        _Velocity.y = velocityY;
    }




    private bool CheckCollision(
        Vector3 start,
        Vector3 halfSize,
        Vector3 direction,
        float maxDistance,
        out RaycastHit hitResdult,
        out DrawGizmoCubeInfo drawGizmoInfo)
    {
        bool isHit = PhysicsExt.BoxCast(
            out drawGizmoInfo,
            // Boxcast ���� ��ġ
            start,
            // �߻��ų Box �� ���� ũ��
            halfSize,
            // �߻� ����
            direction,
            // ���� ����� ��ȯ���� ���� ����
            out hitResdult,
            // �߻��ų Box �� ȸ���� ����
            Quaternion.identity,
            // �߻� �Ÿ� ����
            maxDistance,
            // ������ų ���̾� ����
            m_BlockMovementLayer,
            // ���� ��� ����
            QueryTriggerInteraction.UseGlobal);

        return isHit;
    }



    /// <summary>
    /// ������ �����մϴ�.
    /// </summary>
    private void UpdateDirection()
    {
        if (!_AllowMovementInput) return;

        // ����� �Է��� �����ϴ� ��쿡�� �����մϴ�.
        if (_InputVelocity.sqrMagnitude > 0.0f)
        {
            _IsRight = _Velocity.x > 0.0f;

            onDirectionChaged?.Invoke(_IsRight);
        }
    }



    public void OnMovementInput(Vector2 axisValue)
    {
        //_InputVelocity.x = _AllowMovementInput ? axisValue.x : 0.0f;
        _InputVelocity.x = axisValue.x;
    }

    public void OnJumpInput()
    {
        if (!_IsJump && _IsGrounded)
        {
            _IsJump = true;
        }

    }

    public void AllowMovementInput(bool allow)
    {
        _AllowMovementInput = allow;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireCube(transform.position, boxCollider.size);

        // apply skinWidth
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(transform.position, boxCollider.size + (Vector3.one * m_SkinWidth));

        PhysicsExt.DrawGizmoBox(_CheckXDrawGizmoInfo);
        PhysicsExt.DrawGizmoBox(_CheckFloorDrawGizmoInfo);
    }
#endif

}
