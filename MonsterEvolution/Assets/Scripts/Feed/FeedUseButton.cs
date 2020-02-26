using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedUseButton : MonoBehaviour
{
   public GameObject gameManager;

   public void OnClick(){
       int num = this.transform.parent.GetSiblingIndex();
       Debug.Log(num);
       List<int> feedCountList = SaveData.GetList<int>(SaveDataKeys.feedCount,DefaultValues.FEED_COUNT);
       int feedCount = feedCountList[num];
       if(feedCount==0){
           //餌が0個
           return;
       }
       //餌を1個消費
       feedCountList[num]--;
       //ステータスと経験値アップ
       Monster mons = SaveData.GetClass<Monster>(SaveDataKeys.monster,DefaultValues.MONSTER);
       mons.statusList[num] += 1;
       mons.exp += 1;
       //データ保存
       SaveData.SetList<int>(SaveDataKeys.feedCount,feedCountList);
       SaveData.SetClass<Monster>(SaveDataKeys.monster,mons);
       SaveData.Save();
       //UI反映
       gameManager.GetComponent<UpdateUI>().UpdateFeedCountText(feedCountList);
       gameManager.GetComponent<UpdateUI>().UpdateMonsterUI(mons);
   }

}
