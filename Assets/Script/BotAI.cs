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

    [Header("Cari Sampah")]
    [Tooltip("Tiap berapa detik bot nyari sampah terdekat (biar gak berat)")]
    [SerializeField] private float trashSearchInterval = 0.25f;

    private Rigidbody2D rb;
    private CellSize me;
    private Vector2 moveDirection;
    private float wanderTimer;
    private Transform targetTrash;
    private float trashTimer;


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
            trashTimer -= Time.deltaTime;
            if (trashTimer <= 0f)
            {
                trashTimer = trashSearchInterval;
                targetTrash = FindNearestTrash();
            }

            if (targetTrash != null)
            {
                moveDirection = ((Vector2)targetTrash.position - (Vector2)transform.position).normalized;
            }
            else
            {
                wanderTimer -= Time.deltaTime;
                if (wanderTimer <= 0f)
                    PickRandomDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        float mult = SizeSpeed.Multiplier(me.Points);
        rb.linearVelocity = moveDirection * moveSpeed * mult;

        if (MapBoundary.Instance != null)
        {
        float radius = transform.localScale.x / 2f;
        rb.position = MapBoundary.Instance.Clamp(rb.position, radius);
        }
    }

    private Transform FindNearestTrash()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius);
        Transform nearest = null;
        float best = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (!hit.TryGetComponent(out TrashItem _)) continue;

            float d = ((Vector2)hit.transform.position - (Vector2)transform.position).sqrMagnitude;
            if (d < best)
            {
            best = d;
            nearest = hit.transform;
            }
        }   
        return nearest;
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