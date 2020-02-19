﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryMachineManager : MonoBehaviour
{
    //オブジェクト参照
    public GameObject canvas;
    public GameObject camera;
    public GameObject body;
    public GameObject handle;
    public GameObject gameManager;
    public GameObject gachaBallPrefab;

    //変数宣言
    Vector3 center;
    float oneRotationBaseAngle; //１回転判定の基準となる角度（この角度を正の方向で回転すると1回転）
    float defaultZAngle;
    Vector3 iniPos;
    float iniAngle;
    Vector3 nowPos;
    float nowAngle;
    float prevDiffAngle;
    float nowDiffAngle;
    float prevAngle;

    bool enoughCost = false; //ガチャの費用が足りているかどうか
    int gachaCost = 10;

    void Start()
    {
        center = transform.localPosition;
        oneRotationBaseAngle = body.transform.localEulerAngles.z;
    }

    void OnEnable(){
        enoughCost = SaveData.GetInt(SaveDataKeys.possessedCoin,DefaultValues.POSSESSED_COIN) >= gachaCost;
    }

    // Update is called once per frame
    void Update()
    {  
        //所持金が足りなかったら回せない
        if(!enoughCost) return;

        if(Input.GetMouseButtonDown(0)){
            //クリックした時
            //最初の座標を取得
            Vector2 iniPosV2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle (
                canvas.GetComponent<RectTransform>(), 
                Input.mousePosition, 
                camera.GetComponent<Camera>(), 
                out iniPosV2
            );
            iniPos = new Vector3(iniPosV2.x, iniPosV2.y, 0);
            iniAngle = GetAngle(center,iniPos);

            //最初の抽選器の角度取得
            defaultZAngle = body.transform.localEulerAngles.z;
            prevDiffAngle = 0;
            nowDiffAngle = 0;
            prevAngle = body.transform.localEulerAngles.z;
            nowAngle = body.transform.localEulerAngles.z;
        }

        if(Input.GetMouseButton(0)){
            //押されている時は現在の座標と最初の座標から回転させる角度を計算
            Vector2 nowPosV2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle (
                canvas.GetComponent<RectTransform>(), 
                Input.mousePosition, 
                camera.GetComponent<Camera>(), 
                out nowPosV2
            );
            nowPos = new Vector3(nowPosV2.x,nowPosV2.y,0);
            nowAngle = GetAngle(center,nowPos);

            nowDiffAngle = (iniAngle-nowAngle>=0)?iniAngle-nowAngle:360+(iniAngle-nowAngle);//0<=diffAngle<360
            //逆方向には回さない
            prevDiffAngle = nowDiffAngle;
            
            float angle = defaultZAngle - (iniAngle - nowAngle);
            body.transform.rotation = Quaternion.Euler(0, 0, angle);
            handle.transform.rotation = Quaternion.Euler(0, 0, angle);
            if(prevAngle>oneRotationBaseAngle&&angle<oneRotationBaseAngle)OneRotation();
            prevAngle = angle;
        }
    }

    public void OneRotation(){
        GachaAnimation();
        int coin = SaveData.GetInt(SaveDataKeys.possessedCoin,DefaultValues.POSSESSED_COIN);
        enoughCost = coin - gachaCost >= gachaCost;
        SaveData.SetInt(SaveDataKeys.possessedCoin,coin-gachaCost);
        SaveData.Save();
        gameManager.GetComponent<UpdateUI>().UpdatePossessedCoinText(coin - gachaCost);
    }

    void GachaAnimation(){
        GameObject gachaBall = (GameObject)Instantiate(gachaBallPrefab);
        gachaBall.transform.localScale = new Vector3(1,1,1);
        gachaBall.transform.SetParent(this.transform,false);
        gachaBall.transform.SetSiblingIndex(0); 
        gachaBall.transform.localPosition = new Vector3(15.5f,2.4f,0);
        gachaBall.GetComponent<Rigidbody2D>().AddForce(new Vector3(40,0,0));
    }

    public float GetAngle(Vector3 p1, Vector3 p2) {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
}
