using UnityEngine;
using UnityEngine.UI;
using System;

public class TurnButtonController : MonoBehaviour
{
    private PuzzleExample puzzleEx;
    public GameObject selectedTile;

    private Vector3 defaultScale = Vector3.one;

    private void Awake()
    {
        puzzleEx = FindFirstObjectByType<PuzzleExample>();
    }

    public void SelectTile(int index)
    {
        if (index < 0 || puzzleEx.tiles == null)
            return;

        // 이전 선택 퍼즐은 원래 크기로
        foreach (GameObject tile in puzzleEx.tiles)
        {
            tile.transform.localScale = defaultScale;
        }

        selectedTile = puzzleEx.tiles[index];

        // 선택 퍼즐 확대
        selectedTile.transform.localScale = defaultScale * 1.1f;
    }

    public void RotateLeft()
    {
        if (selectedTile == null)
            return;

        selectedTile.transform.Rotate(0, 0, 90f);
    }

    public void RotateRight()
    {
        if (selectedTile == null)
            return;

        selectedTile.transform.Rotate(0, 0, -90f);
    }
}