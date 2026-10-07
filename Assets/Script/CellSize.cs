using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Menyimpan poin dan mengatur ukuran karakter. Pasang di Player DAN Bot.
/// Ukuran (scale) otomatis mengikuti poin: makin banyak poin, makin besar.
/// </summary>
public class CellSize : MonoBehaviour
{
    [Header("Pengaturan")]
    [SerializeField] private float startPoints = 10f;

    [Tooltip("Jumlah poin yang membuat scale = 1. Dengan 10, karakter 10 poin berukuran scale 1.")]
    [SerializeField] private float pointsForUnitSize = 10f;

    /// <summary>Daftar semua karakter yang sedang aktif (player + bot).</summary>
    public static readonly List<CellSize> All = new List<CellSize>();

    public float Points { get; private set; }

    private void Awake()
    {
        Points = startPoints;
        ApplyScale();
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    /// <summary>Tambah poin (misalnya saat makan makanan atau karakter lain).</summary>
    public void AddPoints(float amount)
    {
        Points += amount;
        ApplyScale();
    }

    private void ApplyScale()
    {
        // Luas lingkaran sebanding dengan poin, jadi diameter = akar dari poin
        float size = Mathf.Sqrt(Points / pointsForUnitSize);
        transform.localScale = new Vector3(size, size, 1f);
    }

    // Supaya perubahan Start Points di Inspector langsung terlihat di Scene
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            float size = Mathf.Sqrt(Mathf.Max(startPoints, 0.01f) / Mathf.Max(pointsForUnitSize, 0.01f));
            transform.localScale = new Vector3(size, size, 1f);
        }
    }
}
