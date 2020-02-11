using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinAppear : MonoBehaviour
{
    //定数定義
    private const int MAX_COIN_NUM = 15;
    private const int RESPAWN_TIME = 5000; //コインの出現間隔[ms]

    //オブジェクト参照
    public GameObject coinPrefab;
    public GameObject coinArea;

    //変数宣言
    private RectTransform coinAreaT;
    private Vector3 coinAreaPos;
    private float coinAreaWidth;
    private float coinAreaHeight;
    private float padding = 7;
    private int currentCoinNum = 0; //現在のコイン数
    private DateTime lastDateTime; //前回コインを生成した時間

    void Start()
    {
        coinAreaT = coinArea.GetComponent<RectTransform>();
        coinAreaPos = new Vector3(coinAreaT.rect.x,coinAreaT.rect.y,0);
        coinAreaWidth = coinAreaT.rect.width;
        coinAreaHeight = coinAreaT.rect.height;

        //初期コイン生成
        currentCoinNum = 10;
        for(int i=0;i<currentCoinNum;i++){
            Appear();
        }
        //初期設定
        lastDateTime = DateTime.UtcNow;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentCoinNum < MAX_COIN_NUM){
            TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

            if(timeSpan >= TimeSpan.FromMilliseconds(RESPAWN_TIME)){
                while(timeSpan >= TimeSpan.FromMilliseconds(RESPAWN_TIME)){
                    AppearNewCoin();
                    timeSpan -= TimeSpan.FromMilliseconds(RESPAWN_TIME);
                }
            }
        }
    }

    //新しいコインを出現させる
    void AppearNewCoin(){
        lastDateTime = DateTime.UtcNow;
        if(currentCoinNum >= MAX_COIN_NUM) return;
        Appear();
        currentCoinNum++;
    }

    //コインを出現させる
    void Appear(){
        Vector3 pos = new Vector3(0,0,0);
        GameObject coin = (GameObject)Instantiate(coinPrefab);
        coin.transform.SetParent(coinArea.transform,false);
        coin.transform.localScale = new Vector3(1,1,1);
        coin.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(coinAreaPos.x+padding,coinAreaPos.x+coinAreaWidth-padding),
            UnityEngine.Random.Range(coinAreaPos.y+padding,coinAreaPos.y+coinAreaHeight-padding),
            0
        );
    }

    //コインをゲット
    public void GetCoin(){
        currentCoinNum--;
    }
}
