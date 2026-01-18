using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private BoardManager boardManager;
    private Vector2Int knightPosition;
    private HashSet<Vector2Int> correctMoves;
    private HashSet<Vector2Int> clickedMoves;
    private bool tutorialActive = false;

    void Awake()
    {
        Instance = this;
    }

    public void StartTutorial()
    {
        if (!tutorialActive)
        {
            InitializeTutorial();
        }
    }

    private void Update()
    {
        if (!tutorialActive)
            return;

        // Check if tutorial completed
        if (correctMoves.All(m => clickedMoves.Contains(m)))
        {
            CompleteTutorial();
        }
    }

    private void InitializeTutorial()
    {
        tutorialActive = true;
        boardManager = FindFirstObjectByType<BoardManager>();
        boardManager.GenerateBoard(GameManager.Instance.Config.boardSize);

        // Place knight on a fixed position for tutorial
        knightPosition = GameManager.Instance.Config.tutorialKnightPosition;

        // Spawn knight
        GameObject knight = SpawnKnight(knightPosition);

        // Set knight square color
        boardManager
            .grid[knightPosition.x, knightPosition.y]
            .GetComponent<Renderer>()
            .material.color = GameManager.Instance.Config.knightSquareColor;

        // Calculate correct moves
        correctMoves = Helpers.GetKnightMoves(
            knightPosition,
            GameManager.Instance.Config.boardSize
        );
        clickedMoves = new HashSet<Vector2Int>();

        // Highlight correct moves with yellow
        foreach (var move in correctMoves)
        {
            boardManager.grid[move.x, move.y].GetComponent<Renderer>().material.color = GameManager
                .Instance
                .Config
                .tutorialHighlightColor;
        }

        // Tutorial UI is activated by UIManager on state change
    }

    public void OnSquareClicked(Vector2Int pos)
    {
        if (!tutorialActive || clickedMoves.Contains(pos))
            return;
        clickedMoves.Add(pos);

        if (correctMoves.Contains(pos))
        {
            // correct move, keep yellow or change to green?
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = GameManager
                .Instance
                .Config
                .correctMoveColor;
        }
        else
        {
            // wrong move
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = GameManager
                .Instance
                .Config
                .incorrectMoveColor;
        }
    }

    private GameObject SpawnKnight(Vector2Int position)
    {
        GameObject knight;

        if (GameManager.Instance.Config.knightPrefab != null)
        {
            knight = Instantiate(GameManager.Instance.Config.knightPrefab);
        }
        else
        {
            // Fallback to primitive if no prefab assigned
            knight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }

        knight.transform.position = new Vector3(
            position.x - GameManager.Instance.Config.boardSize / 2f,
            GameManager.Instance.Config.knightHeight,
            position.y - GameManager.Instance.Config.boardSize / 2f
        );
        knight.name = "Knight";

        return knight;
    }

    private void CompleteTutorial()
    {
        tutorialActive = false;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();

        // Clean up board
        boardManager.ClearBoard();

        // Go to Menu
        GameManager.Instance.CurrentState = GameState.Menu;
    }
}
