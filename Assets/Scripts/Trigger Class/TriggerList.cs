using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerList : MonoBehaviour
{
    //為了快速找出，有一個Index指出，從哪裡開始
    //TriggerNameClass.TriggerName a = new TriggerNameClass();
    #region"Barriers"
    public static int[] BarriersIndex =
    {
        0,
        5
    };
    public static string[]  BarriersList =
    {
        "Cones" , "Filled 1" , "Filled 2", "Filled 3", "Filled 4"
    };
    #endregion
    #region "MonsterList"
    public static int[] MonsterIndex = 
    {
        0,
        4
    };
    public static string[]  MonsterList =
    {
        "M1","M2","M3","M4"
    };
    #endregion

    /// <summary>
    /// 找name是不是障礙物 or 怪物
    /// </summary>
    /// <param name="index">第幾關</param>
    /// <param name="name">名稱</param>
    /// <returns></returns>
    public static bool CheckBarriersOrMonsters(int index, string name)
    {
        if (CheckBarriers(index, name))
            return true;
        else
            return CheckMonsters(index, name);
    }

    /// <summary>
    /// 找name是不是障礙物
    /// </summary>
    /// <param name="index">第幾關</param>
    /// <param name="name">比對是不是障礙物</param>
    /// <returns></returns>
    public static bool CheckBarriers(int index, string name)
    {
        for (int i = BarriersIndex[index]; i < BarriersIndex[index + 1]; i++)
            if (BarriersList[i] == name)
                return true;
        return false;
    }

    /// <summary>
    /// 找name是不是怪物
    /// </summary>
    /// <param name="index">第幾關</param>
    /// <param name="name">比對是不是怪物</param>
    /// <returns></returns>
    public static bool CheckMonsters(int index , string name)
    {
        for (int i = MonsterIndex[index]; i < MonsterIndex[index + 1]; i++)
            if (MonsterList[i] == name)
                return true;
        return false;
    }

}
