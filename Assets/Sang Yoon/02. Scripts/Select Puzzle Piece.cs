using UnityEngine;

class SelectPuzzlePiece : MonoBehaviour 
{
    private PuzzleExample puzzleEx;
    public GameObject selectedTile;
    public GameObject rTurnButton;
    public GameObject lTurnButton;

    public Vector3 puzzleRightTurn;
    public Vector3 puzzleLeftTurn;
    private Vector3 defaultScale = Vector3.one;
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
}