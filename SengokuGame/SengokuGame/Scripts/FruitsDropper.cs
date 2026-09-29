using System.Collections;
using System.Collections.Generic; //ここも追加必要
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using NUnit.Framework; //SE用に追加

public class FruitsDropper : MonoBehaviour
{



    //プレイヤーの操作方法（左右、果物落下）
    [SerializeField]
    private InputAction actionHorizontal;
    [SerializeField]
    private InputAction actionDrop;

    //2行追加
    public List<AudioResource> se;
    AudioSource audio;

    //果物のランダム呼び出し RandomFruitsSelector
    [SerializeField]
    private RandomFruitsSelector randomFruitsSelector;

    //左右の移動速度
    public float moveSpeed = 5.0f;

    [SerializeField]
    private float coolTime = 2.0f;   //SerializeFieldなのでいじれる！ 2.0fに修正
    private GameObject fruitsInstance;

    public bool isErasedByPlayer = false; // プレイヤーによって消されたかどうか 9/18追加




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();  //追加

        //ボタンを使えるよう許可
        actionHorizontal.Enable();
        actionDrop.Enable();

        //果物を接地する処理
        StartCoroutine(HandleFruits(coolTime));

        //ゲームオーバーになった場合フルーツを落とせなくする
        Fruits.OnGameOver.AddListener(() => enabled = false);
    }

    public void RespawnFruitAfterErase() //　9/18追加
    {
        if (fruitsInstance != null)
        {
            fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
            fruitsInstance.transform.SetParent(null);
            fruitsInstance = null;
        }

        StartCoroutine(HandleFruits(coolTime));
    }



    private IEnumerator HandleFruits(float delay)
    {
        //delayの間何もしない(クールタイム）
        yield return new WaitForSeconds(delay);
        var fruitsPrefab = randomFruitsSelector.Pop();
        //droperをいじった場合注意! ランダムな果物を生成
        fruitsInstance = Instantiate(fruitsPrefab, transform.position, Quaternion.identity, transform);
        //落下しないようにする処理
        fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        //移動 readvalue =ボタンを押したとき左右方向の値が入る
        float horizontal = actionHorizontal.ReadValue<float>() * moveSpeed * Time.deltaTime;
        //ドロッパーの上限下限をいじりたいときは最後2つの7.5fのところをいじる
        float x = Mathf.Clamp(transform.position.x + horizontal, -7.5f, 7.5f);
        //transform.position.xを代入
        transform.position = new Vector3(x, transform.position.y, transform.position.z);


        //Aボタンを押したとき
        if (actionDrop.WasPressedThisFrame() && fruitsInstance != null || isErasedByPlayer) //IsEraseedByPlayer追加
        {

            //物理挙動を許可(true)
            fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
            fruitsInstance.transform.SetParent(null);
            //インスタンスの情報を空に
            fruitsInstance = null;
            //一定時間後に、スポナーに果物をくっつける
            StartCoroutine(HandleFruits(coolTime));

            //ここ2行追加SEの追加
            audio.resource = se[0];
            audio.Play();


            // var fruitsPrefab = randomFruitsSelector.Pop();
            // Instantiate(fruitsPrefab, transform.position, transform.rotation); *この2行使わない
        }
    }
}
