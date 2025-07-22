using UnityEngine;

public class ExampleFourPuzzles : MonoBehaviour
{
    public RectTransform[] exPuzzles;

    private void Awake()
    {
        exPuzzles = new RectTransform[4];
    }
}
