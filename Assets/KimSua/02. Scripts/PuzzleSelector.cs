using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSelector : MonoBehaviour, IPointerDownHandler
{
    public int puzzleIndex;
    private TurnButtonController turnButton;

    private void Awake()
    {
        turnButton = FindFirstObjectByType<TurnButtonController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        turnButton.SelectTile(puzzleIndex);
    }
}
