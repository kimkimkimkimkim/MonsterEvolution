using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimation : MonoBehaviour
{
    //オブジェクト参照
    public List<Sprite> sprites = new List<Sprite>();
    public float span = 0.5f;

    //変数
    private float time = 0;
    private int index = 0;
    private int listCount = 0;
    private Image myImage;

    void Start()
    {
        listCount = sprites.Count;
        myImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update(){
        time += Time.deltaTime;
        if(time >= span){
            time = 0;
            index++;
            if(index>=listCount)index -= listCount;
            myImage.sprite = sprites[index];
        }
    }
}
