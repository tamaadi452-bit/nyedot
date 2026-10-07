using UnityEngine;

/// <summary>
/// Membuat karakter bisa memakan karakter lain yang lebih kecil.
/// Pasang di Player DAN Bot. Collider keduanya harus "Is Trigger".
/// </summary>
[RequireComponent(typeof(CellSize))]
public class CellEater : MonoBehaviour
{
    [Tooltip("Harus lebih besar sekian kali lipat baru bisa memakan. Samakan dengan Size Margin di Bot AI.")]
    [SerializeField] private float sizeMargin = 1.1f;

    [Tooltip("Berapa persen poin korban yang didapat (1 = semua)")]
    [SerializeField, Range(0f, 1f)] private float gainRatio = 1f;

    private CellSize me;

    private void Awake()
    {
        me = GetComponent<CellSize>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (me.IsDead) return;

        CellSize prey = other.GetComponent<CellSize>();
        if (prey == null || prey == me || prey.IsDead) return;

        // Harus cukup lebih besar
        if (me.Points <= prey.Points * sizeMargin) return;

        // Pusat korban harus sudah masuk ke dalam badan pemakan (seperti Agar.io)
        float myRadius = transform.localScale.x * 0.5f;
        float dist = Vector2.Distance(transform.position, prey.transform.position);
        if (dist > myRadius) return;

        me.AddPoints(prey.Points * gainRatio);
        prey.MarkEaten();
    }
}