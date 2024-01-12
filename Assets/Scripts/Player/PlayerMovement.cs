using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("# 오브젝트 패딩")]
    public float m_SkinWidth;

    [Header("# 캐릭터 이동 속력")]
    public float speed;


    [Header("# 충돌 관련")]
    public LayerMask m_BlockMovementLayer;



    private Vector3 _InputVelocity;

    private Vector3 _Velocity;


    private BoxCollider _BoxCollider;

    private void Awake()
    {
        _BoxCollider = GetComponent<BoxCollider>();
    }



    private void FixedUpdate()
    {

        CalculateVelocity();

        transform.position += _Velocity * Time.fixedDeltaTime;
    }


    private void CalculateVelocity()
    {
        _Velocity = _InputVelocity * speed;
    }


    
    private bool CheckXAxis()
    {
        RaycastHit hitInfo;

        Physics.BoxCast(
            _BoxCollider.transform.position + _BoxCollider.center,
            _BoxCollider.size * 0.5f,
            _InputVelocity,
            out hitInfo,
            Quaternion.identity,
            (_Velocity * Time.fixedDeltaTime).magnitude,
            m_BlockMovementLayer);


        return false;
    }


    public void OnMovementInput(Vector2 axisValue)
    {
        _InputVelocity.x = axisValue.x;
    }


    public void OnJumpInput()
    {
        Debug.Log("Jump");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(transform.position, _BoxCollider.size);


        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _BoxCollider.size + (Vector3.one * m_SkinWidth));

    }

#endif


}
