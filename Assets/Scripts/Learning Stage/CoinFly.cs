using UnityEngine;
using System.Collections;

public class CoinFly : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(TriggerList.CheckBarriersOrMonsters(0 , other.name))
            this.gameObject.SetActive(false);
    }
}
