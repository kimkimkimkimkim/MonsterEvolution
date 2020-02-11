using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
