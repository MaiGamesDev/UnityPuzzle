using Unity.VisualScripting;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public int[,] puzzleBoard = new int[4, 3];
    public GameObject puzzleCompleted;

    private void Start()
    {
        puzzleCompleted = GetComponent<GameObject>();
    }
    
    


}


