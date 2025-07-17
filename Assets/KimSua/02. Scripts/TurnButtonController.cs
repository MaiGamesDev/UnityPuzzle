using UnityEngine;
using UnityEngine.UI;

public class TurnButtonController : MonoBehaviour
{
    private PuzzleExample puzzleEx;
    public GameObject selectedTile;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private enum RotateDir { Left, Right };

    private void Awake()
    {
        puzzleEx = FindFirstObjectByType<PuzzleExample>();

        //leftButton.onClick.AddListener(() => Rotate("left"));
        //rightButton.onClick.AddListener(() => Rotate("right"));
    }

    void Start()
    {
        SelectTile(0);
    }    

    public void SelectTile(int index)
    {
        if (index >= 0 && puzzleEx.tiles != null)
        {
            selectedTile = puzzleEx.tiles[index];
            Debug.Log($"타일{index} 선택됨. 회전 대기.");
        }               
    }

    private void Rotate(RotateDir dir)
    {
        float angle = (dir == RotateDir.Left) ? 90f : -90f;
        selectedTile.transform.Rotate(0, 0, angle);
        Debug.Log($"Rotated {dir} 실행");
    }
}
