using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;                      // daftar jenis sampah
    public Vector2 mapSize = new Vector2(10f, 10f);        // setengah lebar dan setengah tinggi area spawn
    public int maxTrash = 40;                              // batas jumlah sampah di map
    public float spawnInterval = 0.5f;                     // jeda spawn (detik)

    private List<GameObject> activeTrash = new List<GameObject>();
    private float timer;

    void Start()
    {
        // isi map dengan sampah di awal game
        for (int i = 0; i < maxTrash; i++)
        {
            SpawnOne();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        // sampah yang sudah dimakan jadi kosong (null), buang dari daftar
        activeTrash.RemoveAll(item => item == null);

        if (activeTrash.Count < maxTrash)
        {
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        if (trashPrefabs.Length == 0) return;

        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        Vector2 pos = new Vector2(
            Random.Range(-mapSize.x, mapSize.x),
            Random.Range(-mapSize.y, mapSize.y));

        GameObject trash = Instantiate(prefab, pos, Quaternion.identity);
        activeTrash.Add(trash);
    }

    // kotak hijau di Scene view supaya batas area spawn kelihatan
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(mapSize.x * 2f, mapSize.y * 2f, 0f));
    }
}