using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleController : MonoBehaviour
{
    public int[,] puzzleBoard = new int[5, 3]; // 퍼즐 조각 (5X3)
    public int correctIndex;

    public Vector2 emptyArea; // 퍼즐 빈 공간

    int row; // 행
    int col; // 열

    public GameObject blackPiecePrefab;
    public Sprite[] blackPieces; // 인게임 상의 빈 공간
    public Transform puzzleParent;

    private void Awake()
    {
    }

    private void Start()
    {
        RandomEmptyArea();
    }

    /// <summary>
    /// 랜덤한 검은 공간 생성
    /// </summary>
    void RandomEmptyArea()
    {
        row = Random.Range(0, 4);
        col = Random.Range(0, 2);

        Vector2 localPos = new Vector2(row * 100, -col * 100);

        GameObject blackPiece = Instantiate(blackPiecePrefab, puzzleParent);
        blackPiece.transform.localPosition = localPos;
        blackPiece.SetActive(true);
    }
}