using UnityEngine;
using System.Collections;

public class Menu_Audio : MonoBehaviour {

    public AudioClip Background;
    private AudioSource aSource;
    public static bool IsHaveMusicBk = false;

	void Awake () 
    {
        DontDestroyOnLoad(this);
        if (!IsHaveMusicBk)
        {
            IsHaveMusicBk = true;
            aSource = gameObject.AddComponent<AudioSource>();
            aSource.clip = Background;
            aSource.loop = true;
            aSource.Play();
        }
        else
            Destroy(this.gameObject);
	}
}
