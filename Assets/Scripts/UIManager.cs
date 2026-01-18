using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject menuUI;
    public GameObject playingUI;
    public GameObject resultsUI;
    public GameObject tutorialUI;

    private CanvasGroup currentUI;

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
        CanvasGroup newUI = GetCanvasGroupForState(newState);
        if (newUI == null)
            return;
        if (currentUI != null && currentUI != newUI)
        {
            StartCoroutine(Transition(currentUI, newUI));
        }
        else if (currentUI == null)
        {
            // initial: deactivate all, then activate current
            menuUI.SetActive(false);
            playingUI.SetActive(false);
            resultsUI.SetActive(false);
            tutorialUI.SetActive(false);
            newUI.gameObject.SetActive(true);
            newUI.alpha = 1;
            newUI.interactable = true;
            newUI.blocksRaycasts = true;
            currentUI = newUI;
        }
        // if same, do nothing
    }

    private CanvasGroup GetCanvasGroupForState(GameState state)
    {
        GameObject go = null;
        switch (state)
        {
            case GameState.Menu:
                go = menuUI;
                break;
            case GameState.Playing:
                go = playingUI;
                break;
            case GameState.Results:
                go = resultsUI;
                break;
            case GameState.Tutorial:
                go = tutorialUI;
                break;
        }
        if (go != null)
        {
            var cg = go.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = go.AddComponent<CanvasGroup>();
            return cg;
        }
        return null;
    }

    private IEnumerator Transition(CanvasGroup oldUI, CanvasGroup newUI)
    {
        yield return Helpers.AnimateTransition(oldUI, newUI);
        currentUI = newUI;
    }
}
