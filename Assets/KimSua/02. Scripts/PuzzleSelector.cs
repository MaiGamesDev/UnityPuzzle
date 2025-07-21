using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSelector : MonoBehaviour, IPointerClickHandler
{
    public int puzzleIndex;
    private TurnButtonController turnButton;

    private void Awake()
    {
        turnButton = FindFirstObjectByType<TurnButtonController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        turnButton.SelectTile(puzzleIndex);
    }
}
