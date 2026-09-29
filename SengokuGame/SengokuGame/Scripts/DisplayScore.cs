using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;
    public GameObject HanabiEffect;  //花火エフェクト追加

    //2行追加
    public List<AudioResource> se;
    AudioSource audio;

    public GameObject gameOverPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();//add

        Fruits.OnScoreAdded.AddListener(AddScore);
        AddScore(0);

        Fruits.OnGameOver.AddListener(() => Debug.Log("Game Over"));

        //OnGameOverになった場合 GameOverPanelを表示
        gameOverPanel.SetActive(false);
        Fruits.OnGameOver.AddListener(() => gameOverPanel.SetActive(true));
        HanabiEffect.SetActive(false);
        Fruits.OnGameOver.AddListener(() => HanabiEffect.SetActive(true));  //二行追加
    }

    private void AddScore(int score)
    {
        //今のスコアにくっつけた果物のスコアを加算し、表示させる
        this.score += score;
        scoreText.text = this.score.ToString();

        if(score > 0)
        {
            audio.resource = se[0];
            audio.Play();
        }
    }

    public int GetScore()
    {
        return this.score; //自分自身のスコアを渡す
    }

    
}
