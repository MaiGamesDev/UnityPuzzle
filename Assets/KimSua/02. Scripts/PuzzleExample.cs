using System.Collections.Generic;
using System.Linq;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
    public Image bgImage; // 원본 이미지
    public GameObject[] puzzlePrefab; // 퍼즐 프리팹
    [SerializeField] private Transform puzzleParent; // 보기 4개 배치할 부모

    private int gridX = 5;
    private int gridY = 3;

    private int correctIndex;
    private Sprite correctSprite;

    private List<Sprite> wrongSprties = new List<Sprite>();
    private List<(Sprite sprite, bool isAnswer)> choices = new List<(Sprite, bool)>();

    void Start()
    {
        PuzzleOptions();
    }

    void PuzzleOptions()
    {
        CorrectPuzzle();
        WrongPuzzles();
        ShuffleList();
        ExPuzzleInst();
    }

    /// <summary>
    ///  전체 퍼즐 프리팹 섞기
    /// </summary>
    void ShuffleList()
    {
        for (int i = 0; i < puzzlePrefab.Length; i++)
        {
            int ranA = Random.Range(0, puzzlePrefab.Length);
            int ranB = Random.Range(0, puzzlePrefab.Length);

            var temp = puzzlePrefab[ranA];
            puzzlePrefab[ranA] = puzzlePrefab[ranB];
            puzzlePrefab[ranB] = temp;
        }
    }

    /// <summary>
    ///  정답 조각 랜덤 위치 & Sprite 생성
    /// </summary> 
    public void CorrectPuzzle()
    {
        int answerX = Random.Range(0, gridX);
        int answerY = Random.Range(0, gridY);

        correctSprite = CutSprite(answerX, answerY, true);
    }

    /// <summary>
    ///  오답 조각 랜덤 위치 & Sprite 생성
    /// </summary> 
    void WrongPuzzles()
    {
        wrongSprties.Clear();

        while (wrongSprties.Count < 3)
        {
            int x = Random.Range(0, gridX);
            int y = Random.Range(0, gridY);
            Vector2 wrongPos = new Vector2(x, y);

            Sprite wrong = CutSprite(x, y, false);
            wrongSprties.Add(wrong);
        }
    }

    ///// <summary>
    /////  정답 + 오답 Sprite 섞기
    ///// </summary>    
    //void ShufflePuzzle()
    //{
    //    choices.Clear();
    //    choices.Add((correctSprite, true));

    //    foreach (var wrong in wrongSprties)
    //    {
    //        choices.Add((wrong, false));
    //    }

    //    // 섞기
    //    for (int i = 0; i < choices.Count; i++)
    //    {
    //        int ran = Random.Range(i, choices.Count);
    //        var temp = choices[i];
    //        choices[i] = choices[ran];
    //        choices[ran] = temp;
    //    }

    //    Debug.Log($"정답 인덱스: {correctIndex}");
    //}
        

    /// <summary>
    ///  섞인 Sprite로 보기 퍼즐 4개 생성
    /// </summary>
    void ExPuzzleInst()
    {
        // 이전 퍼즐 제거
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }

        // 정답 인덱스 결정(0~3 중 무작위)
        correctIndex = Random.Range(0,4);
        Debug.Log($"정답 인덱스 : {correctIndex}");

        for (int i = 0; i < 4; i++)
        {
            GameObject prefab = puzzlePrefab[i];
            GameObject puzzleObj = Instantiate(prefab, puzzleParent);
            SpriteRenderer sr = puzzleObj.GetComponent<SpriteRenderer>();
            
            if (i == correctIndex)
            {
                sr.sprite = correctSprite;
            }
            else
            {
                sr.sprite = wrongSprties[0];
                wrongSprties.RemoveAt(0); // 인덱스 제거
            }
        }
    }


    /// <summary>
    ///  퍼즐 조각 영역만큼 이미지 잘라서 사용
    /// </summary>

    Sprite CutSprite(int tileX, int tileY, bool isAnswer)
    {
        int cellWidth = bgImage.sprite.texture.width / gridX;
        int cellHeight = bgImage.sprite.texture.height / gridY;

        int startX = tileX * cellWidth;
        int startY = tileY * cellHeight;

        Texture2D newTex = new Texture2D(cellWidth, cellHeight);

        for (int x = 0; x < cellWidth; x++)
        {
            for (int y = 0; y < cellHeight; y++)
            {
                Color color = bgImage.sprite.texture.GetPixel(startX + x, startY + y);

                if (isAnswer) // 정답 : 이미지 부분 검정처리
                {
                    newTex.SetPixel(x, y, Color.black);
                }

                else // 오답 : 이미지 영역 그대로 표시
                {
                    newTex.SetPixel(x, y, color);
                }
            }
        }
        newTex.Apply();
        return Sprite.Create(newTex, new Rect(0, 0, cellWidth, cellHeight), new Vector2(0.5f, 0.5f));
    }
}
