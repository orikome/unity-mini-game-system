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
            }
        }
    }
    public event Action<GameState> OnStateChanged;
    private BoardManager boardManager;
    private Vector2Int knightPosition;

    // Using HashSet for unique positions
    private HashSet<Vector2Int> correctMoves;
    private HashSet<Vector2Int> clickedMoves;
    private int score;
    private Dictionary<GameState, Action> stateHandlers;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boardManager = FindFirstObjectByType<BoardManager>(); // Could be cached
        stateHandlers = new Dictionary<GameState, Action>
        {
            { GameState.Menu, HandleMenu },
            { GameState.Tutorial, HandleTutorial },
            { GameState.Playing, HandlePlaying },
            { GameState.Results, HandleResults },
        };
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

    void Update()
    {
        stateHandlers[CurrentState]();
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
        correctMoves = GetKnightMoves(knightPosition);
        clickedMoves = new HashSet<Vector2Int>();
    }

    HashSet<Vector2Int> GetKnightMoves(Vector2Int pos)
    {
        HashSet<Vector2Int> moves = new HashSet<Vector2Int>();
        int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
        int[] dz = { 1, -1, 1, -1, 2, -2, 2, -2 };
        for (int i = 0; i < 8; i++)
        {
            int nx = pos.x + dx[i];
            int nz = pos.y + dz[i];
            if (nx >= 0 && nx < BoardManager.BoardSize && nz >= 0 && nz < BoardManager.BoardSize)
            {
                moves.Add(new Vector2Int(nx, nz));
            }
        }
        return moves;
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
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = Color.green;
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

    void EndGame()
    {
        CurrentState = GameState.Results;
        score = correctMoves.Count;
        Debug.Log($"All correct moves clicked! Score: {score}. Press R to restart.");
    }

    public void RestartGame()
    {
        // Destroy all squares
        if (boardManager.grid != null)
        {
            for (int x = 0; x < BoardManager.BoardSize; x++)
            {
                for (int z = 0; z < BoardManager.BoardSize; z++)
                {
                    if (boardManager.grid[x, z] != null)
                    {
                        Destroy(boardManager.grid[x, z].gameObject);
                    }
                }
            }
            boardManager.grid = null;
        }

        // Destroy knight if exists
        GameObject knight = GameObject.Find("Knight");
        if (knight != null)
            Destroy(knight);

        score = 0;
        StartGame();
    }

    private void HandleMenu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    private void HandleTutorial()
    {
        TutorialManager.Instance.HandleTutorial();
    }

    private void HandlePlaying()
    {
        // Game logic handled in OnSquareClicked
    }

    private void HandleResults()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
}
