using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject enemyPrefab;
    public Transform player;

    [Tooltip("Düşmanların doğacağı sabit noktalar")]
    public Transform[] spawnPoints;

    [Tooltip("Toplam sahnedeki maksimum düşman sayısı")]
    public int maxEnemies = 30;

    [Tooltip("Kaç saniyede bir yeni grup denensin")]
    public float spawnInterval = 3f;

    [Header("Grup Ayarları")]
    [Tooltip("Bir seferde en fazla kaç düşman doğsun")]
    public int groupSize = 3;

    [Tooltip("Grup içindeki düşmanların rastgele dağılma yarıçapı")]
    public float groupSpreadRadius = 2f;

    [Header("Safe Zone Ayarları")]
    [Tooltip("SafeZone’a bu kadar yakın noktalara spawn etme")]
    public float safeCheckRadius = 1.5f;

    private float _spawnTimer = 0f;
    private readonly List<GameObject> _enemies = new();

    void Update()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: enemyPrefab atanmadı!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemySpawner: Hiç spawn point atanmadı!");
            return;
        }

        // Ölmüş düşmanları listeden temizle
        _enemies.RemoveAll(e => e == null);

        // Zaten maxEnemies veya üstündeysek yeni spawn yok
        if (_enemies.Count >= maxEnemies) return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnGroup();
        }
    }

    private void SpawnGroup()
    {
        // Rastgele bir spawn point seç
        Transform selectedPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        int spawnedThisGroup = 0;

        for (int i = 0; i < groupSize; i++)
        {
            if (_enemies.Count >= maxEnemies)
                break;

            // Seçilen noktanın etrafında hafif rastgele pozisyon
            Vector2 offset2D = Random.insideUnitCircle * groupSpreadRadius;
            Vector3 spawnPos = new Vector3(
                selectedPoint.position.x + offset2D.x,
                selectedPoint.position.y,
                selectedPoint.position.z + offset2D.y
            );

            // SafeZone'a çok yakınsa bu elemanı atla
            if (IsPositionInSafeZone(spawnPos))
                continue;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            _enemies.Add(enemy);
            spawnedThisGroup++;
        }

        if (spawnedThisGroup > 0)
        {
            Debug.Log($"EnemySpawner: {spawnedThisGroup} düşman spawn edildi. Toplam: {_enemies.Count}");
        }
    }

    private bool IsPositionInSafeZone(Vector3 pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos, safeCheckRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("SafeZone"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        // Spawn point’leri sahnede görebilmek için
        Gizmos.color = Color.yellow;
        foreach (var sp in spawnPoints)
        {
            if (sp == null) continue;
            Gizmos.DrawWireSphere(sp.position, groupSpreadRadius);
        }
    }
}
