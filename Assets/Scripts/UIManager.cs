using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject menuUI;
    public GameObject playingUI;
    public GameObject resultsUI;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
        OnStateChanged(GameManager.Instance.CurrentState);
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        menuUI.SetActive(newState == GameState.Menu);
        playingUI.SetActive(newState == GameState.Playing);
        resultsUI.SetActive(newState == GameState.Results);
    }
}
