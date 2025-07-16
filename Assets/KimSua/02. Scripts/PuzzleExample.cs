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

    private int correctIndex;

    private List<Vector2> selectedPos = new List<Vector2>(); // 4개(정답1 + 오답3)

    // --------------------------------------------------------------------------------------------------------

    void Start()
    {
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
    ///  15개 프리팹 중 랜덤 선택
    /// </summary>  

    GameObject[] SelectRandomPrefabs()
    {
        List<GameObject> prefabs = new List<GameObject>(puzzlePrefab);
        GameObject[] selectedPrefabs = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Count);
            selectedPrefabs[i] = prefabs[randomIndex];
            prefabs.RemoveAt(randomIndex); // 중복 방지 (겹치는 인덱스는 제거)
        }

        return selectedPrefabs;
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
    ///  보기 퍼즐 4개 생성
    /// </summary>
    void ExPuzzleInst()
    {
        ClearPuzzles();

        // 랜덤 명화 1개 선택
        GameObject bgRandom = bgImagePrefab[Random.Range(0, bgImagePrefab.Length)];

        GameObject[] selectedPrefabs = SelectRandomPrefabs();
        correctIndex = Random.Range(0, 4);
        Debug.Log($"정답 인덱스 : {correctIndex}");

        LayoutPuzzles(selectedPrefabs, selectedPos, correctIndex, bgRandom);
    }

    void LayoutPuzzles(GameObject[] prefabs, List<Vector2> posList, int correctIndex, GameObject bgImage)
    {
        GameObject[] puzzleObjs = new GameObject[4];

        // 1. 퍼즐 생성 + 전체 폭 측정

        for (int i = 0; i < 4; i++)
        {
            GameObject puzzleObj = Instantiate(prefabs[i], puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();
            puzzleObjs[i] = puzzleObj;
        }


        // 2. 퍼즐 위치 설정 + 자식으로 명화 넣기
      
        for (int i = 0; i < 4; i++)
        {
            Vector2 puzzlePos;
            
            if (i == correctIndex)
                puzzlePos = posList[correctIndex];

            else
            {
                List<Vector2> wrongPos = new List<Vector2>(posList);
                wrongPos.RemoveAt(correctIndex);

                int wrongIndex = (i < correctIndex) ? i : i -1;
                puzzlePos = wrongPos[wrongIndex];
            }

            AddImage(bgImage, puzzleObjs[i].transform, puzzlePos);
        }
    }

    void AddImage(GameObject bgPrefab, Transform parent, Vector2 gridPos)
    {
        GameObject childImg = Instantiate(bgPrefab);
        childImg.transform.SetParent(parent, false); // 부모 설정
        Image img = childImg.GetComponent<Image>();
        RectTransform bgRect = img.GetComponent<RectTransform>();

        img.raycastTarget = false;

        // 크기 고정 설정
        float fullWidth = 650f;
        float fullHeight = 800f;
        float cellWidth = fullWidth / gridX;
        float cellHeight = fullHeight / gridY;

        bgRect.anchorMin = new Vector2(0.5f, 0.5f);
        bgRect.anchorMax = new Vector2(0.5f, 0.5f);
        bgRect.pivot = new Vector2(0.5f, 0.5f);
        bgRect.sizeDelta = new Vector2(fullWidth, fullHeight);


        // 명화 중심 = (전체 이미지 - 셀 크기) / 2
        // 정답,오답으로 자른 이미지가 퍼즐 조각 정중앙에 오도록 상하좌우 이동 계산
        float totalOffsetX = (fullWidth - cellWidth) / 2f;
        float totalOffsetY = (fullHeight - cellHeight) / 2f;

        float offsetX = totalOffsetX - (gridPos.x * cellWidth);
        float offsetY = totalOffsetY - (gridPos.y * cellHeight);
        bgRect.anchoredPosition = new Vector2(offsetX, offsetY);

    }
}