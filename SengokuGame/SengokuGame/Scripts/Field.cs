using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Field : MonoBehaviour
{
    [SerializeField]
    private InputAction inputAngle;
    private Vector3 offSet = new Vector3(0.0f, 0.0f, 0.0f); 

   //線形補間の働き（戻す処理を一定にする）
    private Quaternion startRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    private Quaternion defaultRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    public float rotationSpeed = 1.0f; //傾くスピード
    private float time = 0.0f;　　　　　//線形保管用の変数

    public GameObject script;
 
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        inputAngle.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
      
       
    }

    public float GetFieldAngle()
    {
        float angle = transform.rotation.eulerAngles.z;
        return angle; 
    }
}
