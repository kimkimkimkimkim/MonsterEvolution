using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FeedButtonManager : MonoBehaviour
{
    //オブジェクト参照
    public GameObject potionPrefab;
    public List<Sprite> potionSpriteList;
    public GameObject effect;
    public GameObject header;
    public GameObject gachaButton;


    //変数宣言
    private GameObject canvas;
    private GameObject gameManager;
    private GameObject monster;
    private float timePotionMove = 0.5f;
    private bool canPush = true; //エサボタンを押していいかどうか

    //アニメーションなどで使用する変数
    private Monster mons = null;
    private GameObject buttonObj = null;
    private int potionNum = -1;


    void Start(){
        canvas = GameObject.Find("Canvas");
        gameManager = GameObject.Find("GameManager");
        monster = GameObject.Find("Canvas/Char");
    }

    public void OnClick(GameObject button){
        int num = button.transform.parent.GetSiblingIndex();
        List<int> feedCountList = SaveData.GetList<int>(SaveDataKeys.feedCount,DefaultValues.FEED_COUNT);
        int feedCount = feedCountList[num];
        if(feedCount==0 || !canPush){
            //餌が0個
            return;
        }

        canPush = false;
        //餌を1個消費
        feedCountList[num]--;
        //ステータスと経験値アップ
        mons = SaveData.GetClass<Monster>(SaveDataKeys.monster,DefaultValues.MONSTER);
        mons.statusList[num] += 1;
        mons.exp += 1;
        //データ保存
        SaveData.SetList<int>(SaveDataKeys.feedCount,feedCountList);
        SaveData.SetClass<Monster>(SaveDataKeys.monster,mons);
        SaveData.Save();
        //UI反映とアニメーション
        //エサの個数のUI反映
        gameManager.GetComponent<UpdateUI>().UpdateFeedCountText(feedCountList);

        buttonObj = button;
        potionNum = num;
        StartFeedAnimation();
    }

    private void MoveUI(bool doHide){
        float k = doHide?1:-1;
        float time = 1f;
        float offset = 100;
        //上にいくやつ
        /*
        header.transform.DOLocalMove(
            new Vector3(0,offset * k,0),
            time
        ).SetRelative();
        */

        //下にいくやつ
        gachaButton.transform.DOLocalMove(
            new Vector3(0,-offset * k,0),
            time
        ).SetRelative();

        this.transform.DOLocalMove(
            new Vector3(0,-offset * k,0),
            time
        ).SetRelative();
    }

    //エサを与えた時のアニメーションスタート！！
    private void StartFeedAnimation(){

        //UIを隠す
        MoveUI(true);

        //0.5秒後に実行する
        StartCoroutine(DelayMethod(0.5f, () =>
        {
            //アニメーション
            FeedMoveAnimationFromButtonToChar();
        }));

    }

    //エサがボタンの位置からモンスターの位置まで移動するアニメーション
    private void FeedMoveAnimationFromButtonToChar(){
        //プレハブ生成
        GameObject potion = (GameObject)Instantiate(potionPrefab);
        potion.transform.SetParent(canvas.transform,false);
        potion.transform.position = buttonObj.transform.parent.position;
        potion.transform.localScale = new Vector3(0.28f,0.28f,0.28f);
        potion.GetComponent<Image>().sprite = potionSpriteList[potionNum];
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
                EatAnimation(potion);
            });
        potion.GetComponent<RectTransform>()
            .DOScale(new Vector3(0.2f,0.2f,0.2f),timePotionMove);
    }

    //届いたエサを食べる時のアニメーション
    private void EatAnimation(GameObject potion){
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
                SwallowAniimation(potion);
            });    
        });
        });


    }

    //エサを食べて消えていく時のアニメーション
    private void SwallowAniimation(GameObject potion){
        //モンスターのアニメーション終了
        monster.GetComponent<Animator>().SetBool("isEating",false);

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
            StatusUpAnimation();
        });
    }

    //エサを食べ終えてステータスがアップするアニメーション
    private void StatusUpAnimation(){
        //UI反映
        //ステータスアップのエフェクト
        StatusUpTextAnimation();
        gameManager.GetComponent<UpdateUI>().UpdateMonsterUI(mons);

        //エフェクト再生
        effect.GetComponent<ParticleSystem>().Play();

    }

    //ステータスアップのテキストアニメーション
    private void StatusUpTextAnimation(){
        GameObject statusParent = header.transform.GetChild(3).gameObject;
        GameObject status = statusParent.transform.GetChild(potionNum).gameObject;
        GameObject statusUpText = status.transform.GetChild(3).gameObject;
        Vector3 iniPos = statusUpText.transform.localPosition;

        float offsetX = 10;
        statusUpText.transform.localPosition = new Vector3(iniPos.x + offsetX, iniPos.y, 0);
        statusUpText.SetActive(true);
        statusUpText.transform.DOLocalMove(iniPos,0.5f);


        StartCoroutine(DelayMethod(0.5f, () =>
        {
            mons = null;
            buttonObj = null;
            potionNum = -1;

            canPush = true;
            MoveUI(false);

            StartCoroutine(DelayMethod(2f, () => {
                statusUpText.SetActive(false);
            }));

        }));

    }

    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="waitTime">遅延時間[ミリ秒]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

}
