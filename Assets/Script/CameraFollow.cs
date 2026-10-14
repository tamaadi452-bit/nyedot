using UnityEngine;

/// <summary>
/// Kamera 2D yang mengikuti target (Player). Pasang di Main Camera.
/// Zoom out per "lompatan": saat player memenuhi layar sebesar maxScreenFill,
/// kamera zoom out sekali (dikali zoomOutFactor). Di antara lompatan, zoom diam.
/// Nggak ada batas maksimal zoom out.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // ============================================================
    //  REFERENSI
    // ============================================================
    [Header("Referensi")]
    [Tooltip("Drag object Player ke sini.")]
    [SerializeField] private Transform target;

    // ============================================================
    //  FOLLOW (ngikutin posisi player)
    // ============================================================
    [Header("Follow")]
    [Tooltip("Makin besar makin cepat kamera menyusul. 0 = kamera langsung nempel tanpa delay.")]
    [SerializeField] private float smoothSpeed = 8f;

    // ============================================================
    //  ZOOM (ATUR DI SINI)
    // ============================================================
    [Header("Zoom - Ukuran Awal")]
    [Tooltip("Orthographic size awal saat game mulai.")]
    [Min(0.1f)]
    [SerializeField] private float baseSize = 5f;

    [Header("Zoom - Pemicu & Lompatan")]
    [Tooltip("Pas player makan segini dari tinggi layar, kamera zoom out. 0.5 = 50%.")]
    [Range(0.05f, 0.95f)]
    [SerializeField] private float maxScreenFill = 0.5f;

    [Tooltip("Tiap zoom out, size dikali angka ini. 1.5 = size jadi 1.5x lebih besar. Harus > 1.")]
    [Min(1.01f)]
    [SerializeField] private float zoomOutFactor = 1.5f;

    [Header("Zoom - Transisi")]
    [Tooltip("Kecepatan transisi zoom. Makin besar makin cepat. Kecil = halus/pelan.")]
    [Range(0.5f, 20f)]
    [SerializeField] private float zoomSpeed = 3f;

    // ============================================================
    //  INTERNAL (nggak perlu diutak-atik)
    // ============================================================
    private float cameraZ;
    private Camera cam;
    private SpriteRenderer targetSprite;
    private float zoomTarget; // size tujuan, cuma naik pas ada lompatan

    private void Start()
    {
        // Simpan posisi Z kamera (biasanya -10) supaya tidak berubah
        cameraZ = transform.position.z;
        cam = GetComponent<Camera>();

        if (target != null)
            targetSprite = target.GetComponentInChildren<SpriteRenderer>();

        zoomTarget = baseSize;
        cam.orthographicSize = baseSize;
    }

    // LateUpdate dipakai supaya kamera bergerak SETELAH player selesai bergerak
    private void LateUpdate()
    {
        if (target == null) return;

        FollowTarget();
        UpdateZoom();
    }

    private void FollowTarget()
    {
        Vector3 goal = new Vector3(target.position.x, target.position.y, cameraZ);

        transform.position = smoothSpeed <= 0f
            ? goal
            : Vector3.Lerp(transform.position, goal, smoothSpeed * Time.deltaTime);
    }

    private void UpdateZoom()
    {
        // Tinggi player di world (udah termasuk scale).
        // Kalau nggak nemu SpriteRenderer, fallback ke localScale.
        float playerHeight = targetSprite != null
            ? targetSprite.bounds.size.y
            : target.localScale.y;

        // Selama fill (terhadap zoomTarget) udah mentok batas, lompat zoom out.
        // Pakai while biar kalau player tiba-tiba gede banget, lompatannya nyusul semua.
        while (playerHeight / (2f * zoomTarget) >= maxScreenFill)
        {
            zoomTarget *= zoomOutFactor;
        }

        // Transisi halus ke size tujuan
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomSpeed * Time.deltaTime);
    }
}