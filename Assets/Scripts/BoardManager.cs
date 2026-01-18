using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static int BoardSize = 5;
    public ClickableSquare[,] grid;

    void Start()
    {
        grid = new ClickableSquare[BoardSize, BoardSize];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int z = 0; z < BoardSize; z++)
            {
                GameObject squareObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                squareObj.transform.position = new Vector3(
                    x - BoardSize / 2f,
                    0,
                    z - BoardSize / 2f
                );
                squareObj.transform.parent = transform;
                squareObj.name = $"Square_{x}_{z}";
                squareObj.AddComponent<ClickableSquare>();
                squareObj.AddComponent<BoxCollider>();
                ClickableSquare square = squareObj.GetComponent<ClickableSquare>();
                square.Initialize(
                    new Vector2Int(x, z),
                    (x + z) % 2 == 0 ? Color.white : Color.black
                );
                grid[x, z] = square;
            }
        }
    }
}
