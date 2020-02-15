using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    //オブジェクト参照
    public Sprite normalSprite;
    public Sprite selectedSprite;

    //変数宣言
    private float scale;
    private RectTransform childRect;
    // Start is called before the first frame update
    void Start()
    {
        scale = this.transform.localScale.y;
        childRect = this.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Debug.Log(scale);

        //eventtriggerがアタッチされてなければアタッチする
        if(this.GetComponent<EventTrigger>() == null){
            this.gameObject.AddComponent<EventTrigger>();
        };
        //クリックしたら画像を変えるメソッド追加
        EventTrigger trigger = this.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventDate) => { 
            this.GetComponent<Image>().sprite = selectedSprite;
            SetStretchedRectOffset(childRect,0, 1*scale, 0, -1*scale);
        });
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((eventDate) => { 
            this.GetComponent<Image>().sprite = normalSprite;
            SetStretchedRectOffset(childRect,0, 0, 0, 0);
        });
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
    }

    //Stretchのサイズを変更する
    private void SetStretchedRectOffset(RectTransform rectT, float left, float top, float right, float bottom) {
        rectT.offsetMin = new Vector2(left, bottom);
        rectT.offsetMax = new Vector2(-right, -top);
    }

    
}
