using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PuzzleExample : MonoBehaviour
{
    public GameObject[] bgImagePrefab; // 명화 프리팹
    public GameObject[] puzzlePrefab; // 퍼즐 프리팹
    [SerializeField] private Transform puzzleParent; // 보기 4개 배치할 부모

    private int gridX = 5;
    private int gridY = 3;

    // 크기 고정 설정
    float fullWidth = 650f;
    float fullHeight = 800f;
    float cellWidth;
    float cellHeight;

    public int correctIndex;
    public List<GameObject> wrongTiles = new List<GameObject>();

    public Vector2 correctPos;
    public List<Vector2> wrongPos = new List<Vector2>();
    private List<Vector2> selectedPos = new List<Vector2>();

    public GameObject[] tiles;
    public BgImageController bgController;

    public RectTransform puzzleHoleRT;

    // --------------------------------------------------------------------------------------------------------

    void Start()
    {
        tiles = new GameObject[4];
        cellWidth = fullWidth / gridX;
        cellHeight = fullHeight / gridY;
        PuzzleOptions();
    }

    public void PuzzleOptions()
    {
        ExPuzzleInst();
    }

    // --------------------------------------------------------------------------------------------------------

    /// <summary>
    ///  퍼즐 조각 배치될 랜덤 4개 위치(좌표) 선택
    /// </summary>    

    void SelectRandomPos()
    {
        while (selectedPos.Count < 4)
        {
            int x = Random.Range(0, gridX);
            int y = Random.Range(0, gridY);

            Vector2 pos = new Vector2(x, y);

            selectedPos.Add(pos);
        }
    }

    /// <summary>
    ///  15개 프리팹 중 랜덤 1개 선택
    /// </summary>  

    GameObject SelectRandomPrefabs()
    {
        int index = Random.Range(0, puzzlePrefab.Length);
        return puzzlePrefab[index];
    }

    /// <summary>
    ///  퍼즐 초기화
    /// </summary>
    void ClearPuzzles()
    {
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }

        wrongTiles.Clear();
    }

    /// <summary>
    ///  보기 퍼즐 4개 생성
    /// </summary>
    void ExPuzzleInst()
    {
        ClearPuzzles();

        List<Vector2> allPos = new List<Vector2>();
        SelectRandomPos();

        // 랜덤 명화 1개 선택
        GameObject bgRandom = bgImagePrefab[Random.Range(0, bgImagePrefab.Length)];

        if (bgController != null)
        {
            bgController.UpdateBg(bgRandom);
        }

        GameObject selectedPrefabs = SelectRandomPrefabs();

        // 정답 인덱스 선택
        this.correctIndex = Random.Range(0, 4);
        correctPos = selectedPos[correctIndex];
        Debug.Log($"이번 퍼즐의 정답 슬롯 : {this.correctIndex + 1}번");

        wrongPos.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (i != correctIndex)
                wrongPos.Add(selectedPos[i]);
        }

        CreatePuzzleHole(selectedPrefabs, correctPos);

        // 보기 퍼즐 4개 생성
        LayoutPuzzles(selectedPrefabs, correctIndex, correctPos, wrongPos, bgRandom);
    }

    void LayoutPuzzles(GameObject prefabs, int correctIndex, Vector2 correctPos, List<Vector2> wrongPos, GameObject bgImage)
    {
        wrongTiles.Clear();

        for (int i = 0; i < 4; i++)
        {
            GameObject puzzleObj = Instantiate(prefabs, puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();
            tiles[i] = puzzleObj;

            // 퍼즐 클릭할 수 있도록 스크립트 추가
            PuzzleSelector pzSelcect = puzzleObj.AddComponent<PuzzleSelector>();
            pzSelcect.puzzleIndex = i;

           Vector2 gridPos = (i == correctIndex) ? correctPos : wrongPos[i];
            SetPuzzlePiece(puzzleObj.transform, bgImage, gridPos);

            if (i != correctIndex)
                wrongTiles.Add(puzzleObj);
        }
    }

    /// <summary>
    /// 명화 이미지 받아와 퍼즐 배치
    /// </summary>

    void SetPuzzlePiece(Transform puzzleTransform, GameObject bgImage, Vector2 gridPos)
    {
        GameObject childImg = Instantiate(bgImage);
        childImg.transform.SetParent(puzzleTransform, false); // 부모 설정

        Image img = childImg.GetComponent<Image>();
        img.raycastTarget = false;

        SetRectTransform(img.rectTransform, gridPos, fullWidth, fullHeight);
    }

    /// <summary>
    /// 정답 위치에 정답 퍼즐 조각만 생성
    /// </summary>
    void CreatePuzzleHole(GameObject puzzlePrefab, Vector2 gridPos)
    {
        if (puzzlePrefab == null) return;

        GameObject puzzleObj = Instantiate(puzzlePrefab, bgController.transform);
        puzzleObj.transform.rotation = Quaternion.identity;

        Image puzzleImg = puzzleObj.GetComponent<Image>();
        puzzleImg.SetNativeSize();
        puzzleImg.raycastTarget = false;

        SetRectTransform(puzzleImg.rectTransform, gridPos, fullWidth, fullHeight);

        puzzleHoleRT = puzzleObj.GetComponent<RectTransform>();
    }

    void SetRectTransform(RectTransform rect, Vector2 gridPos, float fullW, float fullH)
    {
        // 명화 중심 = (전체 이미지 - 셀 크기) / 2
        // 정답,오답으로 자른 이미지가 퍼즐 조각 정중앙에 오도록 상하좌우 이동 계산
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        float offsetX = (fullW - cellWidth) / 2f - (gridPos.x * cellWidth);
        float offsetY = (fullH - cellHeight) / 2f - (gridPos.y * cellHeight);

        rect.anchoredPosition = new Vector2(offsetX, offsetY);
    }
}