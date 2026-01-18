using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public ClickableSquare[,] grid;
    private int boardSize;

    public void GenerateBoard(int size)
    {
        boardSize = size;
        grid = new ClickableSquare[boardSize, boardSize];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                GameObject squareObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                squareObj.transform.position = new Vector3(
                    x - boardSize / 2f,
                    -5f, // Start below the board
                    z - boardSize / 2f
                );
                squareObj.transform.parent = transform;
                squareObj.name = $"Square_{x}_{z}";
                squareObj.AddComponent<ClickableSquare>();
                squareObj.AddComponent<BoxCollider>();
                squareObj.AddComponent<CubeRiseAnimation>();

                ClickableSquare square = squareObj.GetComponent<ClickableSquare>();
                square.Initialize(
                    new Vector2Int(x, z),
                    (x + z) % 2 == 0 ? Color.white : Color.black
                );
                grid[x, z] = square;
            }
        }
    }

    public void ClearBoard()
    {
        if (grid != null)
        {
            for (int x = 0; x < boardSize; x++)
            {
                for (int z = 0; z < boardSize; z++)
                {
                    if (grid[x, z] != null)
                    {
                        Destroy(grid[x, z].gameObject);
                    }
                }
            }
            grid = null;
        }

        // Destroy knight if exists
        GameObject knight = GameObject.Find("Knight");
        if (knight != null)
        {
            Destroy(knight);
        }
    }
}
