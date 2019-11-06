using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Rigidbody2D),typeof(Collider2D),typeof(Animator))]
public class Cones_Move : MonoBehaviour 
{
    private Animator anim;
    //private SpriteRenderer sprite;
    private AnimatorStateInfo stateInfo;
    private bool hitted = false;
    private bool finishehPlay = false;
    private AudioSource AudioHit;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        //sprite = this.GetComponent<SpriteRenderer>();
        AudioHit = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!hitted && stateInfo.IsName("Hit"))
        {
            anim.SetBool("Hitted", false);
            hitted = true;
            finishehPlay = true;

            AudioHit.Play();
        }
        else if (finishehPlay && stateInfo.IsName("Normal"))
            this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (other.name == "Coin" && stateInfo.IsName("Normal"))
        {
            EdgeCollider2D[] EdgeColliderList = this.GetComponents<EdgeCollider2D>();
            for (int i = 0; i < EdgeColliderList.Length; i++)
                EdgeColliderList[i].enabled = false;
            anim.SetBool("Hitted", true);
        }
    }

    public bool IsPlayingAnim()
    {
        return hitted;
    }
}
