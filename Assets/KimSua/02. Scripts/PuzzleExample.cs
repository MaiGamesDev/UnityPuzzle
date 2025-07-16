using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
    public GameObject[] bgImagePrefab; // 명화 프리팹
    public GameObject[] puzzlePrefab; // 퍼즐 프리팹
    [SerializeField] private Transform puzzleParent; // 보기 4개 배치할 부모

    private int gridX = 5;
    private int gridY = 3;

    private int correctIndex;

    private List<(int x, int y)> selectedPos = new List<(int x, int y)>(); // 4개(정답1 + 오답3)

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
    ///  (정답 포함)랜덤 4개 위치 선택
    /// </summary>    

    void SelectRandomPos()
    {
        selectedPos.Clear();

        while (selectedPos.Count < 4)
        {
            int x = Random.Range(0, gridX);
            int y = Random.Range(0, gridY);

            if (!selectedPos.Contains((x, y)))
            {
                selectedPos.Add((x, y));
            }
        }

        // 정답 인덱스 결정
        correctIndex = Random.Range(0, 4);
        Debug.Log($"정답 인덱스 : {correctIndex}");
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
    ///  정답 조각 랜덤 위치 & Sprite 생성
    /// </summary> 
    Vector2 CorrectPuzzle()
    {
        var pos = selectedPos[correctIndex];
        return new Vector2(pos.x, pos.y);
    }

    /// <summary>
    ///  오답 조각 랜덤 위치 크롭
    /// </summary> 
    List<Vector2> WrongPuzzles()
    {
        List<Vector2> wrongs = new List<Vector2>();

        for (int i = 0; i < selectedPos.Count; i++)
        {
            if (i == correctIndex) continue;
            wrongs.Add(new Vector2(selectedPos[i].x, selectedPos[i].y));
        }

        return wrongs;
    }

    /// <summary>
    ///  보기 퍼즐 4개 생성
    /// </summary>
    void ExPuzzleInst()
    {
        // 퍼즐 초기화
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }

        // 랜덤 명화 1개 선택
        GameObject bgRandom = bgImagePrefab[Random.Range(0, bgImagePrefab.Length)];

        float fullWidth = 650f;
        float fullHeight = 800f;
        float cellWidth = fullWidth / gridX;
        float cellHeight = fullHeight / gridY;

        // 퍼즐 모양 15개 중 4개 랜덤 선택
        GameObject[] selectedPrefabs = SelectRandomPrefabs();

        Vector2 correctPos = CorrectPuzzle(); // 정답 위치
        List<Vector2> wrongPosList = WrongPuzzles(); // 오답 위치들

        // 4개 퍼즐 생성
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos;
            if (i == correctIndex)
            {
                pos = correctPos;
            }
            else
            {
                if (i < correctIndex)
                {
                    pos = wrongPosList[i];
                }
                else
                {
                    pos = wrongPosList[i - 1];
                }
            }

            // 퍼즐 외형 프리팹 (Mask용)
            GameObject puzzleObj = Instantiate(selectedPrefabs[i], puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();

            GameObject childImg = Instantiate(bgRandom, puzzleObj.transform);
            Image img = childImg.GetComponent<Image>();
            RectTransform bgRect = img.GetComponent<RectTransform>();

            if (img != null)
            {
                img.raycastTarget = false;

                // 크기 고정 설정
                bgRect.anchorMin = new Vector2(0.5f, 0.5f);
                bgRect.anchorMax = new Vector2(0.5f, 0.5f);
                bgRect.pivot = new Vector2(0.5f, 0.5f);
                bgRect.sizeDelta = new Vector2(fullWidth, fullHeight);

                // 명화 위치 이동
                float offsetX = -(cellWidth * (gridX - 1) / 2f) + pos.x * cellWidth;
                float offsetY = (cellHeight * (gridY - 1) / 2f) + pos.y * cellHeight;
                bgRect.anchoredPosition = new Vector2(offsetX, offsetY);

            }
        }
    }
}