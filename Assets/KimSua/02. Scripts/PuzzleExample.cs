using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
    #region 멤버변수
    public GameObject[] bgImagePrefab; // 명화 프리팹
    public GameObject[] puzzlePrefab; // 퍼즐 프리팹
    [SerializeField] private Transform puzzleParent; // 보기 4개 배치할 부모
    [SerializeField] private Transform outlineParent;

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

    [SerializeField] private BgImageController bgController;
    public GameObject bgRandom;

    // 시작할 때 사용할 단일 프리팹
    private GameObject selectedPuzzlePrefab;

    private int bgRanIndex;
    private int puzzleRanIndex;
    #endregion

    void Start()
    {
        cellWidth = fullWidth / gridX;
        cellHeight = fullHeight / gridY;

        bgRanIndex = Random.Range(0, bgImagePrefab.Length);
        bgRandom = bgImagePrefab[bgRanIndex];

        puzzleRanIndex = Random.Range(0, puzzlePrefab.Length);
        selectedPuzzlePrefab = puzzlePrefab[puzzleRanIndex];

        PuzzleOptions(bgRandom);
    }

    public void PuzzleOptions(GameObject bg)
    {
        SelectRandomPos();
        ExPuzzleInst();
    }

    #region 기본 좌표, 보기 퍼즐, 정답 위치 생성

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
        for (int i = puzzleParent.childCount - 1; i >= 0; i--)
        {
            Destroy(puzzleParent.GetChild(i).gameObject);
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
    #endregion

    #region 퍼즐 삭제
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
        ClearPuzzles();
        DestroyChildren();
        ResetBlinking();

        bgRanIndex = Random.Range(0, bgImagePrefab.Length);
        bgRandom = bgImagePrefab[bgRanIndex];

        puzzleRanIndex = Random.Range(0, puzzlePrefab.Length);
        selectedPuzzlePrefab = puzzlePrefab[puzzleRanIndex];

        PuzzleOptions(bgRandom);
    }
    #endregion


    #region 아웃라인 추가
    private List<GameObject> outlinePuzzles = new List<GameObject>(); // 아웃라인 퍼즐들 관리용
    private List<int> hintIndexs = new List<int>(); // 아웃라인 퍼즐들 관리용
    private bool isBlinking = false;
    float outlineThickness = 6f;

    public void ShowHint()
    {
        ClearOutlines();
        BuildHintIndexs();

        foreach (int idx in hintIndexs)
        {
            if (idx < 0 || idx >= tiles.Length) continue;
            if (tiles[idx] == null) continue;

            var outline = CreateOutlinePuzzle(idx);
            if (outline != null) outlinePuzzles.Add(outline);
        }
    }

    void BuildHintIndexs()
    {
        hintIndexs.Clear();

        hintIndexs.Add(correctIndex);

        // 오답 2개 뽑기 (중복x)
        var pool = new List<int>(wrongIndexs);
        for (int i = 0; i < 2 && pool.Count > 0; i++)
        {
            int r = Random.Range(0, pool.Count);
            hintIndexs.Add(pool[r]);
            pool.RemoveAt(r);
        }
    }

    GameObject CreateOutlinePuzzle(int tileIndex)
    {
        Transform main = tiles[tileIndex].transform;

        GameObject outlinePuzzle = Instantiate(selectedPuzzlePrefab, outlineParent);
        outlinePuzzle.name = main.name + "_Outline";

        var mainRect = (RectTransform)main;
        var outlineRect = (RectTransform)outlinePuzzle.transform;

        outlineRect.anchorMin = mainRect.anchorMin;
        outlineRect.anchorMax = mainRect.anchorMax;
        outlineRect.pivot = mainRect.pivot;

        // 초기 위치 World Position 설정
        outlineRect.position = mainRect.position;
        outlineRect.rotation = mainRect.rotation;
        outlineRect.localScale = mainRect.localScale;

        // 아웃라인 두께 설정(부모 scale이 변해도 동일한 두께이도록 전역scale 사용)
        float tx = outlineThickness / mainRect.lossyScale.x;
        float ty = outlineThickness / mainRect.lossyScale.y;
        outlineRect.sizeDelta = mainRect.sizeDelta + new Vector2(tx * 3f, ty * 3f);

        Image outlineImg = outlinePuzzle.GetComponent<Image>();
        outlineImg.raycastTarget = false;
        outlineImg.color = new Color(14 / 255f, 255 / 255f, 185 / 255f);

        // OutlineFollower 추가
        var follower = outlinePuzzle.AddComponent<OutlineFollower>();
        follower.target = main;

        return outlinePuzzle;
    }


    public void StartBlinking()
    {
        if (!isBlinking)
        {
            StartCoroutine(BlinkingRoutine());
        }
    }

    IEnumerator BlinkingRoutine()
    {
        isBlinking = true;
        float blinkSpeed = 6f; // 깜빡거리는 속도
        bool visible = true;

        while (isBlinking)
        {
            foreach (var outline in outlinePuzzles)
            {
                if (outline != null)
                    outline.SetActive(visible);
            }

            visible = !visible;
            yield return new WaitForSeconds(1f / blinkSpeed);
        }
    }

    public void ClearOutlines()
    {
        for (int i = outlineParent.childCount - 1; i >= 0; i--)
        {
            Destroy(outlineParent.GetChild(i).gameObject);
        }
        outlinePuzzles.Clear();
    }

    public void ResetBlinking()
    {
        isBlinking = false;
        ClearOutlines();
    }
    #endregion
}