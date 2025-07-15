using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    public GameObject[] bgImagePrefab; // ¸íÈ­ ÇÁ¸®ÆÕ
    public GameObject[] puzzlePrefab; // ÆÛÁñ ÇÁ¸®ÆÕ
    [SerializeField] private Transform puzzleParent; // º¸±â 4°³ ¹èÄ¡ÇÒ ºÎ¸ð
=======
=======
>>>>>>> Stashed changes
    public Image bgImage; // ï¿½ï¿½ï¿½ï¿½ ï¿½Ì¹ï¿½ï¿½ï¿½
    public GameObject[] puzzlePrefab; // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    [SerializeField] private Transform puzzleParent; // ï¿½ï¿½ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½Î¸ï¿½
    [SerializeField] private RectTransform puzzleArea; // ï¿½ï¿½ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

    private int gridX = 5;
    private int gridY = 3;

    private int correctIndex;

    private List<(int x, int y)> selectedPos = new List<(int x, int y)>(); // 4°³(Á¤´ä1 + ¿À´ä3)

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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    ///  (Á¤´ä Æ÷ÇÔ)·£´ý 4°³ À§Ä¡ ¼±ÅÃ
=======
    ///  ï¿½Ì¹ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½Ä¡ ï¿½ï¿½ï¿½ï¿½
>>>>>>> Stashed changes
=======
    ///  ï¿½Ì¹ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½Ä¡ ï¿½ï¿½ï¿½ï¿½
>>>>>>> Stashed changes
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

        // ï¿½ï¿½ï¿½ï¿½ ï¿½Îµï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        correctIndex = Random.Range(0, 4);
        Debug.Log($"ï¿½ï¿½ï¿½ï¿½ ï¿½Îµï¿½ï¿½ï¿½ : {correctIndex}");
    }

    /// <summary>
    ///  15ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    /// </summary>  

    GameObject[] SelectRandomPrefabs()
    {
        List<GameObject> prefabs = new List<GameObject>(puzzlePrefab);
        GameObject[] selectedPrefabs = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Count);
            selectedPrefabs[i] = prefabs[randomIndex];
            prefabs.RemoveAt(randomIndex); // ï¿½ßºï¿½ ï¿½ï¿½ï¿½ï¿½ (ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½Îµï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½)
        }

        return selectedPrefabs;
    }


<<<<<<< Updated upstream
    /// <summary>
    ///  Á¤´ä Á¶°¢ ·£´ý À§Ä¡ & Sprite »ý¼º
    /// </summary> 
    Vector2 CorrectPuzzle()
    {
        var pos = selectedPos[correctIndex];
        return new Vector2(pos.x, pos.y);
    }
=======
    ///// <summary>
    /////  ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ & Sprite ï¿½ï¿½ï¿½ï¿½
    ///// </summary> 
    //public void CorrectPuzzle()
    //{
    //    int answerX = Random.Range(0, gridX);
    //    int answerY = Random.Range(0, gridY);
>>>>>>> Stashed changes

    /// <summary>
    ///  ¿À´ä Á¶°¢ ·£´ý À§Ä¡ Å©·Ó
    /// </summary> 
    List<Vector2> WrongPuzzles()
    {
        List<Vector2> wrongs = new List<Vector2>();

<<<<<<< Updated upstream
        for (int i = 0; i < selectedPos.Count; i++)
        {
            if (i == correctIndex) continue;
            wrongs.Add(new Vector2(selectedPos[i].x, selectedPos[i].y));
        }
=======
    ///// <summary>
    /////  ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ Å©ï¿½ï¿½
    ///// </summary> 
    //void WrongPuzzles()
    //{
    //    wrongSprites.Clear();
>>>>>>> Stashed changes

        return wrongs;
    }

    /// <summary>
    ///  ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    /// </summary>
    void ExPuzzleInst()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        // ÆÛÁñ ÃÊ±âÈ­
=======
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
>>>>>>> Stashed changes
=======
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
>>>>>>> Stashed changes
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        // ·£´ý ¸íÈ­ 1°³ ¼±ÅÃ
        GameObject bgRandom = bgImagePrefab[Random.Range(0, bgImagePrefab.Length)];

        float fullWidth = 650f;
        float fullHeight = 800f;
        float cellWidth = fullWidth / gridX;
        float cellHeight = fullHeight / gridY;

        // ÆÛÁñ ¸ð¾ç 15°³ Áß 4°³ ·£´ý ¼±ÅÃ
        GameObject[] selectedPrefabs = SelectRandomPrefabs();

        Vector2 correctPos = CorrectPuzzle(); // Á¤´ä À§Ä¡
        List<Vector2> wrongPosList = WrongPuzzles(); // ¿À´ä À§Ä¡µé
=======
        // 15ï¿½ï¿½ ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        selectedPrefabs = SelectRandomPrefabs();
