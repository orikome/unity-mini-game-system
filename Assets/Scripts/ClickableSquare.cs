using UnityEngine;

public class ClickableSquare : MonoBehaviour
{
    private Vector2Int position;
    private Color color;

    public void Initialize(Vector2Int pos, Color col)
    {
        position = pos;
        color = col;
        GetComponent<Renderer>().material.color = col;
    }

    private void OnMouseDown()
    {
        Debug.Log($"Clicked square at {position}");
        GetComponent<Renderer>().material.color =
            GetComponent<Renderer>().material.color == Color.white ? Color.red : Color.white;
    }
}
