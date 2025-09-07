using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
}

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [Space(5)]
    [SerializeField] private float minIdleTime = 2.0f;
    [SerializeField] private float maxIdleTime = 5.0f;
    [SerializeField] private float minMoveTime = 2.0f;
    [SerializeField] private float maxMoveTime = 5.0f;
    [SerializeField] private float updateDirectionTime = 1.0f;
    [SerializeField] private Transform centerPoint;

    [Header("Obstacles")]
    [Space(5)]
    [SerializeField] private float obstacleRayDetectionRadius = 0.5f;
    [SerializeField] private float obstacleRayDetectionDistance = 1.0f;
    [SerializeField] private LayerMask obstaclesLayer;

    private float stateTimer;
    private float updateDirectionTimer;
    private float idleTime;
    private float moveTime;
    private Vector2 direction;
    private EnemyState enemyState;
    private CharacterMovement characterMovement;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        EnterIdleState();
    }

    private void Update()
    {
        stateTimer += Time.deltaTime;
        updateDirectionTimer += Time.deltaTime;
        HandleState();
    }

    private void EnterIdleState()
    {
        enemyState = EnemyState.Idle;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        stateTimer = 0;
        characterMovement.SetDirection(Vector2.zero);
    }

    private void EnterMoveState()
    {
        enemyState = EnemyState.Move;
        moveTime = Random.Range(minMoveTime, maxMoveTime);
        stateTimer = 0;
        updateDirectionTimer = 0;
        direction = AvoidObstacles(Random.insideUnitCircle.normalized);
        characterMovement.SetDirection(direction);
    }

    private void HandleState()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:

                if (stateTimer >= idleTime)
                {
                    EnterMoveState();
                }

                break;

            case EnemyState.Move:

                if (updateDirectionTimer > updateDirectionTime)
                {
                    updateDirectionTimer = 0;
                    direction = AvoidObstacles(Random.insideUnitCircle.normalized);
                    characterMovement.SetDirection(direction);
                }

                if (stateTimer >= moveTime)
                {
                    EnterIdleState();
                }

                break;
        }
    }

    private Vector2 AvoidObstacles(Vector2 direction)
    {
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

        Vector2[] directions = {
            roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, 45.0f) * roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, -45.0f) * roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, 90.0f) * roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, -90.0f) * roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, 135.0f) * roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, -135.0f) * roundedDirection,
            Quaternion.Euler(0.0f, 0.0f, 180.0f) * roundedDirection,
        };

        Vector2 bestDireciton = Vector2.zero;
        float bestScore = float.MinValue;
        bool foundValidDirection = false;

        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.CircleCast(centerPoint.position, obstacleRayDetectionRadius, dir, obstacleRayDetectionDistance, obstaclesLayer);
            bool isBlocked = hit.collider != null && !hit.collider.isTrigger;

            Debug.DrawLine(centerPoint.position, (Vector2)centerPoint.position + dir * obstacleRayDetectionDistance, isBlocked ? Color.red : Color.green);

            if (!isBlocked)
            {
                foundValidDirection = true;
                float score = (obstacleRayDetectionDistance - (hit.collider != null ? hit.distance : 0.0f));

                if (score > bestScore)
                {
                    bestScore = score;
                    bestDireciton = dir.normalized;
                }
            }            
        }

        if (!foundValidDirection)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                RaycastHit2D hit2 = Physics2D.CircleCast(centerPoint.position, obstacleRayDetectionRadius, randomDirection, obstacleRayDetectionDistance, obstaclesLayer);

                if (hit2.collider == null || hit2.collider.isTrigger)
                {
                    return randomDirection;
                }
            }

            return -direction;
        }

        return bestDireciton;
    }
}
