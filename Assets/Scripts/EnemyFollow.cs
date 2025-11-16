using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 3f;
    public float attackRange = 1.3f;
    public float visionRange = 10f;

    [Header("Saldırı Ayarları")]
    public float attackCooldown = 1f;
    public int damage = 1;

    [Header("Wander Ayarları")]
    public float wanderRadius = 5f;
    public float wanderSpeed = 1.5f;
    public float wanderChangeInterval = 3f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float lastAttackTime = -999f;

    private Vector3 wanderCenter;
    private Vector3 wanderTarget;
    private float lastWanderChangeTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("EnemyFollow: 'Player' tag'li obje bulunamadı!");
        }

        wanderCenter = transform.position;
        ChooseNewWanderTarget();
    }

    void Update()
    {
        if (player == null)
        {
            Wander();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        bool canChasePlayer = !SafeZone.IsPlayerInSafeZone && distance <= visionRange;

        if (!canChasePlayer)
        {
            Wander();
            return;
        }

        if (distance > attackRange)
        {
            FollowPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    // --- WANDER ---
    void Wander()
    {
        float distToTarget = Vector3.Distance(transform.position, wanderTarget);
        if (distToTarget < 0.3f || Time.time - lastWanderChangeTime > wanderChangeInterval)
        {
            ChooseNewWanderTarget();
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        direction.y = 0f;

        Vector3 move = direction * wanderSpeed * Time.deltaTime;
        MoveIfNotInSafeZone(move);

        if (direction != Vector3.zero)
        {
            Vector3 lookPos = new Vector3(
                transform.position.x + direction.x,
                transform.position.y,
                transform.position.z + direction.z
            );
            transform.LookAt(lookPos);
        }
    }

    void ChooseNewWanderTarget()
    {
        Vector2 circle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(
            wanderCenter.x + circle.x,
            transform.position.y,
            wanderCenter.z + circle.y
        );

        lastWanderChangeTime = Time.time;
    }

    // --- PLAYER TAKİBİ ---
    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        Vector3 move = direction * moveSpeed * Time.deltaTime;
        MoveIfNotInSafeZone(move);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    // --- SALDIRI ---
    void TryAttack()
    {
        if (playerHealth == null) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            playerHealth.TakeDamage(damage);
        }
    }

    // --- GÖRÜNMEZ DUVAR BURADA ---
    private void MoveIfNotInSafeZone(Vector3 move)
    {
        Vector3 newPos = transform.position + move;

        // Eğer bu adım SafeZone'un collider'ının içine sokuyorsa, hareket ETME
        if (SafeZone.IsPointInside(newPos))
        {
            return;
        }

        transform.position = newPos;
    }
}
