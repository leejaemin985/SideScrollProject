using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyCharacter : MonoBehaviour
{
    [Header("# �� �ڵ�")]
    public string m_EnemyCode;

    private Collider _EnemyCollider;
    public Collider enemyCollider => _EnemyCollider ??
        (_EnemyCollider = GetComponent<Collider>());


    public void ApplyDamage(float damage)
    {

    }
}
