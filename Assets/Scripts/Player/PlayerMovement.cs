using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// 충돌체 감지를 위해 오브젝트 영역에 추가되는 두께
    /// </summary>
    [Header("# 오브젝트 패딩")]
    public float m_SkinWidth;

    [Header("# 캐릭터 이동 관련")]
    /// <summary>
    /// 이동 속력입니다.
    /// </summary>
    public float m_Speed;

    /// <summary>
    /// 점프 힘입니다.
    /// </summary>
    public float m_JumpPower;

    /// <summary>
    /// 중력에 적용될 승수입니다.
    /// </summary>
    public float m_GravityMultiplier;

    [Header("# 충돌 관련")]
    /// <summary>
    /// 이동을 블록시킬 레이어를 나타냅니다.
    /// </summary>
    public LayerMask m_BlockMovementLayer;


    /// <summary>
    /// 입력 축 값입니다.
    /// </summary>
    private Vector3 _InputVelocity;

    /// <summary>
    /// 현재 적용된 속도
    /// </summary>
    private Vector3 _Velocity;

    /// <summary>
    /// 캐릭터가 땅에 닿아있음을 나타냅니다.
    /// </summary>
    private bool _IsGrounded;

    /// <summary>
    /// 점프 입력이 들어왔음을 나타냅니다.
    /// </summary>
    private bool _IsJump;

    /// <summary>
    /// 오른쪽을 바라	보고 있음을 나타냅니다.
    /// </summary>
    private bool _IsRight;

    /// <summary>
    /// 이동 입력을 허용합니다.
    /// </summary>
    private bool _AllowMovementInput = true;

    /// <summary>
    /// 캐릭터 영역을 나타냅니다.
    /// </summary>
    private BoxCollider _BoxCollider;

    public BoxCollider boxCollider => _BoxCollider ??
        (_BoxCollider = GetComponent<BoxCollider>());

    /// <summary>
    /// 속도에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public Vector3 velocity => _Velocity;

    /// <summary>
    /// 오른쪽을 바라보고 있음을 나타냅니다.
    /// </summary>
    public bool isRight => _IsRight;

    /// <summary>
    /// 땅에 닿아있음을 나타냅니다.
    /// </summary>
    public bool isGrounded => _IsGrounded;


    /// <summary>
    /// 방향이 변경되었을 경우 발생하는 이벤트
    /// isRight: 오른쪽 방향일 경우 True가 전달됩니다.
    /// </summary>
    public event System.Action<bool> onDirectionChaged;


    #region DEBUG
    private DrawGizmoCubeInfo _CheckXDrawGizmoInfo;
    private DrawGizmoCubeInfo _CheckFloorDrawGizmoInfo;
    #endregion


    private void FixedUpdate()
    {
        // 평면 발사 시작 위치
        Vector3 origin = transform.position + boxCollider.center;

        // 캐릭터 절반 크기
        Vector3 halfSize = boxCollider.size * 0.5f;

        // X 축 속도 계산
        CalculateVelocityX(origin, halfSize);

        // Y 축 속도 계산
        CalculateVelocityY(origin, halfSize);

        // 방향을 갱신합니다.
        UpdateDirection();

        // 계산된 속도로 이동합니다.
        transform.position += _Velocity;
    }

    /// <summary>
    /// X 축 속도를 계산합니다.
    /// </summary>
    private void CalculateVelocityX(Vector3 origin, Vector3 halfSize)
    {
        float velocityX = _Velocity.x;

        // 입력 값을 속도로 변환합니다.
        velocityX = (_AllowMovementInput ? _InputVelocity.x : 0.0f) * m_Speed * Time.fixedDeltaTime;

        // X 축 방향으로 쏠 사각형의 절반 크기
        Vector3 halfZYSizeWithSkin = halfSize - (Vector3.one * m_SkinWidth);
        halfZYSizeWithSkin.x = 0.0f;

        Vector3 direction = Vector3.right * Mathf.Sign(_InputVelocity.x);

        // 검사 길이
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

            // 벽을 감지한 경우 벽까지 이동하며, 벽을 통과하지 않도록 합니다.
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

        // 중력 연산
        velocityY -= gravity;

        if (_IsJump)
        {
            _IsJump = false;
            velocityY = m_JumpPower;
        }


        // Y 축 방향으로 쏠 평면의 절반 크기를 계산합니다.
        Vector3 halfXZSizeWithSkin = halfSize - (Vector3.one * m_SkinWidth);
        halfXZSizeWithSkin.y = 0.0f;

        // 방향
        Vector3 direction = Vector3.up * Mathf.Sign(velocityY);

        // 검사 길이
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
            // Boxcast 시작 위치
            start,
            // 발사시킬 Box 의 절반 크기
            halfSize,
            // 발사 방향
            direction,
            // 감지 결과를 반환받을 변수 전달
            out hitResdult,
            // 발사시킬 Box 의 회전값 전달
            Quaternion.identity,
            // 발사 거리 전달
            maxDistance,
            // 감지시킬 레이어 지정
            m_BlockMovementLayer,
            // 감지 방식 전달
            QueryTriggerInteraction.UseGlobal);

        return isHit;
    }



    /// <summary>
    /// 방향을 갱신합니다.
    /// </summary>
    private void UpdateDirection()
    {
        if (!_AllowMovementInput) return;

        // 사용자 입력이 존재하는 경우에만 실행합니다.
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
