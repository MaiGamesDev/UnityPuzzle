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

        Debug.Log($"타일{index} 선택됨. 회전 대기.");
    }

    public void RotateLeft()
    {
        if (selectedTile == null)
        {
            Debug.LogWarning("회전할 타일이 선택되지 않았습니다.");
            return;
        }

        selectedTile.transform.Rotate(0, 0, 90f);
        Debug.Log("왼쪽으로 회전");
    }

    public void RotateRight()
    {
        if (selectedTile == null)
        {
            Debug.LogWarning("회전할 타일이 선택되지 않았습니다.");
            return;
        }

        selectedTile.transform.Rotate(0, 0, -90f);
        Debug.Log("오른쪽으로 회전");
    }
}