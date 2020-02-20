using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    //オブジェクト参照
    public List<GameObject> textPossessedCoinList;
    public GameObject potionTab;

    public void UpdatePossessedCoinText(int possessedCoin){
        textPossessedCoinList.ForEach(t => {
            t.GetComponent<Text>().text = possessedCoin.ToString();
        });
    }

    public void UpdateFeedCountText(List<int> feedCountList){
        for(int i=0;i<potionTab.transform.childCount;i++){
            potionTab.transform.GetChild(i).GetChild(3).gameObject.GetComponent<Text>().text = "x"+feedCountList[i].ToString();
        }
    }
}
