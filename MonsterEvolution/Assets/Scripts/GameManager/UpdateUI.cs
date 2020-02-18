using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    //オブジェクト参照
    public List<GameObject> textPossessedCoinList;

    public void UpdatePossessedCoinText(int possessedCoin){
        textPossessedCoinList.ForEach(t => {
            t.GetComponent<Text>().text = possessedCoin.ToString();
        });
    }
}
