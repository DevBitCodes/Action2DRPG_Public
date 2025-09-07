using System.Collections;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [Header("Stats")]
    [Space(5)]
    [SerializeField] private int damage = 1;
    [SerializeField] private float cooldown = 1.0f;
    [SerializeField] private float range = 1.0f;

    [Header("Components")]
    [Space(5)]
    [SerializeField] private Animator combatAnimator;

    private bool isAttacking;

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            combatAnimator.SetTrigger("Attack");

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.right + Vector3.up * 0.5f, range);

            foreach (Collider2D hit in hits)
            {
                if (GetComponent<PlayerController>() != null)
                {
                    if (hit.TryGetComponent(out Destructible destructible))
                    {
                        destructible.TakeDamage(damage);
                    }
                }
            }

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right + Vector3.up * 0.5f, range);
    }
}
