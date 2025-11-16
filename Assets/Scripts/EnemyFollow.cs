using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 3f;
    public float attackRange = 1.3f;
    public float visionRange = 10f; // Oyuncuyu bu menzilde “fark eder”

    [Header("Saldırı Ayarları")]
    public float attackCooldown = 1f;
    public int damage = 1;

    [Header("Wander Ayarları")]
    public float wanderRadius = 5f;        // Kendi etrafında ne kadar dolaşsın
    public float wanderSpeed = 1.5f;       // Dolaşma hızı
    public float wanderChangeInterval = 3f; // Kaç saniyede bir yeni hedef nokta seçsin

    private Transform player;
    private PlayerHealth playerHealth;
    private float lastAttackTime = -999f;

    // Wander için
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
            Debug.LogError("EnemyFollow: Sahne içinde 'Player' tag'li obje bulunamadı!");
        }

        // İlk wander merkezi → doğduğu yer
        wanderCenter = transform.position;
        ChooseNewWanderTarget();
    }

    void Update()
    {
        if (player == null)
        {
            // Oyuncu yoksa sadece wander
            Wander();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // Oyuncuyu kovalamaya izin var mı?
        // SafeZone içindeyse veya çok uzaktaysa KOVALAMA → sadece wander
        bool canChasePlayer = !SafeZone.IsPlayerInSafeZone && distance <= visionRange;

        if (!canChasePlayer)
        {
            Wander();
            return;
        }

        // Bu noktadan sonrası: Oyuncu görüşte ve safezonede değil → chase / attack

        if (distance > attackRange)
        {
            FollowPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    // --- WANDER (boş boş gezinme davranışı) ---
    void Wander()
    {
        // Hedefe çok yaklaştıysa veya süre dolduysa yeni hedef seç
        float distToTarget = Vector3.Distance(transform.position, wanderTarget);
        if (distToTarget < 0.3f || Time.time - lastWanderChangeTime > wanderChangeInterval)
        {
            ChooseNewWanderTarget();
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * wanderSpeed * Time.deltaTime;

        // Gideceği yöne dönsün
        if (direction != Vector3.zero)
        {
            Vector3 lookPos = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.z);
            transform.LookAt(lookPos);
        }
    }

    void ChooseNewWanderTarget()
    {
        // wanderCenter etrafında rastgele nokta
        Vector2 circle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(
            wanderCenter.x + circle.x,
            transform.position.y, // yüksekliği sabit tut
            wanderCenter.z + circle.y
        );

        lastWanderChangeTime = Time.time;
    }

    // --- PLAYER TAKİBİ ---
    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        move.y = 0f;

        transform.position += move;
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
}
