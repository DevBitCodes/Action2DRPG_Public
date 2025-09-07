using System.Collections;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [Header("Stats")]
    [Space(5)]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float range = 1.0f;
    [SerializeField] private float cooldown = 1.0f;

    [Header("Components")]
    [Space(5)]
    [SerializeField] private Animator combatAnimator;

    private bool isAttacking;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public virtual void Attack()
    {
        if (health.IsDead) return;
        
        if (!isAttacking)
        {
            isAttacking = true;
            combatAnimator.SetTrigger("Attack");
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (health.IsDead) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right + Vector3.up * 0.5f, range);
    }
}
