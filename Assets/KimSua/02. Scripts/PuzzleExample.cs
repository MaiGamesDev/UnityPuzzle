using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleExample : MonoBehaviour
{
    public GameObject[] bgImagePrefab; // ��ȭ ������
    public GameObject[] puzzlePrefab; // ���� ������
    [SerializeField] private Transform puzzleParent; // ���� 4�� ��ġ�� �θ�

    private int gridX = 5;
    private int gridY = 3;

    private int correctIndex;

    private List<(int x, int y)> selectedPos = new List<(int x, int y)>(); // 4��(����1 + ����3)

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

        // ���� �ε��� ����
        correctIndex = Random.Range(0, 4);
        Debug.Log($"���� �ε��� : {correctIndex}");
    }

    /// <summary>
    ///  15�� ������ �� ���� ����
    /// </summary>  

    GameObject[] SelectRandomPrefabs()
    {
        List<GameObject> prefabs = new List<GameObject>(puzzlePrefab);
        GameObject[] selectedPrefabs = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Count);
            selectedPrefabs[i] = prefabs[randomIndex];
            prefabs.RemoveAt(randomIndex); // �ߺ� ���� (��ġ�� �ε����� ����)
        }

        return selectedPrefabs;
    }
    /// <summary>
    ///  ���� ���� ���� ��ġ & Sprite ����
    /// </summary> 
    Vector2 CorrectPuzzle()
    {
        var pos = selectedPos[correctIndex];
        return new Vector2(pos.x, pos.y);
    }

    /// <summary>
    ///  ���� ���� ���� ��ġ ũ��
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
    ///  ���� ���� 4�� ����
    /// </summary>
    void ExPuzzleInst()
    {
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }


        // ���� ��ȭ 1�� ����
        GameObject bgRandom = bgImagePrefab[Random.Range(0, bgImagePrefab.Length)];

        float fullWidth = 650f;
        float fullHeight = 800f;
        float cellWidth = fullWidth / gridX;
        float cellHeight = fullHeight / gridY;

        // ���� ��� 15�� �� 4�� ���� ����
        GameObject[] selectedPrefabs = SelectRandomPrefabs();

        Vector2 correctPos = CorrectPuzzle(); // ���� ��ġ
        List<Vector2> wrongPosList = WrongPuzzles(); // ���� ��ġ��

        // 15�� �� 4�� ���� ����
        selectedPrefabs = SelectRandomPrefabs();


        // (07-16)

        // 4�� ���� ����
        //for (int i = 0; i < 4; i++)
        //{

        //Vector2 pos;
        //if (i == correctIndex)

        //    GameObject prefab = selectedPrefabs[i]; // ���õ� 4�� ����
        //GameObject puzzleObj = Instantiate(prefab, puzzleParent);

        //// UI Image ������Ʈ ã��
        //Image img = puzzleObj.GetComponent<Image>();
        //if (img == null)

        //{
        //    pos = correctPos;
        //}
        //else
        //{

        //    if (i < correctIndex)
        //    {
        //        pos = wrongPosList[i];
        //    }
        //    else
        //    {
        //        pos = wrongPosList[i - 1];

        //        var pos = selectedPos[i];

        //        // ������ ����� ��� �̹��� �ڸ���
        //        Sprite pieceSr = CutSprite(pos.x, pos.y, prefab);
        //        img.sprite = pieceSr;
        //        img.color = Color.white;
        //    }
        //}

        // ���� ������ ���� �������� ��ġ
        ArrangePuzzle();
    }

    // (07-16)

    /// <summary>
    /// ���� ���� ���� ���� ��ġ
    /// </summary>
    void ArrangePuzzle()
    {
        //float spacingX = puzzleArea.rect.width / 2;
        //float spacingY = puzzleArea.rect.height / 2;

        for (int i = 0; i < puzzleParent.childCount && i < 4; i++)
        {
            Transform child = puzzleParent.GetChild(i);
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect == null) continue;

            //float x = (i % 2) * spacingX - spacingX / 2;
            //float y = spacingY / 2 - (i / 2) * spacingY;

            //childRect.anchoredPosition = new Vector2(x, y);
        }
    }
}


    // (07-16)

    /// <summary>
    ///  ���� ���� ������ŭ �̹��� �߶� ���
    /// </summary>

    //Sprite CutSprite(int tileX, int tileY, GameObject prefab)
    //{
    //    Texture2D bgTex = bgImage.sprite.texture;

    //    int cellWidth = bgImage.sprite.texture.width / gridX;
    //    int cellHeight = bgImage.sprite.texture.height / gridY;

    //    int startX = tileX * cellWidth;
    //    int startY = tileY * cellHeight;

    //    // ������ ��������Ʈ ����ũ�� ���
    //    Image prefabImg = prefab.GetComponent<Image>();

    //    Texture2D newTex = new Texture2D(cellWidth, cellHeight);

    //    if (prefabImg != null && prefabImg.sprite != null)
    //    {
    //        Texture2D maskTex = prefabImg.sprite.texture;

    //        for (int x = 0; x < cellWidth; x++)
    //        {
    //            for (int y = 0; y < cellHeight; y++)
    //            {
    //                // ����ũ ��ġ ���, ���İ� Ȯ��
    //                int maskX = x * maskTex.width / cellWidth;
    //                int maskY = y * maskTex.height / cellHeight;

    //                // ����ũ ���� �ȿ� ��� �̹��� ����
    //                if (maskTex.GetPixel(maskX, maskY).a > 0.1f)
    //                {
    //                    Color bgColor = bgTex.GetPixel(startX + x, startY + y);
    //                    newTex.SetPixel(x, y, bgColor);
    //                }
    //            }
    //        }



    // (07-16)

            // ���� ���� ������ (Mask��)
            //GameObject puzzleObj = Instantiate(selectedPrefabs[i], puzzleParent);
            //Image puzzleImg = puzzleObj.GetComponent<Image>();
            //puzzleImg.SetNativeSize();

            //GameObject childImg = Instantiate(bgRandom, puzzleObj.transform);
            //Image img = childImg.GetComponent<Image>();
            //RectTransform bgRect = img.GetComponent<RectTransform>();

            //if (img != null)
            //{
            //    img.raycastTarget = false;

            //    // ũ�� ���� ����
            //    bgRect.anchorMin = new Vector2(0.5f, 0.5f);
            //    bgRect.anchorMax = new Vector2(0.5f, 0.5f);
            //    bgRect.pivot = new Vector2(0.5f, 0.5f);
            //    bgRect.sizeDelta = new Vector2(fullWidth, fullHeight);

                // ��ȭ ��ġ �̵�
                //float offsetX = -(cellWidth * (gridX - 1) / 2f) + pos.x * cellWidth;
                //float offsetY = (cellHeight * (gridY - 1) / 2f) + pos.y * cellHeight;
                //bgRect.anchoredPosition = new Vector2(offsetX, offsetY);

    // (07-16)


