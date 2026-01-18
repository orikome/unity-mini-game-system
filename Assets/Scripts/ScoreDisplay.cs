using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnStateChanged += OnStateChanged;
        }

        scoreText.text = "";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnStateChanged -= OnStateChanged;
        }
    }

    private void UpdateScore(int correctClicked, int totalMoves)
    {
        scoreText.text = $"{correctClicked}/{totalMoves}";
    }

    private void OnStateChanged(GameState state)
    {
        if (state != GameState.Playing)
        {
            scoreText.text = "";
        }
    }
}
