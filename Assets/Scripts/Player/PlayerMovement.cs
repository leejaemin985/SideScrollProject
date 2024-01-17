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
    /// 캐릭터 영역을 나타냅니다.
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
        // 입력 값을 속도로 변환합니다.
        _Velocity.x = _InputVelocity.x * m_Speed * Time.fixedDeltaTime;

        // X 축 속도 계산
        CalculateVelocityX();


        // Y 축 속도 계산
        CalculateVelocityY();



        // 계산된 속도로 이동합니다.
        transform.position += _Velocity;
    }

    /// <summary>
    /// X 축 속도를 계산합니다.
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
            // 벽을 감지한 경우 벽까지 이동하며, 벽을 통과하지 않도록 합니다.
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
        
        // 땅을 감지했음을 나타냅니다.
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
    /// 바닥에 닿아있음을 확인합니다.
    /// </summary>
    /// <param name="distanceIfDetected">
    /// 바닥에 닿아있지 않을 경우 바닥을 감지한다면 skinWidth 를 제외한 캐릭터 하단부터 바닥까지의 거리를 반환합니다.
    /// -1 이 아닌 값을 반환합니다.</param>
    /// <returns></returns>
    private bool IsGrounded(out float distanceIfDetected)
    {
        // 기본 검사 길이를 계산합니다.
        float defaultLength = boxCollider.size.y + m_SkinWidth;

        // Y 속력을 얻습니다.
        float yVelocityLength = (Mathf.Abs(_Velocity.y) + m_SkinWidth);


        // 바닥 검사를 위하여 밑으로 박스를 쏠 길이입니다.
        float checkLength = (yVelocityLength > defaultLength) ?
            yVelocityLength : defaultLength;

        // 아래를 향해 BoxCast 를 진행합니다.
        RaycastHit hitResult;
        bool isHit = CheckCollision(
            transform.position,
            Vector3.down,
            checkLength,
            out hitResult,
            out _CheckFloorDrawGizmoInfo);

        distanceIfDetected = -1;

        // 바닥을 감지한 경우
        if (isHit)
        {
            // skinWidth 가 적용된 캐릭터 하단 Y 위치를 계산합니다.
            float characterBottomYPos = (transform.position.y + boxCollider.center.y) -
                (boxCollider.size.y * 0.5f) - m_SkinWidth;

            // 감지된 Y 위치를 얻습니다.
            float detectedFloorYPos = hitResult.point.y;

            // 바닥까지의 거리를 반환합니다.
            //distanceIfDetected = hitResult.distance;
            distanceIfDetected = Mathf.Abs(characterBottomYPos - detectedFloorYPos);

            // 감지된 바닥의 높이가 캐릭터 발 위치보다 더 낮은 위치인 경우 바닥에 닿아있지 않습니다.
            // 그렇지 않은 경우 바닥에 닿아있음.
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
        // 박스의 절반 크기
        Vector3 halfSize = boxCollider.size * 0.5f;

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