//        // ���� ��� �̹��� ����
//        for (int x = 0; x < bgTex.width; x++)
//        {
//            for (int y = 0; y < bgTex.height; y++)
//            {
//                newBgTex.SetPixel(x, y, bgTex.GetPixel(x,y));
//            }
//        }

//        // ���� ��ġ�� ���� ����
//        int cellWidth = bgTex.width / gridX;
//        int cellHeight = bgTex.height / gridY;

//        int startX = correctPos.x * cellWidth;
//        int startY = correctPos.y * cellHeight;

//        Image prefabImg = correctPrefab.GetComponent<Image>();
//        Texture2D maskTex = prefabImg.sprite.texture;

//        for (int x = 0; x < cellWidth; x++)
//        {
//            for (int y = 0; y < cellHeight; y++)
//            {
//                // ����ũ ��ġ ���, ���İ� Ȯ��
//                int maskX = x * maskTex.width / cellWidth;
//                int maskY = y * maskTex.height / cellHeight;

//                // ������ ����
//                if (maskTex.GetPixel(maskX, maskY).a > 0.1f)
//                {
//                    newBgTex.SetPixel(startX + x, startY + y, Color.black);
//                }
//            }
//        }

//        newBgTex.Apply();

//        Sprite newBgSr = Sprite.Create(newBgTex, new Rect(0, 0, newBgTex.width, newBgTex.height), new Vector2(0.5f, 0.5f));
//        bgImage.sprite = newBgSr;
//    }
//}
