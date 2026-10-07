using UnityEngine;

/// <summary>
/// AI bot ala Agar.io:
/// - Ada karakter yang lebih besar di dekatnya -> kabur menjauh.
/// - Ada karakter yang lebih kecil di dekatnya -> dikejar untuk dimakan.
/// - Tidak ada keduanya -> keliling ke arah acak.
/// Pasang di Bot (butuh Rigidbody2D dan CellSize).
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CellSize))]
public class BotAI : MonoBehaviour
{
    [Header("Pergerakan")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Deteksi")]
    [Tooltip("Jarak maksimal bot bisa 'melihat' karakter lain")]
    [SerializeField] private float detectRadius = 6f;

    [Tooltip("Harus lebih besar sekian kali lipat baru dianggap lebih besar/kecil. Mencegah bot plin-plan saat ukuran hampir sama.")]
    [SerializeField] private float sizeMargin = 1.1f;

    [Header("Keliling (Wander)")]
    [Tooltip("Tiap berapa detik bot ganti arah kalau tidak ada target")]
    [SerializeField] private float wanderChangeTime = 2.5f;

    private Rigidbody2D rb;
    private CellSize me;
    private Vector2 moveDirection;
    private float wanderTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        me = GetComponent<CellSize>();
        PickRandomDirection();
    }

    private void Update()
    {
        CellSize threat = null; // yang lebih besar (berbahaya)
        CellSize prey = null;   // yang lebih kecil (mangsa)
        float threatDist = float.MaxValue;
        float preyDist = float.MaxValue;

        foreach (CellSize other in CellSize.All)
        {
            if (other == null || other == me || other.IsDead) continue;

            float dist = Vector2.Distance(transform.position, other.transform.position);
            if (dist > detectRadius) continue;

            if (other.Points > me.Points * sizeMargin)
            {
                if (dist < threatDist) { threat = other; threatDist = dist; }
            }
            else if (me.Points > other.Points * sizeMargin)
            {
                if (dist < preyDist) { prey = other; preyDist = dist; }
            }
        }

        // Prioritas: kabur dulu, baru mengejar, terakhir keliling
        if (threat != null)
        {
            moveDirection = ((Vector2)transform.position - (Vector2)threat.transform.position).normalized;
        }
        else if (prey != null)
        {
            moveDirection = ((Vector2)prey.transform.position - (Vector2)transform.position).normalized;
        }
        else
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
                PickRandomDirection();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void PickRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        wanderTimer = wanderChangeTime;
    }

    // Lingkaran kuning di Scene view = jarak pandang bot (saat bot dipilih)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}