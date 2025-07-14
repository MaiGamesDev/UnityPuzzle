using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
    public Image bgImage; // 원본 이미지
    public GameObject[] puzzlePrefab; // 퍼즐 프리팹
    [SerializeField] private Transform puzzleParent; // 보기 4개 배치할 부모
    [SerializeField] private RectTransform puzzleArea; // 보기 4개 배치할 영역

    private int gridX = 5;
    private int gridY = 3;

    private int correctIndex;
    private List<Sprite> wrongSprites = new List<Sprite>();

    private List<(int x, int y)> selectedPos = new List<(int x, int y)>();
    private GameObject[] selectedPrefabs;

    void Start()
    {
        PuzzleOptions();
    }

    void PuzzleOptions()
    {
        SelectRandomPos();
        //CorrectPuzzle();
        //WrongPuzzles();
        ExPuzzleInst();
        CreateHole();
    }

    /// <summary>
    ///  이미지 랜덤 4개 위치 선택
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


    ///// <summary>
    /////  정답 조각 랜덤 위치 & Sprite 생성
    ///// </summary> 
    //public void CorrectPuzzle()
    //{
    //    int answerX = Random.Range(0, gridX);
    //    int answerY = Random.Range(0, gridY);

    //    correctSprite = CutSprite(answerX, answerY);
    //}

    ///// <summary>
    /////  오답 조각 랜덤 위치 크롭
    ///// </summary> 
    //void WrongPuzzles()
    //{
    //    wrongSprites.Clear();

    //    while (wrongSprites.Count < 3)
    //    {
    //        int x = Random.Range(0, gridX);
    //        int y = Random.Range(0, gridY);

    //        Sprite wrong = CutSprite(x, y);
    //        wrongSprites.Add(wrong);
    //    }
    //}

    /// <summary>
    ///  보기 퍼즐 4개 생성
    /// </summary>
    void ExPuzzleInst()
    {
        // 이전 퍼즐 제거
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }

        // 15개 중 4개 랜덤 선택
        selectedPrefabs = SelectRandomPrefabs();

        // 4개 퍼즐 생성
        for (int i = 0; i < 4; i++)
        {
            GameObject prefab = selectedPrefabs[i]; // 선택된 4개 퍼즐
            GameObject puzzleObj = Instantiate(prefab, puzzleParent);

            // UI Image 컴포넌트 찾기
            Image img = puzzleObj.GetComponent<Image>();
            if (img == null)
            {
                img = puzzleObj.GetComponentInChildren<Image>();
            }

            else
            {
                var pos = selectedPos[i];

                // 프리팹 모양대로 배경 이미지 자르기
                Sprite pieceSr = CutSprite(pos.x, pos.y, prefab);
                img.sprite = pieceSr;
                img.color = Color.white;
            }
        }

        // 퍼즐 조각들 일정 간격으로 배치
        ArrangePuzzle();
    }

    /// <summary>
    /// 퍼즐 조각 일정 간격 배치
    /// </summary>
    void ArrangePuzzle()
    {
        float spacingX = puzzleArea.rect.width / 2;
        float spacingY = puzzleArea.rect.height / 2;

        for (int i = 0; i < puzzleParent.childCount && i < 4; i++)
        {
            Transform child = puzzleParent.GetChild(i);
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect == null) continue;

            float x = (i % 2) * spacingX - spacingX / 2;
            float y = spacingY / 2 - (i / 2) * spacingY;

            childRect.anchoredPosition = new Vector2(x, y);
        }
    }


    /// <summary>
    ///  퍼즐 조각 영역만큼 이미지 잘라서 사용
    /// </summary>

    Sprite CutSprite(int tileX, int tileY, GameObject prefab)
    {
        Texture2D bgTex = bgImage.sprite.texture;

        int cellWidth = bgImage.sprite.texture.width / gridX;
        int cellHeight = bgImage.sprite.texture.height / gridY;

        int startX = tileX * cellWidth;
        int startY = tileY * cellHeight;

        // 프리팹 스프라이트 마스크로 사용
        Image prefabImg = prefab.GetComponent<Image>();

        Texture2D newTex = new Texture2D(cellWidth, cellHeight);

        if (prefabImg != null && prefabImg.sprite != null)
        {
            Texture2D maskTex = prefabImg.sprite.texture;

            for (int x = 0; x < cellWidth; x++)
            {
                for (int y = 0; y < cellHeight; y++)
                {
                    // 마스크 위치 계산, 알파값 확인
                    int maskX = x * maskTex.width / cellWidth;
                    int maskY = y * maskTex.height / cellHeight;

                    // 마스크 영역 안에 배경 이미지 복사
                    if (maskTex.GetPixel(maskX, maskY).a > 0.1f)
                    {
                        Color bgColor = bgTex.GetPixel(startX + x, startY + y);
                        newTex.SetPixel(x, y, bgColor);
                    }
                }
            }
        }

        /* 원본 배경 이미지에서 해당 영역 복사(기본 사각형 모양)
        for (int x = 0; x < cellWidth; x++)
        {
            for (int y = 0; y < cellHeight; y++)
            {
                Color color = originalTex.GetPixel(startX + x, startY + y);
                newTex.SetPixel(x, y, color);
            }
        } */
        newTex.Apply();
        return Sprite.Create(newTex, new Rect(0, 0, cellWidth, cellHeight), new Vector2(0.5f, 0.5f));
    }

    void CreateHole()
    {
        GameObject correctPrefab = selectedPrefabs[correctIndex];
        var correctPos = selectedPos[correctIndex];

        Texture2D bgTex = bgImage.sprite.texture;
        Texture2D newBgTex = new Texture2D(bgTex.width, bgTex.height);

        // 원본 배경 이미지 복사
        for (int x = 0; x < bgTex.width; x++)
        {
            for (int y = 0; y < bgTex.height; y++)
            {
                newBgTex.SetPixel(x, y, bgTex.GetPixel(x,y));
            }
        }

        // 정답 위치에 구멍 생성
        int cellWidth = bgTex.width / gridX;
        int cellHeight = bgTex.height / gridY;

        int startX = correctPos.x * cellWidth;
        int startY = correctPos.y * cellHeight;

        Image prefabImg = correctPrefab.GetComponent<Image>();
        Texture2D maskTex = prefabImg.sprite.texture;

        for (int x = 0; x < cellWidth; x++)
        {
            for (int y = 0; y < cellHeight; y++)
            {
                // 마스크 위치 계산, 알파값 확인
                int maskX = x * maskTex.width / cellWidth;
                int maskY = y * maskTex.height / cellHeight;

                // 검정색 설정
                if (maskTex.GetPixel(maskX, maskY).a > 0.1f)
                {
                    newBgTex.SetPixel(startX + x, startY + y, Color.black);
                }
            }
        }

        newBgTex.Apply();

        Sprite newBgSr = Sprite.Create(newBgTex, new Rect(0, 0, newBgTex.width, newBgTex.height), new Vector2(0.5f, 0.5f));
        bgImage.sprite = newBgSr;
    }
}
