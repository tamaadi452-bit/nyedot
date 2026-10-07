using UnityEngine;

/// <summary>
/// Gerak player ala snake/Agar.io: tekan arah sekali, karakter terus jalan lurus
/// ke arah itu sampai arah baru ditentukan. Pasang di GameObject Player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referensi (boleh kosong kalau tes di laptop)")]
    [SerializeField] private Joystick joystick;

    [Header("Pengaturan")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("Aktif: karakter tetap jalan lurus setelah tombol dilepas. Nonaktif: berhenti saat dilepas.")]
    [SerializeField] private bool keepMovingAfterRelease = true;

    [Tooltip("Aktif: hanya 4 arah (atas/bawah/kiri/kanan) seperti snake. Nonaktif: boleh diagonal.")]
    [SerializeField] private bool fourDirectionsOnly = false;

    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Prioritas: joystick. Kalau tidak disentuh, coba keyboard (untuk tes di PC).
        Vector2 input = joystick != null ? joystick.Direction : Vector2.zero;

        if (input == Vector2.zero)
            input = ReadKeyboard();

        if (input != Vector2.zero)
        {
            // Ada input baru: ganti arah jalan
            if (fourDirectionsOnly)
            {
                // Ambil sumbu yang paling dominan saja
                input = Mathf.Abs(input.x) >= Mathf.Abs(input.y)
                    ? new Vector2(Mathf.Sign(input.x), 0f)
                    : new Vector2(0f, Mathf.Sign(input.y));
            }

            moveDirection = input.normalized;
        }
        else if (!keepMovingAfterRelease)
        {
            moveDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    // Keyboard WASD / panah, hanya untuk testing di editor.
    private Vector2 ReadKeyboard()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb == null) return Vector2.zero;

        float x = (kb.dKey.isPressed || kb.rightArrowKey.isPressed ? 1f : 0f)
                - (kb.aKey.isPressed || kb.leftArrowKey.isPressed ? 1f : 0f);
        float y = (kb.wKey.isPressed || kb.upArrowKey.isPressed ? 1f : 0f)
                - (kb.sKey.isPressed || kb.downArrowKey.isPressed ? 1f : 0f);
        return Vector2.ClampMagnitude(new Vector2(x, y), 1f);
#elif ENABLE_LEGACY_INPUT_MANAGER
        return Vector2.ClampMagnitude(
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
#else
        return Vector2.zero;
#endif
    }
}