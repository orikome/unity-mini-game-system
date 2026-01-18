using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnRestartClicked);
    }

    void OnRestartClicked()
    {
        if (
            GameManager.Instance != null
            && (
                GameManager.Instance.CurrentState == GameState.Results
                || GameManager.Instance.CurrentState == GameState.Playing
            )
        )
        {
            GameManager.Instance.RestartGame();
        }
    }
}
