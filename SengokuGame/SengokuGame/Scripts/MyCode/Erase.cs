using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class Erase : MonoBehaviour
{
    [SerializeField]
    private InputAction EraseFruits; //ボタンでフルーツ消す処理

    private GameObject fruitsInstance; //ここから５行一度フルーツを全消しした後再スポーンをする処理
    [SerializeField]
    private float coolTime = 2.0f;
    [SerializeField]
    private RandomFruitsSelector randomFruitsSelector;

    public GameObject eraseObj;

    int erasecount = 0;


    public string targetTag = "Fruits";

    private FruitsDropper fruitsDropper;// 9/18追加

    void Start()// 9/18追加
    {
        fruitsDropper = FindObjectOfType<FruitsDropper>();
    }





    void OnEnable()
    {
        EraseFruits.Enable();
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

    void Update()
    {
        if (EraseFruits.WasPressedThisFrame() && erasecount == 0)
        {
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(targetTag);
            erasecount++; //この処理でeraseを１回しか使えないようにする

            if (fruitsInstance != null)
            {
                fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
                fruitsInstance.transform.SetParent(null);
                fruitsInstance = null;
            }

            StartCoroutine(HandleFruits(coolTime));

            foreach (GameObject obj in objectsToDestroy)
            {
                Fruits fruit = obj.GetComponent<Fruits>();//ここから四行9/18追加
                if (fruit != null)
                {
                    fruit.isErasedByPlayer = true; // フラグを立てる
                }


                Destroy(obj);
            }

            // フルーツ再生成を呼び出す 9/18追加
            if (fruitsDropper != null)
            {
                fruitsDropper.RespawnFruitAfterErase();
            }


        }
    }


}

