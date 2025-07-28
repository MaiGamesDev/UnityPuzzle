using UnityEngine;

public class TurnButtonController : MonoBehaviour
{
    // public PuzzleController puzzleController;
    public GameObject seletedTile;
    public GameObject[] puzzles;
    [SerializeField] private Transform puzzleParent;

    private int selectedIndex = 0;

    void Start()
    {
        SelectTile(0);
        RandomPuzzle();
    }

    // �׽�Ʈ��
    public void RandomPuzzle()
    {
        var randomIndex = Random.Range(0, puzzles.Length);
        var randomX = Random.Range(0, 4);
        var randomY = Random.Range(0, 2);
        var createPos = new Vector3(randomX, randomY, 0);

        var newTile = Instantiate(puzzles[randomIndex], puzzleParent);
        Debug.Log($"������ ���� ����: {newTile.name}");

        seletedTile = newTile;
    }

    public void SelectTile(int index)
    {
        //  seletedTile = puzzleController.candidateTiles[index]; // ���� 4������ �ε��� ������


        // selectedIndex = index;
    }

    public void RotateLeft()
    {
        if (seletedTile != null)
        {
            Debug.Log("Rotate Left");
            seletedTile.transform.Rotate(0, 0, 90f); // �ݽð� ����
        }
            
    }

    public void RotateRight()
    {
        if (seletedTile != null)
        {
            Debug.Log("Rotate Right");
            seletedTile.transform.Rotate(0, 0, -90f); // �ð� ����
        }            
    }
}
