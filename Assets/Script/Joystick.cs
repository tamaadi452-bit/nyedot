using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Digital analog (joystick) di layar. Pasang di GameObject background joystick.
/// Arah gerak bisa dibaca lewat properti Direction (panjang 0 sampai 1).
/// </summary>
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Tooltip("Gambar bulat kecil yang digeser (child dari background)")]
    [SerializeField] private RectTransform handle;

    [Tooltip("Seberapa jauh handle boleh keluar dari tengah (1 = sampai tepi background)")]
    [SerializeField, Range(0.1f, 1f)] private float handleRange = 1f;

    [Tooltip("Input di bawah nilai ini dianggap 0, supaya tidak gerak sendiri")]
    [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.1f;

    private RectTransform background;
    private Canvas canvas;

    /// <summary>Arah joystick, nilai -1 sampai 1 di sumbu X dan Y.</summary>
    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        background = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, cam, out Vector2 localPoint))
            return;

        float radius = background.rect.width * 0.5f;
        Vector2 input = Vector2.ClampMagnitude(localPoint / radius, 1f);

        Direction = input.magnitude < deadZone ? Vector2.zero : input;

        if (handle != null)
            handle.anchoredPosition = input * radius * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
