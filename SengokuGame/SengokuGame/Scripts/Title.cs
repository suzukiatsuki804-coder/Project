using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    private InputAction action;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.Enable();  //ボタン操作で動くようにする
    }

    // Update is called once per frame
    void Update()
    {
           if (action.WasPressedThisFrame())//ifボタンを押したか
           {
               SceneManager.LoadScene("SampleScene");
           }

        
    }
}
