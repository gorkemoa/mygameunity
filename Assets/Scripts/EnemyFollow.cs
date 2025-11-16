using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;

    void Start()
    {
        // Eğer inspector'dan atamadıysak Player'ı otomatik bul
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f; // Yukarı aşağı oynamasın
        dir = dir.normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

        // Gittiği yöne dönsün
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                10f * Time.deltaTime
            );
        }
    }
}
