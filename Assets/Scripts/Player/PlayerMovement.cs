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
    /// ĳ���� ������ ��Ÿ���ϴ�.
    /// </summary>
    private BoxCollider _BoxCollider;

    public BoxCollider boxCollider => _BoxCollider ??
        (_BoxCollider = GetComponent<BoxCollider>());


    #region DEBUG
    private DrawGizmoCubeInfo _CheckXDrawGizmoInfo;
    private DrawGizmoCubeInfo _CheckFloorDrawGizmoInfo;
    #endregion


    private void FixedUpdate()
    {
        // �Է� ���� �ӵ��� ��ȯ�մϴ�.
        _Velocity.x = _InputVelocity.x * m_Speed * Time.fixedDeltaTime;

        // X �� �ӵ� ���
        CalculateVelocityX();


        // Y �� �ӵ� ���
        CalculateVelocityY();



        // ���� �ӵ��� �̵��մϴ�.
        transform.position += _Velocity;
    }

    /// <summary>
    /// X �� �ӵ��� ����մϴ�.
    /// </summary>
    private void CalculateVelocityX()
    {
        Vector3 direction = _InputVelocity.normalized;
        Vector3 start = transform.position + boxCollider.center + (direction * m_SkinWidth);
        float maxDistance = Mathf.Abs(_Velocity.x);

        RaycastHit hitResult;
        bool isHit = CheckCollision(
            start,
            direction,
            maxDistance,
            out hitResult,
            out _CheckXDrawGizmoInfo);

        if (isHit)
        {
            // ���� ������ ��� ������ �̵��ϸ�, ���� ������� �ʵ��� �մϴ�.
            int sign = direction.x > 0.0f ? 1 : -1;
            _Velocity.x = hitResult.distance * sign;
        }
    }

    private void CalculateVelocityY()
    {
        float gravity = Mathf.Abs(Physics.gravity.y) * m_GravityMultiplier * Time.fixedDeltaTime;
        float velocityY = _Velocity.y;

        float addYSpeed = -gravity;
        float floorDistance;
        _IsGrounded = IsGrounded(out floorDistance);
        
        // ���� ���������� ��Ÿ���ϴ�.
        bool detectFloor = floorDistance >= 0.0f;

        if (_IsJump)
        {
            addYSpeed = m_JumpPower * Time.fixedDeltaTime;
            _IsGrounded = false;
            _IsJump = false;
        }

        if (_IsGrounded)
        {
            velocityY = 0.0f;
        }
        else
        {
            if (detectFloor && velocityY < 0)
            {
                if (Mathf.Abs(addYSpeed) + Mathf.Abs(velocityY) > floorDistance)
                {
                    velocityY = 0.0f;
                    addYSpeed = -floorDistance;
                }
            }

            velocityY += addYSpeed;
        }

        _Velocity.y = velocityY;
    }


    /// <summary>
    /// �ٴڿ� ��������� Ȯ���մϴ�.
    /// </summary>
    /// <param name="distanceIfDetected">
    /// �ٴڿ� ������� ���� ��� �ٴ��� �����Ѵٸ� skinWidth �� ������ ĳ���� �ϴܺ��� �ٴڱ����� �Ÿ��� ��ȯ�մϴ�.
    /// -1 �� �ƴ� ���� ��ȯ�մϴ�.</param>
    /// <returns></returns>
    private bool IsGrounded(out float distanceIfDetected)
    {
        // �⺻ �˻� ���̸� ����մϴ�.
        float defaultLength = boxCollider.size.y + m_SkinWidth;

        // Y �ӷ��� ����ϴ�.
        float yVelocityLength = (Mathf.Abs(_Velocity.y) + m_SkinWidth);


        // �ٴ� �˻縦 ���Ͽ� ������ �ڽ��� �� �����Դϴ�.
        float checkLength = (yVelocityLength > defaultLength) ?
            yVelocityLength : defaultLength;

        // �Ʒ��� ���� BoxCast �� �����մϴ�.
        RaycastHit hitResult;
        bool isHit = CheckCollision(
            transform.position,
            Vector3.down,
            checkLength,
            out hitResult,
            out _CheckFloorDrawGizmoInfo);

        distanceIfDetected = -1;

        // �ٴ��� ������ ���
        if (isHit)
        {
            // skinWidth �� ����� ĳ���� �ϴ� Y ��ġ�� ����մϴ�.
            float characterBottomYPos = (transform.position.y + boxCollider.center.y) -
                (boxCollider.size.y * 0.5f) - m_SkinWidth;

            // ������ Y ��ġ�� ����ϴ�.
            float detectedFloorYPos = hitResult.point.y;

            // �ٴڱ����� �Ÿ��� ��ȯ�մϴ�.
            //distanceIfDetected = hitResult.distance;
            distanceIfDetected = Mathf.Abs(characterBottomYPos - detectedFloorYPos);

            // ������ �ٴ��� ���̰� ĳ���� �� ��ġ���� �� ���� ��ġ�� ��� �ٴڿ� ������� �ʽ��ϴ�.
            // �׷��� ���� ��� �ٴڿ� �������.
            return (detectedFloorYPos >= characterBottomYPos - 0.001f);
            //print($"detectedFloorYPos:{detectedFloorYPos}\tcharacterBottomYPos:{characterBottomYPos}");
            //return true;
        }

        return false;
    }


    private bool CheckCollision(
        Vector3 start,
        Vector3 direction,
        float maxDistance,
        out RaycastHit hitResdult,
        out DrawGizmoCubeInfo drawGizmoInfo)
    {
        // �ڽ��� ���� ũ��
        Vector3 halfSize = boxCollider.size * 0.5f;

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

    public void OnMovementInput(Vector2 axisValue)
    {
        _InputVelocity.x = axisValue.x;
    }

    public void OnJumpInput()
    {
        if (!_IsJump && _IsGrounded)
        {
            _IsJump = true;
        }

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
    }
#endif

}
