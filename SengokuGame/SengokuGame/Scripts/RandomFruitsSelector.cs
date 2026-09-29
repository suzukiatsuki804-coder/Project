
using UnityEngine;

public class RandomFruitsSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fruitsPrefabs;  //[]...配列を表す


    private GameObject reservedFruits;

    public GameObject ReservedFruits
    {
        get { return reservedFruits; }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pop();
    }

   public GameObject Pop()
    {
        GameObject ret = reservedFruits;

        int index = Random.Range(0, fruitsPrefabs.Length); //0からruitsprefabの最大値からランダムで取得する
        reservedFruits = fruitsPrefabs[index];

        return ret;
    }
}