>>>>>>> Stashed changes

        // 4ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        for (int i = 0; i < 4; i++)
        {
<<<<<<< Updated upstream
            Vector2 pos;
            if (i == correctIndex)
=======
=======
        // 15ï¿½ï¿½ ï¿½ï¿½ 4ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        selectedPrefabs = SelectRandomPrefabs();

        // 4ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        for (int i = 0; i < 4; i++)
        {
>>>>>>> Stashed changes
            GameObject prefab = selectedPrefabs[i]; // ï¿½ï¿½ï¿½Ãµï¿½ 4ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            GameObject puzzleObj = Instantiate(prefab, puzzleParent);

            // UI Image ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ® Ã£ï¿½ï¿½
            Image img = puzzleObj.GetComponent<Image>();
            if (img == null)
>>>>>>> Stashed changes
            {
                pos = correctPos;
            }
            else
            {
<<<<<<< Updated upstream
                if (i < correctIndex)
                {
                    pos = wrongPosList[i];
                }
                else
                {
                    pos = wrongPosList[i - 1];
=======
                var pos = selectedPos[i];

                // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ ï¿½Ì¹ï¿½ï¿½ï¿½ ï¿½Ú¸ï¿½ï¿½ï¿½
                Sprite pieceSr = CutSprite(pos.x, pos.y, prefab);
                img.sprite = pieceSr;
                img.color = Color.white;
            }
        }

        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡
        ArrangePuzzle();
    }

    /// <summary>
    /// ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡
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
    ///  ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Å­ ï¿½Ì¹ï¿½ï¿½ï¿½ ï¿½ß¶ï¿½ ï¿½ï¿½ï¿½
    /// </summary>

    Sprite CutSprite(int tileX, int tileY, GameObject prefab)
    {
        Texture2D bgTex = bgImage.sprite.texture;

        int cellWidth = bgImage.sprite.texture.width / gridX;
        int cellHeight = bgImage.sprite.texture.height / gridY;

        int startX = tileX * cellWidth;
        int startY = tileY * cellHeight;

        // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ® ï¿½ï¿½ï¿½ï¿½Å©ï¿½ï¿½ ï¿½ï¿½ï¿½
        Image prefabImg = prefab.GetComponent<Image>();

        Texture2D newTex = new Texture2D(cellWidth, cellHeight);

        if (prefabImg != null && prefabImg.sprite != null)
        {
            Texture2D maskTex = prefabImg.sprite.texture;

            for (int x = 0; x < cellWidth; x++)
            {
                for (int y = 0; y < cellHeight; y++)
                {
                    // ï¿½ï¿½ï¿½ï¿½Å© ï¿½ï¿½Ä¡ ï¿½ï¿½ï¿½, ï¿½ï¿½ï¿½Ä°ï¿½ È®ï¿½ï¿½
                    int maskX = x * maskTex.width / cellWidth;
                    int maskY = y * maskTex.height / cellHeight;

                    // ï¿½ï¿½ï¿½ï¿½Å© ï¿½ï¿½ï¿½ï¿½ ï¿½È¿ï¿½ ï¿½ï¿½ï¿½ ï¿½Ì¹ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
                    if (maskTex.GetPixel(maskX, maskY).a > 0.1f)
                    {
                        Color bgColor = bgTex.GetPixel(startX + x, startY + y);
                        newTex.SetPixel(x, y, bgColor);
                    }
>>>>>>> Stashed changes
                }
            }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
            // ÆÛÁñ ¿ÜÇü ÇÁ¸®ÆÕ (Mask¿ë)
            GameObject puzzleObj = Instantiate(selectedPrefabs[i], puzzleParent);
            Image puzzleImg = puzzleObj.GetComponent<Image>();
            puzzleImg.SetNativeSize();

            GameObject childImg = Instantiate(bgRandom, puzzleObj.transform);
            Image img = childImg.GetComponent<Image>();
            RectTransform bgRect = img.GetComponent<RectTransform>();

            if (img != null)
=======
=======
>>>>>>> Stashed changes
        /* ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ ï¿½Ì¹ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ø´ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½(ï¿½âº» ï¿½ç°¢ï¿½ï¿½ ï¿½ï¿½ï¿½)
        for (int x = 0; x < cellWidth; x++)
        {
            for (int y = 0; y < cellHeight; y++)
>>>>>>> Stashed changes
            {
                img.raycastTarget = false;

                // Å©±â °íÁ¤ ¼³Á¤
                bgRect.anchorMin = new Vector2(0.5f, 0.5f);
                bgRect.anchorMax = new Vector2(0.5f, 0.5f);
                bgRect.pivot = new Vector2(0.5f, 0.5f);
                bgRect.sizeDelta = new Vector2(fullWidth, fullHeight);

                // ¸íÈ­ À§Ä¡ ÀÌµ¿
                //float offsetX = -(cellWidth * (gridX - 1) / 2f) + pos.x * cellWidth;
                //float offsetY = (cellHeight * (gridY - 1) / 2f) + pos.y * cellHeight;
                //bgRect.anchoredPosition = new Vector2(offsetX, offsetY);

<<<<<<< Updated upstream
            }
<<<<<<< Updated upstream
        }
=======
=======
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

>>>>>>> Stashed changes
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ ï¿½Ì¹ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        for (int x = 0; x < bgTex.width; x++)
        {
            for (int y = 0; y < bgTex.height; y++)
            {
                newBgTex.SetPixel(x, y, bgTex.GetPixel(x,y));
            }
        }

        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
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
                // ï¿½ï¿½ï¿½ï¿½Å© ï¿½ï¿½Ä¡ ï¿½ï¿½ï¿½, ï¿½ï¿½ï¿½Ä°ï¿½ È®ï¿½ï¿½
                int maskX = x * maskTex.width / cellWidth;
                int maskY = y * maskTex.height / cellHeight;

                // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
                if (maskTex.GetPixel(maskX, maskY).a > 0.1f)
                {
                    newBgTex.SetPixel(startX + x, startY + y, Color.black);
                }
            }
        }

        newBgTex.Apply();

        Sprite newBgSr = Sprite.Create(newBgTex, new Rect(0, 0, newBgTex.width, newBgTex.height), new Vector2(0.5f, 0.5f));
        bgImage.sprite = newBgSr;
>>>>>>> Stashed changes
    }
}
