using UnityEngine;

/// <summary>
/// Saat bot dimakan, bot menghilang sebentar lalu muncul lagi dari ukuran awal
/// di posisi acak. Pasang di Bot.
/// </summary>
[RequireComponent(typeof(CellSize), typeof(Rigidbody2D))]
public class BotRespawn : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 3f;

    [Tooltip("Bot muncul di posisi acak dalam kotak sekitar titik (0,0) sebesar ini ke tiap arah")]
    [SerializeField] private float areaHalfSize = 15f;

    private CellSize cell;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D col;
    private BotAI ai;

    private void Awake()
    {
        cell = GetComponent<CellSize>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        ai = GetComponent<BotAI>();
    }

    private void OnEnable()
    {
        cell.Eaten += OnEaten;
    }

    private void OnDisable()
    {
        cell.Eaten -= OnEaten;
    }

    private void OnEaten(CellSize _)
    {
        SetActiveState(false);
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        Vector2 pos = new Vector2(
            Random.Range(-areaHalfSize, areaHalfSize),
            Random.Range(-areaHalfSize, areaHalfSize));

        transform.position = pos;
        rb.position = pos;

        cell.ResetCell();
        SetActiveState(true);
    }

    // Sembunyikan / munculkan bot tanpa mematikan GameObject (supaya Invoke tetap jalan)
    private void SetActiveState(bool on)
    {
        if (!on) rb.linearVelocity = Vector2.zero;

        sprite.enabled = on;
        col.enabled = on;
        rb.simulated = on;
        if (ai != null) ai.enabled = on;
    }
}