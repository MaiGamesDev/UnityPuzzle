using System.Collections;
using UnityEngine;

public class ExampleFourPuzzlesTransform : MonoBehaviour
{
    public Transform puzzleExParent;
    public RectTransform[] puzzleExRTs;
    public Vector2[] originPos;

    IEnumerator Start()
    {
        yield return null;

        int puzzleCount = 4; // 퍼즐 조각 개수
        puzzleExRTs = new RectTransform[puzzleCount];
        originPos = new Vector2[puzzleCount];

        for (int i = 0; i < puzzleCount; i++)
        {
            puzzleExParent = this.transform.GetChild(i);
            puzzleExRTs[i] = puzzleExParent.GetComponent<RectTransform>();

            originPos[i] = puzzleExRTs[i].anchoredPosition;
            //puzzleExRTs[i].sizeDelta = new Vector2(100, 100);

            if (puzzleExRTs[i] != null)
            {
                Debug.Log("퍼즐 위치(anchoredPosition): " + puzzleExRTs[i].anchoredPosition);
            }
        }
    }
}
