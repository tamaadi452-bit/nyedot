using UnityEngine;

public class PlayerEat : MonoBehaviour
{
    public string foodTag = "food";   // harus sama persis dengan tag di prefab sampah
    public int totalScore;

    // dipanggil otomatis saat Player menyentuh collider yang Is Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(foodTag)) return;

        TrashItem trash = other.GetComponent<TrashItem>();
        if (trash == null) return;

        // Player membesar
        GetComponent<CellSize>().AddPoints(trash.growAmount);
        totalScore += trash.score;

        // sampah hilang, spawner akan mengisi lagi
        Destroy(other.gameObject);
    }
}