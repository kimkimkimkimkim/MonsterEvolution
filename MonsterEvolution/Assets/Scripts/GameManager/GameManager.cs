using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //変数宣言
    UpdateUI updateUI;

    void Start(){
        updateUI = this.GetComponent<UpdateUI>();

        SetSaveData();
    }

    //セーブデータの取得と反映
    void SetSaveData(){
        updateUI.UpdatePossessedCoinText(SaveData.GetInt(SaveDataKeys.possessedCoin,DefaultValues.POSSESSED_COIN));
    }
}
