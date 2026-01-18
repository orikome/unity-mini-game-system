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

    public void HandleTutorial()
    {
        if (!tutorialActive)
        {
            InitializeTutorial();
        }
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
        boardManager.GenerateBoard();

        // Place knight on a fixed position for tutorial, say center
        knightPosition = new Vector2Int(2, 2); // Assuming BoardSize 5, center

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

        // Highlight correct moves with yellow
        foreach (var move in correctMoves)
        {
            boardManager.grid[move.x, move.y].GetComponent<Renderer>().material.color =
                Color.yellow;
        }

        // Tutorial UI is activated by UIManager on state change
    }

    private HashSet<Vector2Int> GetKnightMoves(Vector2Int pos)
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
        if (!tutorialActive || clickedMoves.Contains(pos))
            return;
        clickedMoves.Add(pos);

        if (correctMoves.Contains(pos))
        {
            // correct move, keep yellow or change to green?
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            // wrong move
            boardManager.grid[pos.x, pos.y].GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void CompleteTutorial()
    {
        tutorialActive = false;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();
        // Clean up board
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
        // Destroy knight
        GameObject knight = GameObject.Find("Knight");
        if (knight != null)
            Destroy(knight);
        // Go to Menu
        GameManager.Instance.CurrentState = GameState.Menu;
    }
}
