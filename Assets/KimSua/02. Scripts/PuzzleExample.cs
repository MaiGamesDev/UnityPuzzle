using System.Collections.Generic;
using System.Linq;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
    public Image bgImage; // ���� �̹���
    public GameObject[] puzzlePrefab; // ���� ������
    [SerializeField] private Transform puzzleParent; // ���� 4�� ��ġ�� �θ�

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
    ///  ��ü ���� ������ ����
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
    ///  ���� ���� ���� ��ġ & Sprite ����
    /// </summary> 
    public void CorrectPuzzle()
    {
        int answerX = Random.Range(0, gridX);
        int answerY = Random.Range(0, gridY);

        correctSprite = CutSprite(answerX, answerY, true);
    }

    /// <summary>
    ///  ���� ���� ���� ��ġ & Sprite ����
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
    /////  ���� + ���� Sprite ����
    ///// </summary>    
    //void ShufflePuzzle()
    //{
    //    choices.Clear();
    //    choices.Add((correctSprite, true));

    //    foreach (var wrong in wrongSprties)
    //    {
    //        choices.Add((wrong, false));
    //    }

    //    // ����
    //    for (int i = 0; i < choices.Count; i++)
    //    {
    //        int ran = Random.Range(i, choices.Count);
    //        var temp = choices[i];
    //        choices[i] = choices[ran];
    //        choices[ran] = temp;
    //    }

    //    Debug.Log($"���� �ε���: {correctIndex}");
    //}
        

    /// <summary>
    ///  ���� Sprite�� ���� ���� 4�� ����
    /// </summary>
    void ExPuzzleInst()
    {
        // ���� ���� ����
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }

        // ���� �ε��� ����(0~3 �� ������)
        correctIndex = Random.Range(0,4);
        Debug.Log($"���� �ε��� : {correctIndex}");

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
                wrongSprties.RemoveAt(0); // �ε��� ����
            }
        }
    }


    /// <summary>
    ///  ���� ���� ������ŭ �̹��� �߶� ���
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

                if (isAnswer) // ���� : �̹��� �κ� ����ó��
                {
                    newTex.SetPixel(x, y, Color.black);
                }

                else // ���� : �̹��� ���� �״�� ǥ��
                {
                    newTex.SetPixel(x, y, color);
                }
            }
        }
        newTex.Apply();
        return Sprite.Create(newTex, new Rect(0, 0, cellWidth, cellHeight), new Vector2(0.5f, 0.5f));
    }
}
