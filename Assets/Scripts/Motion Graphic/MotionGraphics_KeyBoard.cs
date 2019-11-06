using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
[RequireComponent (typeof (PlayMotionGraphic))]
public class MotionGraphics_KeyBoard : MonoBehaviour 
{
    private PlayMotionGraphic movieTexture;
    void Start()
    {
        movieTexture = this.GetComponent<PlayMotionGraphic>();
    }
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            movieTexture.MovieTextureStop();
            SceneManager.LoadSceneAsync(4);
        }
	}
}
