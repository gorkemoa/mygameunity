using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject enemyPrefab;   // Ayı prefab'ı
    public Transform player;         // Player referansı
    public float spawnRadius = 15f;  // Player'ın etrafında hangi uzaklığa spawn edilsin
    public float spawnInterval = 2f; // Kaç saniyede bir spawn
    public int maxEnemies = 30;      // Sahnede aynı anda maksimum ayı sayısı

    [Header("Zorluk")]
    public float difficultyInterval = 30f; // Kaç saniyede bir zorlaşsın
    public float minSpawnInterval = 0.7f;  // En hızlı spawn süresi
    public int enemiesPerLevel = 5;        // Her levelde ekstra max düşman

    private float _spawnTimer = 0f;
    private float _difficultyTimer = 0f;
    private readonly List<GameObject> _enemies = new();

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        // Ölmüş ayıları listeden temizle
        _enemies.RemoveAll(e => e == null);

        // ✅ Spawn zamanı kontrolü
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnInterval && _enemies.Count < maxEnemies)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }

        // ✅ Zorluk arttır
        _difficultyTimer += Time.deltaTime;
        if (_difficultyTimer >= difficultyInterval)
        {
            _difficultyTimer = 0f;
            if (spawnInterval > minSpawnInterval)
                spawnInterval -= 0.2f; // biraz hızlansın

            maxEnemies += enemiesPerLevel;   // sahnedeki limit artsın
            Debug.Log($"Zorluk arttı! Yeni spawnInterval: {spawnInterval}, maxEnemies: {maxEnemies}");
        }
    }

    private void SpawnEnemy()
    {
        // Player'ın etrafında rastgele bir noktaya spawn
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(
            player.position.x + circle.x,
            0.5f, // ayının yerden yüksekliği
            player.position.z + circle.y
        );

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        _enemies.Add(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnRadius);
    }
}
