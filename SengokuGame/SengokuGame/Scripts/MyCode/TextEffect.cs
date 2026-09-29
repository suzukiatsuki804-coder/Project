using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public  float speed = 1.0f;
    float time;
    TMP_Text text;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.color = GetAlphaColor(text.color);
    }

    Color GetAlphaColor(Color color)//Ç±Ç±Ç≈ì_ñ≈èàóùÇ≥ÇπÇÈ
    {
        time += Time.deltaTime * 3.5f * speed;
        color.a = Mathf.Sin(time);

        return color;
    }
}
