using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Stats")]
    [Space(5)]
    [SerializeField] private int resistence = 1;

    [Header("Effects")]
    [Space(5)]
    [SerializeField] private GameObject descructionEffect;

    public void TakeDamage(int damage)
    {
        resistence = Mathf.Max(0, resistence - damage);

        if (resistence == 0)
        {
            Instantiate(descructionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.1f);
        }
    }
}
