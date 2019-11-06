using UnityEngine;
using System.Collections;
[RequireComponent (typeof(Rigidbody2D),typeof(Collider2D),typeof(SpriteRenderer))]

public class FillShow : MonoBehaviour 
{
    private SpriteRenderer sprite;
    private new CircleCollider2D collider2D;
    private AnimatorStateInfo stateInfo;
    private Animator anim;
    private GameObject cloud;
    private bool hitted = false;
    private bool finishPlay = false;

    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        collider2D = this.GetComponent<CircleCollider2D>();
        sprite.enabled = false;
        collider2D.enabled = true;

        Transform[] tran = this.GetComponentsInChildren<Transform>();
        cloud = tran[1].gameObject;
        anim = tran[1].GetComponent<Animator>();
    }
    void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (hitted && stateInfo.IsName("CloudsF"))
        {
            anim.SetBool("Show", false);
            finishPlay = true;
        }
        else if (finishPlay && stateInfo.IsName("CloudsB"))
            this.GetComponent<SpriteRenderer>().enabled = true;
        else if (finishPlay && stateInfo.IsName("Start"))
        {
            Destroy(cloud);
            Destroy(this);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (other.name == "Time")
        {
            this.GetComponent<Collider2D>().enabled = false;
            anim.SetBool("Show", true);
            hitted = true;
        }
    }
}
