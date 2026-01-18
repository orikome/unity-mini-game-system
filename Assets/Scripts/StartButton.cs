using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnStartClicked);
    }

    void OnStartClicked()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Menu)
        {
            GameManager.Instance.StartGame();
        }
    }
}
