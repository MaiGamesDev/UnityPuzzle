using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    [HideInInspector] public int correctIndex;
    [HideInInspector] public List<int> wrongIndexs = new List<int>();

    [HideInInspector] public Vector2Int correctPos;
    [HideInInspector] public List<Vector2Int> wrongPos = new List<Vector2Int>();

    [HideInInspector] public Vector2Int answerPos;
    [HideInInspector] private List<Vector2Int> selectedPos = new List<Vector2Int>();

    [HideInInspector] public GameObject[] tiles = new GameObject[4];

    [SerializeField] private Transform holeParent;
    [SerializeField] private Transform pieceParent;

    public BgImageController bgController;
    public GameObject bgRandom;

    // 시작할 때 사용할 단일 프리팹
    private GameObject selectedPuzzlePrefab;

    public int randomIndex;


    // --------------------------------------------------------------------------------------------------------
    void Start()
    {
        cellWidth = fullWidth / gridX;
        cellHeight = fullHeight / gridY;

        // 시작할 때 퍼즐 프리팹 1개를 랜덤 선택
        randomIndex = Random.Range(0, puzzlePrefab.Length);
        selectedPuzzlePrefab = puzzlePrefab[randomIndex];
        bgRandom = bgImagePrefab[randomIndex];

        PuzzleOptions(bgRandom);
    }

    public void PuzzleOptions(GameObject bg)
    {
        SelectRandomPos();
        ExPuzzleInst();
    }

    // --------------------------------------------------------------------------------------------------------

    /// <summary>
    ///  퍼즐 조각 배치될 랜덤 4개 위치(좌표) 선택
    /// </summary>    

    void SelectRandomPos()
    {
        selectedPos.Clear();

        HashSet<Vector2Int> posSet = new HashSet<Vector2Int>();

        while (posSet.Count < 4)
        {
            int x = Random.Range(0, gridX);
            int y = Random.Range(0, gridY);
            posSet.Add(new Vector2Int(x, y));
        }

        selectedPos = posSet.ToList();

        correctIndex = Random.Range(0, selectedPos.Count);

        answerPos = selectedPos[correctIndex];
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
    }

    /// <summary>
    ///  보기 퍼즐 4개 생성 (모두 같은 프리팹 사용)
    /// </summary>
    void ExPuzzleInst()
    {
        ClearPuzzles();

        if (bgController != null)
            bgController.UpdateBg(bgRandom);


        // 정답 인덱스와 위치 설정
        correctPos = answerPos;

        // 오답 설정
        wrongIndexs.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (i != correctIndex) wrongIndexs.Add(i);
        }

        wrongPos = new List<Vector2Int>(selectedPos);
        wrongPos.RemoveAt(correctIndex);

        // 보기 퍼즐 4개 생성 (모두 같은 프리팹 사용)
        LayoutPuzzles(selectedPuzzlePrefab, selectedPos, bgRandom);

        // 정답 퍼즐 홀 생성 (answerPos 위치)
        CreatePuzzleHole(selectedPuzzlePrefab, answerPos, bgRandom);
    }

    void LayoutPuzzles(GameObject puzzlePrefab, List<Vector2Int> posList, GameObject bgImage)
    {
        GameObject[] puzzleObjs = new GameObject[4];

        // 1. 퍼즐 생성 + 전체 폭 측정 (모두 같은 프리팹 사용)

        for (int i = 0; i < posList.Count; i++)
        {
            GameObject puzzleObj = Instantiate(puzzlePrefab, puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();

            tiles[i] = puzzleObj;

            puzzleObj.name = $"Puzzle_{i}";

            // 드래그 정답 퍼즐 설정 (07-29 추가)
            var drag = puzzleObj.GetComponent<JoystickController>()
                       ?? puzzleObj.AddComponent<JoystickController>();
            drag.puzzleIndex = i;      // 0~3 중 이 조각의 인덱스
            drag.puzzleExample = this;

            // 퍼즐 클릭할 수 있도록 스크립트 추가
            PuzzleSelector pzSelcect = puzzleObj.AddComponent<PuzzleSelector>();
            pzSelcect.puzzleIndex = i;

            // 랜덤 회전 적용
            int randomAngle = 90 * Random.Range(0, 4);
            puzzleObj.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);

            AddBgImage(bgImage, puzzleObj.transform, posList[i]);
        }
    }

    /// <summary>
    /// 퍼즐 조각에 명화 이미지 자식으로 붙이기
    /// </summary>
    void AddBgImage(GameObject bgPrefab, Transform parent, Vector2Int gridPos)
    {
        GameObject childImg = Instantiate(bgPrefab);
        childImg.transform.SetParent(parent, false); // 부모 설정

        Image img = childImg.GetComponent<Image>();
        img.raycastTarget = false;

        SetRectTransform(img.rectTransform, (Vector2)gridPos, fullWidth, fullHeight);
    }

    /// <summary>
    /// 정답 위치에 정답 퍼즐 조각만 생성 (bgController 내부에 생성)
    /// </summary>
    void CreatePuzzleHole(GameObject puzzlePrefab, Vector2Int gridPos, GameObject bgImage)
    {
        if (puzzlePrefab == null || bgController == null) return;

        // bgController의 자식으로 퍼즐 조각 생성
        GameObject puzzleObj = Instantiate(puzzlePrefab, holeParent, false);

        var drag = puzzleObj.GetComponent<JoystickController>();
        drag.isDraggable = false;

        // 빈 퍼즐 영역 (07-29 추가)
        var dropZone = puzzleObj.AddComponent<PuzzleDropZone>();
        dropZone.puzzleExample = this;

        puzzleObj.transform.rotation = Quaternion.identity;

        Image puzzleImg = puzzleObj.GetComponent<Image>();
        puzzleImg.SetNativeSize();
        puzzleImg.raycastTarget = true;

        // 퍼즐 조각을 배경 기준 정중앙에 배치
        RectTransform puzzleRect = puzzleImg.rectTransform;
        puzzleRect.anchorMin = new Vector2(0.5f, 0.5f);
        puzzleRect.anchorMax = new Vector2(0.5f, 0.5f);
        puzzleRect.pivot = new Vector2(0.5f, 0.5f);

        // 정중앙에서 퍼즐 위치 answerPos로 이동
        float posX = (gridPos.x * cellWidth) - (fullWidth / 2f) + (cellWidth / 2f);
        float posY = (gridPos.y * cellHeight) - (fullHeight / 2f) + (cellHeight / 2f);
        puzzleRect.anchoredPosition = new Vector2(posX, posY);

        // AddBgImage(bgImage, puzzleObj.transform, gridPos);
    }

    void SetRectTransform(RectTransform rect, Vector2 gridPos, float fullW, float fullH)
    {
        // 명화 중심 = (전체 이미지 - 셀 크기) / 2
        // 정답,오답으로 자른 이미지가 퍼즐 조각 정중앙에 오도록 상하좌우 이동 계산
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        float offsetX = (fullW - cellWidth) / 2f - (gridPos.x * cellWidth);
        float offsetY = (cellHeight * gridY - cellHeight) / 2f - (gridPos.y * cellHeight);

        rect.anchoredPosition = new Vector2(offsetX, offsetY);
    }

    public void DestroyChildren()
    {
        for (int i = holeParent.childCount - 1; i >= 0; i--)
        {
            Destroy(holeParent.GetChild(i).gameObject);
        }
        for (int i = pieceParent.childCount - 1; i >= 0; i--)
        {
            Destroy(pieceParent.GetChild(i).gameObject);
        }
    }

    public void PuzzleReset()
    {
        randomIndex = Random.Range(0, puzzlePrefab.Length);
        selectedPuzzlePrefab = puzzlePrefab[randomIndex];
        bgRandom = bgImagePrefab[randomIndex];

        PuzzleOptions(bgRandom);
    }
}