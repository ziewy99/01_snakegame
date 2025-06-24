using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultScoreText;

    void Start()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player != null && resultScoreText != null)
        {
            int score = player.score;
            Debug.Log("score");
            resultScoreText.text = "Score: " + score.ToString();
        }
    }
}