using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct RankingData {
        public int score;               // 表示するスコア
    }
    [System.Serializable]
    public struct SaveData {
        public List<RankingData> rank;  // ランク
    }
           //静的（シーンをまたいでもデータ残る）
    public static int playerScore = 0;
    // ランキング情報
    private static List<RankingData> rankingDataList = new List<RankingData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 保存したランキングデータのロード
        Load();

        Fruits.OnGameOver.RemoveAllListeners(); // すべてのリスナーを削除 ここに追加した
        Fruits.OnGameOver.AddListener(() =>
        {
            // ここでランキング集計
            CalcRankingData();
            // ここで保存
            Save();
            enabled = false;
        });

    }

    // Update is called once per frame
    void Update()
    {
        // score情報を取得
        GameObject obj = GameObject.Find("ScoreFrame");
        DisplayScore displayScore = obj.GetComponent<DisplayScore>();
        if (displayScore != null)
        {
            playerScore = displayScore.GetScore();
        }

    }
    void CalcRankingData()
    {
        RankingData playerData = new RankingData();
        playerData.score = playerScore;

        bool insertFlag = false;

        for(int i = 0; i < rankingDataList.Count; i++)
        {
            if(playerData.score > rankingDataList[i].score)
            {
                rankingDataList.Insert(i, playerData);
                insertFlag = true;
                break; //挿入出来たらループを終わらせる
            }
        }

        if(insertFlag == false)//最低点だった場合の処理
        {
            rankingDataList.Add(playerData);

        }
    }
    public static RankingData GetRankingData(int num)  //順位とデータのひもづけ
    {
        // リストの範囲外を指定されたらダミーを返す
        if (num - 1 >= rankingDataList.Count || rankingDataList.Count == 0)
        {
            // ダミーデータを作成
            RankingData dummy = new RankingData();
            // とりあえずスコアを0にする
            dummy.score = 0;
            return dummy;
        }
        // データがあったらその構造体を返す else
        return rankingDataList[num - 1];
    }
    void Save()
    {
        // セーブデータの構造体を作る
        SaveData data = new SaveData();
        // ランキング情報をdata.rankの中に入れる
        data.rank = rankingDataList;
        // JSONとはJavaScript Object Notationの略。データフォーマット
        // Jsonが何かという話は実際にセーブした後にテキストを見るとわかりやすい
        // 構造体からJson形式の文字列に変換する
        string json = JsonUtility.ToJson(data);
        // (Projectフォルダのパス)/Assets/ に保存される
        // ただし、本来のセーブ場所としてApplication.persistentDataPathが用意されている
        // 今回はPC開発でアクセスしやすいという理由でApplication.dataPathを利用しているが、
        // スマホ開発（Android,iPhone）で同じことをすると沼にハマるので注意。
        string path = Application.dataPath + "/ranking.txt";
        // C#版のファイル書き込み機能
        StreamWriter writer = new StreamWriter(path);
        // 文字列を書き込む
        writer.Write(json);
        // セーブ処理
        writer.Flush();
        // お片付け
        writer.Close();

        //追加
        Debug.Log("Save called");

    }
    void Load()
    {
        SaveData data = new SaveData();
        // (Projectフォルダのパス)/Assets/ から読み込む
        string path = Application.dataPath + "/ranking.txt";
        // C#版のファイル読み込み機能
        StreamReader reader = new StreamReader(path);
        // 指定パスにセーブファイルがなかったら何もしない
        if (reader == null)
        {
            return;
        }
        // ファイルから文字列を文末まで読み込む
        string json = reader.ReadToEnd();
        // リーダーのお片付け
        reader.Close();
        // 空文字だったら何もしない
        if (json == "")
        {
            return;
        }
        // 構造体にJsonの文字列を流し込んで変換する
        // <○○>に構造体を指定する
        // json 読み込んだJson文字列
        data = JsonUtility.FromJson<SaveData>(json);
        // セーブしたランキングのリストを読み込む
        rankingDataList = data.rank;

    }
}
