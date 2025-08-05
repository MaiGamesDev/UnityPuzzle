using UnityEngine;
using UnityEngine.Rendering;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int totalScore { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    public void AddScore(int score)
    {
        totalScore += score;
    }
}
