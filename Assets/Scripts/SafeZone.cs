using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public static bool IsPlayerInSafeZone = false;

    // 👉 Düşmanlar için yasak alan bilgisi
    public static Vector3 Center;
    public static float Radius;

    void Awake()
    {
        Center = transform.position;

        // Tercihen SphereCollider kullanırsan tam yuvarlak alan alırız
        SphereCollider sphere = GetComponent<SphereCollider>();
        if (sphere != null)
        {
            float scale = Mathf.Max(transform.localScale.x, transform.localScale.z);
            Radius = sphere.radius * scale;
        }
        else
        {
            // SphereCollider yoksa yaklaşık bir değer ver
            Radius = 5f;
            Debug.LogWarning("SafeZone: SphereCollider bulunamadı, Radius 5 olarak ayarlandı. İstersen düzelt.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInSafeZone = true;
            Debug.Log(">>> PLAYER SAFE ZONE İÇİNDE");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInSafeZone = false;
            Debug.Log(">>> PLAYER SAFE ZONE DIŞINDA");
        }
    }

    // 👉 Dışarıdan pozisyon verip, bu nokta SafeZone içinde mi diye sorabilelim
    public static bool IsPointInside(Vector3 pos)
    {
        if (Radius <= 0) return false;

        Vector2 a = new Vector2(pos.x, pos.z);
        Vector2 b = new Vector2(Center.x, Center.z);
        return Vector2.Distance(a, b) <= Radius;
    }
}
