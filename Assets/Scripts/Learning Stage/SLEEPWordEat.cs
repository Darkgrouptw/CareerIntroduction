using UnityEngine;
using System.Collections;

public class SLEEPWordEat : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Designer")
        {
            Road_Move.SleepWordEat = true;
            switch (this.gameObject.name)
            {
                case "S(Clone)":
                    Road_Move.SleepWordShow = 0;
                    break;
                case "L(Clone)":
                    Road_Move.SleepWordShow = 1;
                    break;
                case "E 1(Clone)":
                    Road_Move.SleepWordShow = 2;
                    break;
                case "E 2(Clone)":
                    Road_Move.SleepWordShow = 3;
                    break;
                case "P(Clone)":
                    Road_Move.SleepWordShow = 4;
                    break;
            }
            this.gameObject.SetActive(false);
        }
    }
}
