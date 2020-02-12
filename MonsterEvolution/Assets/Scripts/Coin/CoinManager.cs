using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinManager : MonoBehaviour
{
    //変数宣言
    private GameObject coinArea;

    void Start(){
        coinArea = GameObject.Find("Canvas/CoinArea");
    }

    public void TouchCoin(){
        if(Input.GetMouseButton(0) == false) return;
        
        coinArea.GetComponent<CoinAppear>().GetCoin();
        Destroy(this.gameObject);
    }

    public void TouchCoinAnimation(Vector3 iniPos){
        //Coin
        float time = 0.5f;
        this.transform
            .DOLocalMove(
                new Vector3(iniPos.x,iniPos.y + 7,0),
                time/2)
            .SetEase(Ease.OutCirc);
        this.transform
            .DOLocalMove(
                iniPos,
                time/2)
            .SetEase(Ease.InSine)
            .SetDelay(time/2)
            .OnComplete(() => Destroy(this.gameObject));
        
        //Text

        
    }
}
