using UnityEngine;
using UnityEngine.UI;

public class PuzzleImageUtil
{
    private int gridX;
    private int gridY;

    // 크기 고정 설정
    float fullWidth;
    float fullHeight;
    float cellWidth;
    float cellHeight;

    public PuzzleImageUtil(int gridX, int gridY, float fullWidth, float fullHeight)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.fullWidth = fullWidth;
        this.fullHeight = fullHeight;

        cellWidth = fullWidth / gridX;
        cellHeight = fullHeight / gridY;
    }

    /// <summary>
    /// 퍼즐 조각에 특정 위치 조각 붙임
    /// </summary>
    public void CutBgSprite(GameObject bgPrefab, Transform parent, Vector2 gridPos)
    {
        if (bgPrefab == null || parent == null) return;

        GameObject childImg = GameObject.Instantiate(bgPrefab);
        childImg.transform.SetParent(parent, false); // 부모 설정

        Image img = childImg.GetComponent<Image>();
        RectTransform bgRect = img.GetComponent<RectTransform>();
        img.raycastTarget = false;

        bgRect.anchorMin = new Vector2(0.5f, 0.5f);
        bgRect.anchorMax = new Vector2(0.5f, 0.5f);
        bgRect.pivot = new Vector2(0.5f, 0.5f);
        bgRect.sizeDelta = new Vector2(fullWidth, fullHeight);

        float totalOffsetX = (fullWidth - cellWidth) / 2f;
        float totalOffsetY = (fullHeight - cellHeight) / 2f;

        float offsetX = totalOffsetX - (gridPos.x * cellWidth);
        float offsetY = totalOffsetY - (gridPos.y * cellHeight);

        bgRect.anchoredPosition = new Vector2(offsetX, offsetY);
    }
}
