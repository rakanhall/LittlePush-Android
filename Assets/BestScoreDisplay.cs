using UnityEngine;
using TMPro;

public class BestScoreDisplay : MonoBehaviour
{
    public GameManager1 gameManager;
    private TextMeshPro textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        textMeshPro.text = bestScore.ToString();
    }
}



