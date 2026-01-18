using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameState _currentState;
    public GameState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnStateChanged?.Invoke(_currentState);
                OnStateEntered(_currentState);
            }
        }
    }
    public event Action<GameState> OnStateChanged;
    public event Action<int, int> OnScoreChanged; // (correctClicked, totalMoves)
    private BoardManager boardManager;
    private Vector2Int knightPosition;

    // Using HashSet for unique positions
    private HashSet<Vector2Int> correctMoves;
    private HashSet<Vector2Int> clickedMoves;
    private HashSet<Vector2Int> correctClickedMoves;
    private int score;
    private bool didWin;

    public bool DidWin => didWin;
    public int CorrectClickedCount => correctClickedMoves?.Count ?? 0;
    public int TotalMoves => correctMoves?.Count ?? 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boardManager = FindFirstObjectByType<BoardManager>(); // Could be cached

        // Subscribe to timer events
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimeUp += OnTimeUp;
        }

        if (
            !PlayerPrefs.HasKey("TutorialCompleted")
            || PlayerPrefs.GetInt("TutorialCompleted") == 0
        )
        {
            CurrentState = GameState.Tutorial;
        }
        else
        {
            CurrentState = GameState.Menu;
        }
        Debug.Log("Press Space to start the game");
    }

    public void StartGame()
    {
        boardManager.GenerateBoard();

        CurrentState = GameState.Playing;
        Debug.Log("Game started! Click all possible knight moves.");

        // Place knight on random square
        knightPosition = new Vector2Int(
            UnityEngine.Random.Range(0, BoardManager.BoardSize),
            UnityEngine.Random.Range(0, BoardManager.BoardSize)
        );

        // Spawn knight primitive
        GameObject knight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        knight.transform.position = new Vector3(
            knightPosition.x - BoardManager.BoardSize / 2f,
            1,
            knightPosition.y - BoardManager.BoardSize / 2f
        );
        knight.name = "Knight";

        // Set knight square color
        boardManager
            .grid[knightPosition.x, knightPosition.y]
            .GetComponent<Renderer>()
            .material.color = Color.blue;

        // Calculate correct moves
        correctMoves = Helpers.GetKnightMoves(knightPosition, BoardManager.BoardSize);
        clickedMoves = new HashSet<Vector2Int>();
        correctClickedMoves = new HashSet<Vector2Int>();

        // Fire initial score event
        OnScoreChanged?.Invoke(0, correctMoves.Count);

        // Start timer
        if (Timer.Instance != null)
        {
            Timer.Instance.StartTimer(15f);
        }
    }

    public void OnSquareClicked(Vector2Int pos)
    {
        if (CurrentState == GameState.Tutorial)
        {
            TutorialManager.Instance.OnSquareClicked(pos);
            return;
        }
        if (CurrentState != GameState.Playing)
            return;
        if (clickedMoves.Contains(pos))
            return; // already clicked
        clickedMoves.Add(pos);

        if (correctMoves.Contains(pos))
        {
            // correct move
            correctClickedMoves.Add(pos);
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = Color.green;
            OnScoreChanged?.Invoke(correctClickedMoves.Count, correctMoves.Count);
        }
        else
        {
            // wrong move
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = Color.red;
        }

        // Check if all correct moves are clicked
        if (correctMoves.All(m => clickedMoves.Contains(m)))
        {
            EndGame();
        }
    }

    void EndGame(bool won = true)
    {
        didWin = won;

        // Stop timer
        if (Timer.Instance != null)
        {
            Timer.Instance.StopTimer();
        }

        CurrentState = GameState.Results;
        score = correctMoves.Count;
        if (won)
        {
            Debug.Log($"All correct moves clicked! Score: {score}. Press R to restart.");
        }
        else
        {
            Debug.Log(
                $"Time's up! You clicked {clickedMoves.Count} out of {score} correct moves. Press R to restart."
            );
        }
    }

    void OnTimeUp()
    {
        if (CurrentState == GameState.Playing)
        {
            EndGame(false);
        }
    }

    public void RestartGame()
    {
        // Clean up board and knight
        boardManager.ClearBoard();

        // Stop timer if running
        if (Timer.Instance != null)
        {
            Timer.Instance.StopTimer();
        }

        score = 0;
        StartGame();
    }

    private void OnDestroy()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimeUp -= OnTimeUp;
        }
    }

    private void OnStateEntered(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Tutorial:
                TutorialManager.Instance.StartTutorial();
                break;
            case GameState.Playing:
                break;
            case GameState.Results:
                break;
        }
    }
}
