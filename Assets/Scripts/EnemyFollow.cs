using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 3f;

    [Header("Takip Mesafeleri")]
    public float detectionRange = 8f;   
    public float stopChaseRange = 12f;

    [Header("Vuruş Ayarları")]
    public int damage = 10;                 // Kaç damage vursun
    public float attackInterval = 1f;       // Kaç saniyede bir vursun
    private float lastAttackTime = 0f;

    [Header("Wander (Dolanma)")]
    public float wanderRadius = 5f;
    public float wanderChangeInterval = 4f;

    [Header("Referanslar")]
    public PlayerHealth playerHealth;       // Inspector'dan da atayabilirsin

    private Transform player;
    private Vector3 wanderCenter;
    private Vector3 wanderTarget;
    private float wanderTimer;
    private bool isChasing = false;

    void Start()
    {
        // Eğer Inspector'dan atamadıysan otomatik bulmaya çalış
        if (playerHealth == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                // PlayerHealth player'ın child'ında bile olsa bul
                playerHealth = playerObj.GetComponentInChildren<PlayerHealth>();
                if (playerHealth == null)
                {
                    Debug.LogWarning("EnemyFollow: PlayerHealth bulunamadı! Lütfen Inspector'dan atayın.");
                }
            }
        }

        if (playerHealth != null)
        {
            player = playerHealth.transform;
        }

        // Wander başlangıcı
        wanderCenter = transform.position;
        PickNewWanderTarget();
    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // Algılama menziline girdiyse kovala
        if (distToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            // Çok uzaklaşırsa kovalamayı bırak
            if (distToPlayer > stopChaseRange)
            {
                isChasing = false;
                wanderCenter = transform.position;
                PickNewWanderTarget();
            }
            else
            {
                ChasePlayer();
                TryAttack(distToPlayer);   // Vuruş burada
                return;
            }
        }

        // Kovalamıyorsa dolan
        Wander();
    }

    void TryAttack(float distToPlayer)
    {
        if (playerHealth == null)
        {
            // Sadece uyarı ver, her frame spam olmasın diye nadir log istiyorsan burayı yoruma alabilirsin.
            // Debug.LogWarning("EnemyFollow: PlayerHealth yok, damage veremiyorum.");
            return;
        }

        // Yakın mesafede mi? (vuruş mesafesi)
        if (distToPlayer > 1.3f) return;

        // SAFE ZONE içindeyse vurma
        if (SafeZone.IsPlayerInSafeZone)
        {
            // Debug.Log("Enemy: Player safe zone'da, vurmadım.");
            return;
        }

        // Attack interval kontrolü
        if (Time.time - lastAttackTime < attackInterval) return;

        // Damage ver
        playerHealth.TakeDamage(damage);
        lastAttackTime = Time.time;

        Debug.Log("Enemy vurdu! Damage: " + damage + " | Player HP: " + playerHealth.CurrentHealth);
    }

    void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 10f * Time.deltaTime);
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        }
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        Vector3 flatTarget = new Vector3(wanderTarget.x, transform.position.y, wanderTarget.z);
        Vector3 dir = flatTarget - transform.position;

        if (dir.magnitude < 0.3f || wanderTimer >= wanderChangeInterval)
        {
            PickNewWanderTarget();
        }
        else
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5f * Time.deltaTime);
            transform.position += dir.normalized * (moveSpeed * 0.5f) * Time.deltaTime;
        }
    }

    void PickNewWanderTarget()
    {
        wanderTimer = 0f;
        Vector2 circle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = wanderCenter + new Vector3(circle.x, 0f, circle.y);
    }
}
