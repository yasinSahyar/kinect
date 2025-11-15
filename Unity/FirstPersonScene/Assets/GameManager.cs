using UnityEngine;
using TMPro; // TextMeshPro için

public class GameManager : MonoBehaviour
{
    public int totalCollectibles = 5;
    public int collectedCount = 0;
    public TextMeshProUGUI scoreText;
    public GameObject winText;

    private void Start()
    {
        if (scoreText != null)
            scoreText.text = "Score: 0 / " + totalCollectibles;

        if (winText != null)
            winText.SetActive(false);
    }

    public void CollectItem()
    {
        collectedCount++;

        if (scoreText != null)
            scoreText.text = "Score: " + collectedCount + " / " + totalCollectibles;

        if (collectedCount >= totalCollectibles)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        if (winText != null)
            winText.SetActive(true);
    }
}
