using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField]
	Fade fade = null;

    //オブジェクト参照
    public List<GameObject> canvas;
    
    //変数宣言
    private float timeFadeIn = 0.5f;
    private float timeFadeOut = 0.5f;

    public void SceneChange(int index){
        fade.FadeIn (timeFadeIn, () =>
		{
            canvas.ForEach(c => c.SetActive(false));
            canvas[index].SetActive(true);
			fade.FadeOut(timeFadeOut);
		});
    }
}
