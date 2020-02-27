using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FeedButtonManager : MonoBehaviour
{
    //オブジェクト参照
    public GameObject potionPrefab;
    public List<Sprite> potionSpriteList;

    //変数宣言
    private GameObject canvas;
    private GameObject gameManager;
    private GameObject monster;
    private float timePotionMove = 0.5f;

    void Start(){
        canvas = GameObject.Find("Canvas");
        gameManager = GameObject.Find("GameManager");
        monster = GameObject.Find("Canvas/Char");
    }

    public void OnClick(GameObject button){
        int num = button.transform.parent.GetSiblingIndex();
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
        //UI反映とアニメーション
        //エサの個数のUI反映
        gameManager.GetComponent<UpdateUI>().UpdateFeedCountText(feedCountList);
        //アニメーション
        Animation(button,num);
        //経験値、ステータスのUI反映
        gameManager.GetComponent<UpdateUI>().UpdateMonsterUI(mons);
    }


    private void Animation(GameObject button, int num){
        //プレハブ生成
        GameObject potion = (GameObject)Instantiate(potionPrefab);
        potion.transform.SetParent(canvas.transform,false);
        potion.transform.position = button.transform.parent.position;
        potion.transform.localScale = new Vector3(0.28f,0.28f,0.28f);
        potion.GetComponent<Image>().sprite = potionSpriteList[num];
        //軌跡
        float ratio = 0.5f; //s -ratio-> m -(1-ratio)-> e
        float yOffset = 1f;
        Vector3 sPos = potion.transform.position;
        Vector3 ePos = monster.transform.position;
        Vector3 mPos = new Vector3(
            sPos.x + (ePos.x-sPos.x)*ratio,
            sPos.y + (ePos.y-sPos.y)*ratio + yOffset,
            0
        );
        Vector3[] path = {
            mPos,
            monster.transform.position
        };
        //アニメーション
        potion.GetComponent<RectTransform>()
            .DOPath(path, timePotionMove, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {});
        potion.GetComponent<RectTransform>()
            .DOScale(new Vector3(0.2f,0.2f,0.2f),timePotionMove);

    }

}
