using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    public override void Attack()
    {
        base.Attack();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.right + Vector3.up * 0.5f, range);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Destructible destructible))
            {
                destructible.TakeDamage(damage);
            }

            if (hit.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
