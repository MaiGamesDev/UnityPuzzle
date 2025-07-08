using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleController : MonoBehaviour
{
    public int[,] puzzleBoard = new int[5, 3]; // 퍼즐 조각 (5X3)
    public int correctIndex;

    public Vector3Int emptyArea; // 퍼즐 빈 공간

    int row; // 행
    int col; // 열

    public GameObject[] candidateTiles; // 보기 4개

    public GameObject blackPiece; // 인게임 상의 빈 공간
    public GameObject wrongTile; // 4개중 틀린 3개의 타일
    public GameObject correctTile; // 4개중 맞는 1개의 타일

    private void Awake()
    {
        blackPiece = GetComponent<GameObject>();
        candidateTiles = new GameObject[4];
    }

    private void Start()
    {
        RandomEmptyArea();
    }

    /// <summary>
    /// 랜덤한 검은 공간 생겅
    /// </summary>
    void RandomEmptyArea() 
    {
        row = Random.Range(0, 4);
        col = Random.Range(0, 2);

        emptyArea = new Vector3Int(row, col, 0);

        blackPiece.transform.position = emptyArea;
        blackPiece.SetActive(true);
    }
    void GuessPuzzle()
    {
    }
}


