using TMPro;
using UnityEngine;

public class RankingText : MonoBehaviour
{
    public int rank;
    public TextMeshProUGUI textScore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.RankingData data = GameManager.GetRankingData(rank);
        textScore.text = data.score.ToString("D"); //ƒf[ƒ^‚ğó‚¯æ‚é
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
