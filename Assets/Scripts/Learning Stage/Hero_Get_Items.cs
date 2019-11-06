using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D) , typeof(AudioSource))]
public class Hero_Get_Items : MonoBehaviour 
{
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.gameObject.GetComponent<Collider2D>().isTrigger = true;
    }
    void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.name == "Designer")
        {
            Road_Move.add_stage_index();
            Destroy(this.gameObject);
        }
	}
}
