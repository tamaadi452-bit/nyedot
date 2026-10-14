using UnityEngine;

public class CellEater : MonoBehaviour
{
    [Tooltip("Harus berapa kali lebih besar baru boleh makan. 1.1 = 10% lebih besar")]
    [SerializeField] private float eatRatio = 1.1f;

    [Tooltip("Berapa persen poin korban yang didapat")]
    [SerializeField] private float gainPercent = 1f;

    private CellSize me;

    private void Awake()
    {
        me = GetComponent<CellSize>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryEat(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryEat(other); // supaya tetap kena kalau ukuran baru berubah saat sudah bersentuhan
    }

    private void TryEat(Collider2D other)
    {
        if (me == null || me.IsDead) return;

        if (!other.TryGetComponent(out CellSize victim)) return;
        if (victim == me || victim.IsDead) return;

        // Aku hanya makan kalau POIN-ku jelas lebih besar
        if (me.Points > victim.Points * eatRatio)
        {
            me.AddPoints(victim.Points * gainPercent);
            victim.MarkEaten();
        }
    }
}