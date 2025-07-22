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
    public List<int> wrongIndexs = new List<int>();

    public Vector2 correctPos;
    public List<Vector2> wrongPos = new List<Vector2>();

    private List<Vector2> selectedPos = new List<Vector2>();

    public GameObject[] tiles = new GameObject[4];

    public BgImageController bgController;

    // 시작할 때 사용할 단일 프리팹
    private GameObject selectedPuzzlePrefab;

    // --------------------------------------------------------------------------------------------------------

    void Start()
    {
        cellWidth = fullWidth / gridX;
        cellHeight = fullHeight / gridY;

        // 시작할 때 퍼즐 프리팹 1개를 랜덤 선택
        selectedPuzzlePrefab = puzzlePrefab[Random.Range(0, puzzlePrefab.Length)];

        PuzzleOptions();
    }

    void PuzzleOptions()
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

        while (selectedPos.Count < 4)
        {
            int x = Random.Range(0, gridX);
            int y = Random.Range(0, gridY);

            Vector2 pos = new Vector2(x, y);

            if (!selectedPos.Contains(pos))
            {
                selectedPos.Add(pos);
            }
        }
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

        // 랜덤 명화 1개 선택
        GameObject bgRandom = bgImagePrefab[Random.Range(0, bgImagePrefab.Length)];

        if (bgController != null)
        {
            bgController.UpdateBg(bgRandom);
        }

        // 정답 인덱스와 위치 설정
        correctIndex = Random.Range(0, 4);
        correctPos = selectedPos[correctIndex];
        Debug.Log($"정답 인덱스 : {correctIndex}, 정답 위치 : {correctPos}");

        // 오답 설정
        wrongIndexs.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (i != correctIndex) wrongIndexs.Add(i);
        }

        Debug.Log($"오답 인덱스 : {string.Join(", ", wrongIndexs)}");
        wrongPos = new List<Vector2>(selectedPos);
        wrongPos.RemoveAt(correctIndex);

        // 보기 퍼즐 4개 생성 (모두 같은 프리팹 사용)
        LayoutPuzzles(selectedPuzzlePrefab, selectedPos, correctIndex, bgRandom);

        // 정답 퍼즐 홀 생성 (correctIndex 퍼즐과 동일한 위치)
        Vector2 correctPuzzlePos = selectedPos[correctIndex];
        CreatePuzzleHole(selectedPuzzlePrefab, correctPuzzlePos, bgRandom);
    }

    void LayoutPuzzles(GameObject puzzlePrefab, List<Vector2> posList, int correctIndex, GameObject bgImage)
    {
        GameObject[] puzzleObjs = new GameObject[4];

        // 1. 퍼즐 생성 + 전체 폭 측정 (모두 같은 프리팹 사용)

        for (int i = 0; i < 4; i++)
        {
            GameObject puzzleObj = Instantiate(puzzlePrefab, puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();
            puzzleObjs[i] = puzzleObj;

            tiles[i] = puzzleObj;

            // 퍼즐 클릭할 수 있도록 스크립트 추가
            PuzzleSelector pzSelcect = puzzleObj.AddComponent<PuzzleSelector>();
            pzSelcect.puzzleIndex = i;

            // 랜덤 회전 적용
            //float randomAngle = 90f * Random.Range(0, 4);
            //puzzleObj.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
        }


        // 2. 퍼즐 위치 설정 + 자식으로 명화 넣기

        for (int i = 0; i < 4; i++)
        {
            Vector2 puzzlePos = posList[i]; // 각 퍼즐은 selectedPos의 해당 인덱스 위치 사용

            AddBgImage(bgImage, puzzleObjs[i].transform, puzzlePos);
        }
    }

    /// <summary>
    /// 퍼즐 조각에 명화 이미지 자식으로 붙이기
    /// </summary>
    void AddBgImage(GameObject bgPrefab, Transform parent, Vector2 gridPos)
    {
        GameObject childImg = Instantiate(bgPrefab);
        childImg.transform.SetParent(parent, false); // 부모 설정

        Image img = childImg.GetComponent<Image>();
        img.raycastTarget = false;

        SetRectTransform(img.rectTransform, gridPos, fullWidth, fullHeight);
    }

    /// <summary>
    /// 정답 위치에 정답 퍼즐 조각만 생성 (bgController 내부에 생성)
    /// </summary>
    void CreatePuzzleHole(GameObject puzzlePrefab, Vector2 gridPos, GameObject bgImage)
    {
        if (puzzlePrefab == null || bgController == null) return;

        // bgController의 자식으로 퍼즐 조각 생성
        GameObject puzzleObj = Instantiate(puzzlePrefab);
        puzzleObj.transform.SetParent(bgController.transform, false);
        puzzleObj.transform.rotation = Quaternion.identity;

        Image puzzleImg = puzzleObj.GetComponent<Image>();
        puzzleImg.SetNativeSize();
        puzzleImg.raycastTarget = false;

        // 퍼즐 조각을 정확한 위치에 배치 (bgController 기준)
        RectTransform puzzleRect = puzzleImg.rectTransform;
        puzzleRect.anchorMin = new Vector2(0f, 1f); // 좌상단 기준
        puzzleRect.anchorMax = new Vector2(0f, 1f);
        puzzleRect.pivot = new Vector2(0.5f, 0.5f);

        // 그리드 위치를 실제 픽셀 위치로 변환
        float posX = (gridPos.x * cellWidth) + (cellWidth / 2f);
        float posY = -(gridPos.y * cellHeight) - (cellHeight / 2f);
        puzzleRect.anchoredPosition = new Vector2(posX, posY);
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