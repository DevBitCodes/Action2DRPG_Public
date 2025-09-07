using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Header("Stats"), Space(5)]
    [SerializeField] private int maxHealth = 1;

    [Header("Components"), Space(5)]
    [SerializeField] private Animator bodyAnimator;

    private int curHealth;
    private bool isDead;

    public bool IsDead => isDead;

    private void Start()
    {
        curHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        curHealth = Mathf.Max(0, curHealth - damage);

        if (curHealth == 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        bodyAnimator.SetTrigger("Die");
        Destroy(gameObject, 2.0f);
    }
}
