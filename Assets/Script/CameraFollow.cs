using UnityEngine;

/// <summary>
/// Kamera 2D yang mengikuti target (Player). Pasang di Main Camera.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Referensi")]
    [SerializeField] private Transform target;

    [Header("Pengaturan")]
    [Tooltip("Makin besar makin cepat kamera menyusul. 0 = kamera langsung nempel tanpa delay.")]
    [SerializeField] private float smoothSpeed = 8f;

    private float cameraZ;

    private void Start()
    {
        // Simpan posisi Z kamera (biasanya -10) supaya tidak berubah
        cameraZ = transform.position.z;
    }

    // LateUpdate dipakai supaya kamera bergerak SETELAH player selesai bergerak
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 goal = new Vector3(target.position.x, target.position.y, cameraZ);

        transform.position = smoothSpeed <= 0f
            ? goal
            : Vector3.Lerp(transform.position, goal, smoothSpeed * Time.deltaTime);
    }
}
