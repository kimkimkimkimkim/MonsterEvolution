using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    //オブジェクト参照
    public GameObject textPossessedCoin;

    public void UpdatePossessedCoinText(int possessedCoin){
        textPossessedCoin.GetComponent<Text>().text = possessedCoin.ToString();
    }
}
