using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    //オブジェクト参照
    public List<GameObject> textPossessedCoinList;
    public GameObject potionTab;
    public GameObject expSlider;
    public GameObject statusParent;

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

    public void UpdateMonsterUI(Monster monster){
        //経験値
        int maxValue = 100;
        expSlider.GetComponent<Slider>().maxValue = maxValue;
        expSlider.GetComponent<Slider>().value = monster.exp;
        expSlider.transform.GetChild(3).gameObject.GetComponent<Text>().text = monster.exp+"/"+maxValue;
        //ステータス
        for(int i=0;i<monster.statusList.Count;i++){
            statusParent.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Text>().text =
                monster.statusList[i].ToString();
        }
    }
}
