using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private CellSize target;   // boleh dikosongin, nanti dicari otomatis
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // Cari Player otomatis (juga kalau Player di-respawn)
        if (target == null)
        {
            PlayerMovement p = FindFirstObjectByType<PlayerMovement>();
            if (p != null) target = p.GetComponent<CellSize>();
            return;
        }

        label.text = $"Points: {Mathf.FloorToInt(target.Points)}";
    }
}