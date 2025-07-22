using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PuzzleExample : MonoBehaviour
{
    private PuzzleImageUtil imageUtil;

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

    public GameObject[] tiles;
    public BgImageController bgController;

    public RectTransform puzzleHoleRT;

    // --------------------------------------------------------------------------------------------------------

    void Start()
    {
        tiles = new GameObject[4];
        cellWidth = fullWidth / gridX;
        cellHeight = fullHeight / gridY;

        imageUtil = new PuzzleImageUtil(gridX, gridY, fullWidth, fullHeight);

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
        HashSet<Vector2> uniquePos = new HashSet<Vector2>();

        while (uniquePos.Count < 4)
        {
            int x = Random.Range(0, gridX);
            int y = Random.Range(0, gridY);
            uniquePos.Add(new Vector2(x, y));
        }

        selectedPos = new List<Vector2>();
        foreach (var pos in uniquePos)
        {
            selectedPos.Add(new Vector2(pos.x, pos.y));
        }

        // 디버깅용 로그
        for (int i = 0; i < selectedPos.Count; i++)
        {
            Debug.Log($"선택된 위치 {i}: {selectedPos[i]}");
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

        wrongIndexs.Clear();
    }

    /// <summary>
    ///  보기 퍼즐 4개 생성
    /// </summary>
    void ExPuzzleInst()
    {
        ClearPuzzles();
        selectedPos.Clear();

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
        Debug.Log($"정답 인덱스 : {this.correctIndex}");

        CreatePuzzleHole(selectedPrefabs, correctPos, bgRandom);

        wrongPos.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (i != correctIndex)
                wrongIndexs.Add(i);
        }
        wrongPos = new List<Vector2>(selectedPos);
        wrongPos.RemoveAt(correctIndex);        

        // 보기 퍼즐 4개 생성
        LayoutPuzzles(selectedPrefabs, selectedPos, correctIndex, bgRandom);
    }

    void LayoutPuzzles(GameObject prefabs, List<Vector2> posList, int correctIndex, GameObject bgImage)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject puzzleObj = Instantiate(prefabs, puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();
            tiles[i] = puzzleObj;

            // 퍼즐 클릭할 수 있도록 스크립트 추가
            PuzzleSelector pzSelcect = puzzleObj.AddComponent<PuzzleSelector>();
            pzSelcect.puzzleIndex = i;

            Vector2 puzzlePos;

            if (i == correctIndex)
            {
                puzzlePos = posList[correctIndex];
            }
            else
            {
                int wrongListIndex = (i < correctIndex) ? i : i - 1;
                puzzlePos = wrongPos[wrongListIndex];
            }

            imageUtil.CutBgSprite(bgImage, tiles[i].transform, puzzlePos);
        }
    }
    

    /// <summary>
    /// 정답 위치에 정답 퍼즐 조각만 생성
    /// </summary>
    void CreatePuzzleHole(GameObject puzzlePrefab, Vector2 gridPos, GameObject bgPrefab)
    {
        if (puzzlePrefab == null) return;

        GameObject puzzleHole = Instantiate(puzzlePrefab, bgController.transform);
        puzzleHole.transform.SetParent(bgController.transform, false);

        Image puzzleImg = puzzleHole.GetComponent<Image>();
        puzzleImg.SetNativeSize();
        puzzleImg.raycastTarget = false;

        puzzleHoleRT = puzzleHole.GetComponent<RectTransform>();
        Vector2 bgOffset = bgController.GetComponent<RectTransform>().anchoredPosition;

        // 퍼즐 자체 위치도 gridPos 기준으로 설정
        float offsetX = (gridPos.x - gridX / 2f + 0.5f) * cellWidth;
        float offsetY = (gridPos.y - gridY / 2f + 0.5f) * cellHeight;
        puzzleHoleRT.anchoredPosition = new Vector2(offsetX, offsetY) - bgOffset;

        Debug.Log($"CreatePuzzleHole 위치 설정: {gridPos}");
    }   
}