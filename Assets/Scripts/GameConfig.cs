using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class GameConfig : ScriptableObject
{
    [Header("Board Settings")]
    [Tooltip("Size of the game board (NxN)")]
    public int boardSize = 5;

    [Header("Knight Settings")]
    [Tooltip("Prefab for the knight piece. If null, creates a primitive sphere.")]
    public GameObject knightPrefab;

    [Tooltip("Fixed position for knight in tutorial mode")]
    public Vector2Int tutorialKnightPosition = new Vector2Int(2, 2);

    [Header("Timing Settings")]
    [Tooltip("Round duration in seconds")]
    public float roundDuration = 15f;

    [Tooltip("Time remaining threshold for warning display")]
    public float warningThreshold = 10f;

    [Header("Visual Settings")]
    [Tooltip("Color for the knight's square")]
    public Color knightSquareColor = Color.blue;

    [Tooltip("Color for correct moves")]
    public Color correctMoveColor = Color.green;

    [Tooltip("Color for incorrect moves")]
    public Color incorrectMoveColor = Color.red;

    [Tooltip("Color for highlighted tutorial moves")]
    public Color tutorialHighlightColor = Color.yellow;

    [Tooltip("Height at which knight spawns above the board")]
    public float knightHeight = 1f;
}
