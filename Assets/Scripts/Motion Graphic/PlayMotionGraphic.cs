using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayMotionGraphic : MonoBehaviour 
{
    private WebGLMovieTexture tex;
    public GameObject Plane;
    private bool played = false;            // 影片開始播，就改成true
    void Start()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WebGLPlayer:
                tex = new WebGLMovieTexture("StreamingAssets/Motion Graphic.mp4");
		        Plane.GetComponent<MeshRenderer>().material = new Material (Shader.Find("Diffuse"));
		        Plane.GetComponent<MeshRenderer>().material.mainTexture = tex;
                break;
            default:
                Debug.LogError("只支持 WebGL 喔 !!");
                break;
        }
    }
    

    void Update()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.WebGLPlayer:
                if(tex.isReady && !played)
                {
                    tex.Play();
                    played = true;
                }
                tex.Update();
                if (played && tex.time != 0 && tex.time == tex.duration)
                {
                    tex.Pause();
                    SceneManager.LoadSceneAsync(4);
                }
                break;
        }
    }

    public void MovieTextureStop()
    {
        if (played)
            tex.Pause();
    }
}
