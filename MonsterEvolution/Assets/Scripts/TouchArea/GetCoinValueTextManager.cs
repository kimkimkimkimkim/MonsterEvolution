using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GetCoinValueTextManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float time = 0.7f;
        Vector3 pos = this.gameObject.transform.localPosition;
        this.transform
            .DOLocalMove(
                new Vector3(pos.x, pos.y + 10,0),
                time
            );
    }

}
