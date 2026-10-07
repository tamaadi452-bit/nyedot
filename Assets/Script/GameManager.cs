using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Mengatur game over: saat Player dimakan, Player hilang dan panel Game Over muncul.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private CellSize player;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        player.Eaten += OnPlayerEaten;
    }

    private void OnDestroy()
    {
        if (player != null)
            player.Eaten -= OnPlayerEaten;
    }

    private void OnPlayerEaten(CellSize _)
    {
        player.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    /// <summary>Dipanggil dari tombol "Main Lagi": muat ulang scene dari awal.</summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}