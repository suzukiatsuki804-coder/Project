using System.Collections;
using System.Collections.Generic;  //add
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio; //add
using UnityEngine.InputSystem;//追加（ゲームオーバー　タイトル遷移）
using UnityEngine.SceneManagement;//上に同じく

public enum FRUITS_TYPE //列挙型
{
    いまがわ = 1,
    あさくら,
    あしかが,
    たけだ,
    おだ,
    あけち,
    もうり,
    ほうじょう,
    とよとみ,
    いしだ,
    とくがわ
}




public class Fruits : MonoBehaviour
{



    public FRUITS_TYPE fruitsType;
    public bool isDestroyed = false;
    //進化先を追加
    [SerializeField] private GameObject nextFruitsPrefab;
    [SerializeField] private int score;

    public static UnityEvent<int> OnScoreAdded = new UnityEvent<int>();

    //ゲームオーバー処理
    public static UnityEvent OnGameOver = new UnityEvent();
    private bool isInside = false;
    private bool isGameOver = false;  //追加

    public bool isErasedByPlayer = false; // プレイヤーによって消されたかどうか 9/18追加


    //二行追加
    public List<AudioResource> se;
    AudioSource audio;

    [SerializeField]
    private InputAction action;  //追加

    private GameObject field; //9/17追加
    private Field fieldScript;

    private void Awake()
    {
        audio = GetComponent<AudioSource>(); //追加

        field = GameObject.Find("Field"); //Fieldプレハブを探す時このような文が必要
        if ((field != null))
        {
            fieldScript = field.GetComponent<Field>();
        }


        OnGameOver.AddListener(() =>
        {
            isGameOver = true;  //追加
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;

            rb.velocity = new Vector2(0.0f, 0.0f);
            rb.angularVelocity = 0.0f; //フルーツを動かないようにする(速度、回転速度をを０にする)

            //BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            // boxCollider.enabled = false;  この2行使わない
        });



    }



    IEnumerator Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (rb.isKinematic)
        {
            yield return null;
        }
        yield return new WaitForSeconds(8.0f);  //ここを大きくするとチェックが遅れる
        if (!isInside && isGameOver)
        {
            OnGameOver.Invoke(); //isinsideにフルーツがない場合、ゲームオーバー処理をする。

            StartCoroutine(EndGame());//追加　タイトル遷移をIEnumeratorで分けてわかりやすくした

        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(4.0f);  //追加(ランキング画面に遷移）
        SceneManager.LoadScene("RankingScene");
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        isInside = true;

    }
    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (isDestroyed || isErasedByPlayer) // 9/18isEraseByPlayer追加
        {
            return;
        }
        OnGameOver.Invoke();
        //追加
        audio.resource = se[1];
        audio.Play();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        audio = GetComponent<AudioSource>();

        if (isDestroyed || isGameOver)
        {
            return;
        }
        //当たったゲームオブジェクトがFruitsスクリプトを持っているかチェック
        Fruits script = collision.gameObject.GetComponent<Fruits>();

        if (script != null)　　　//フルーツスクリプトを持っているか
        {
            //当たったゲームオブジェクトが同じフルーツかチェック
            if (script.fruitsType == fruitsType)
            {
                OnScoreAdded.Invoke(score);


                isDestroyed = true;
                Destroy(gameObject);　　//オブジェクトの削除
                //接触相手を消す
                script.isDestroyed = true;
                Destroy(script.gameObject);


                if (nextFruitsPrefab == null)
                {
                    return;
                }

                //進化先を生成
                Vector3 center = (transform.position + script.transform.position) / 2;
                Quaternion rotation = Quaternion.Lerp(transform.rotation, script.transform.rotation, 0.5f);
                var next = Instantiate(nextFruitsPrefab, center, rotation);

                //移動速度の平均を設定
                Rigidbody2D nextRb = next.GetComponent<Rigidbody2D>();
                Vector3 velocity = GetComponent<Rigidbody2D>().velocity + script.gameObject.GetComponent<Rigidbody2D>().velocity / 2;
                nextRb.velocity = velocity;
                //回転速度の平均を設定
                float angularVelocity = GetComponent<Rigidbody2D>().angularVelocity + script.gameObject.GetComponent<Rigidbody2D>().angularVelocity / 2;

                //ここ2行追加SEの追加(今のところ不使用、Displayscoreで鳴らすようにしている
                //audio.resource = se[0];
                //audio.Play();
            }

        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;  //重力を０に

        float angle = 270.0f + fieldScript.GetFieldAngle(); //下に落とすため２７０をプラス

        float radians = Mathf.Deg2Rad * angle;
        Vector2 gravityDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        rb.AddForce(gravityDirection * 9.81f * rb.mass); //力を加える
    }


}

