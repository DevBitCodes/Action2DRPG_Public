using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Stats")]
    [Space(5)]
    [SerializeField] private float speed = 5.0f;

    [Header("Components")]
    [Space(5)]
    [SerializeField] private Animator bodyAnimator;

    private Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Animate();
        Flip();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Animate()
    {
        bodyAnimator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private void Flip()
    {
        if (direction.x == 0.0f) return;
        float yRotation = direction.x < 0.0f ? 180.0f : 0.0f;
        transform.eulerAngles = new Vector2(transform.rotation.x, yRotation);
    }

    public void Move()
    {
        if (direction != Vector2.zero)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
}
