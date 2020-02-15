using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TouchAreaManager : MonoBehaviour
{
    //オブジェクト参照
    public GameObject gameManager;
    public GameObject coinPrefab;
    public GameObject textPrefab;

    //変数宣言
    private GameObject canvas;
    private GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCoin(){
        int possessedCoin = SaveData.GetInt(SaveDataKeys.possessedCoin,DefaultValues.POSSESSED_COIN);
        int coinValue = SaveData.GetInt(SaveDataKeys.coinValue,DefaultValues.COIN_VALUE);
        possessedCoin += coinValue;
        SaveData.SetInt(SaveDataKeys.possessedCoin,possessedCoin);
        SaveData.Save();
        gameManager.GetComponent<UpdateUI>().UpdatePossessedCoinText(possessedCoin);


        //Coin
        GameObject coin = (GameObject)Instantiate(coinPrefab);
        coin.GetComponent<Image>().raycastTarget = false;
        coin.transform.SetParent(this.transform,false);
        coin.transform.localScale = new Vector3(1,1,1);
        coin.transform.localPosition = new Vector3(0,0,0);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle (
            canvas.GetComponent<RectTransform>(), 
            Input.mousePosition, 
            camera.GetComponent<Camera>(), 
            out localPos
        );
        Vector3 pos = new Vector3(localPos.x,localPos.y + 7,0);
        coin.transform.localPosition = pos;

        coin.GetComponent<CoinManager>().TouchCoinAnimation(pos);

        //Text
        GameObject text = (GameObject)Instantiate(textPrefab);
        Vector3 textPos = new Vector3(pos.x, pos.y+10,0);
        text.GetComponent<Text>().text = "+1";
        text.transform.SetParent(this.transform, false);
        text.transform.localScale = new Vector3(1,1,1);
        text.transform.localPosition = textPos;

        float time = 0.7f;
        text.transform
            .DOLocalMove(
                new Vector3(textPos.x, textPos.y + 5,0),
                time)
            .OnComplete(() => Destroy(text));
    }
}
