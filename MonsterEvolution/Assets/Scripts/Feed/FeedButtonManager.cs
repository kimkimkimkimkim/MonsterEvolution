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
        Animation(button,num,mons);
        //経験値、ステータスのUI反映
        //gameManager.GetComponent<UpdateUI>().UpdateMonsterUI(mons);
    }

    //エサがボタンの位置からモンスターの位置まで移動するアニメーション
    private void Animation(GameObject button, int num, Monster mons){
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
            .OnComplete(() => {
                //移動のアニメーションが終わったら食べるアニメーションに
                EatAnimation(potion,mons);
            });
        potion.GetComponent<RectTransform>()
            .DOScale(new Vector3(0.2f,0.2f,0.2f),timePotionMove);
    }

    //届いたエサを食べる時のアニメーション
    private void EatAnimation(GameObject potion,Monster mons){
        //モンスターのアニメーション
        monster.GetComponent<Animator>().SetBool("isEating",true);

        //ポーションのアニメーション
        float sectionTime = 0.5f;
        float speed = 2;
        sectionTime /= speed;
        int sectionNum = 18;
        float timeAnimation = sectionTime * sectionNum;
        Vector3 iniPos = potion.transform.localPosition;
        //ジャンプ
        
        potion.transform
            .DOLocalJump(
                iniPos,           // 移動終了地点
                2,               // ジャンプする力
                9,               // ジャンプする回数
                timeAnimation    // アニメーション時間
            ).SetEase(Ease.Linear);
            
        //角度
        potion.transform
            .DORotate(                      /* まず一回左に回転 */
                new Vector3(0f, 0f, 10f),   // 終了時点のRotation
                sectionTime * 2)                // アニメーション時間
            .SetEase(Ease.InOutSine)
        .OnComplete(() => {
        potion.transform
            .DORotate(                      /* そこから右行って左を片道7回やる */
                new Vector3(0f,0f,-10f),
                sectionTime * 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(
                7, 
                LoopType.Yoyo)
        .OnComplete(() => {
        potion.transform
            .DORotate(                     /* 最後に正面に回転 */
                new Vector3(0f, 0f, 0f),   // 終了時点のRotation
                sectionTime * 2
            ) 
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                //飲み込んで消えるアニメーション
                SwallowAniimation(potion,mons);
            });    
        });
        });


    }

    //エサを食べて消えていく時のアニメーション
    private void SwallowAniimation(GameObject potion, Monster mons){
        float time = 1f;
        Vector3 iniPos = potion.transform.localPosition;

        //ちょっと上にあげる
        potion.transform.DOLocalMove(
            new Vector3(iniPos.x, iniPos.y + 1.5f, 0),
            time
        );
        //スケールを小さくしていく
        potion.transform.DOScale(
            new Vector3(0,0,0),
            time
        ).OnComplete(() => {
            //終わったらpotionを削除して、UIを反映
            Destroy(potion);
            gameManager.GetComponent<UpdateUI>().UpdateMonsterUI(mons);
        });
    }

}
