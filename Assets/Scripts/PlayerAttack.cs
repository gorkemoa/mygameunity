using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;     // oyuncunun saldırı mesafesi
    public int attackDamage = 25;      // vereceği hasar

    void Update()
    {
        // Space tuşuna basınca saldır
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Etrafındaki tüm colliderları bulur
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var hit in hitColliders)
        {
            // Enemy tag'i olanlara hasar ver
            if (hit.CompareTag("Enemy"))
            {
                // EnemyHealth scriptini bul
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    Debug.Log("Enemy'e damage verildi");
                }
            }
        }
    }

    // Scene'de saldırı alanını görmek için
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
