using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Monster
{
    public int monsterNum; 
    public int exp;
    public List<int> statusList;

    public Monster(){
        monsterNum = 1;
        exp = 0;
        statusList = new List<int>{0,0,0,0,0,0};
    }
}
