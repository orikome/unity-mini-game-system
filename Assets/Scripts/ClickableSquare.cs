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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSquareClicked(position);
        }
    }
}
