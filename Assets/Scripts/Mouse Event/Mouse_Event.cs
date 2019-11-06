using UnityEngine;
using System.Collections;
//using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Mouse_Event : MonoBehaviour 
{
    private int Mouse_On_Hash = Animator.StringToHash("Mouse_On");

    public void Mouse_Over (GameObject btn)
    {
        Animator button_anim = btn.GetComponent<Animator>();
        button_anim.SetBool(Mouse_On_Hash, true);
    }

    public void Mouse_Exit (GameObject btn)
    {
        Animator button_anim = btn.GetComponent<Animator>();
        button_anim.SetBool(Mouse_On_Hash, false);
    }

    public void Mouse_Down (GameObject btn)
    {
        Animator button_anim = btn.GetComponent<Animator>();
        button_anim.SetBool(Mouse_On_Hash, false);
        Transform[] temp = btn.GetComponentsInChildren<Transform>(true);
        temp[1].gameObject.SetActive(false);
        temp[temp.Length - 1].gameObject.SetActive(true);

        switch (btn.name)
        {
            //Menu Scene
            case "Start":
                SceneManager.LoadSceneAsync(2);
                break;
            case "About":
                SceneManager.LoadSceneAsync(1);
                break;

            //About Scene
            case "Menu":
                SceneManager.LoadSceneAsync(0);
                break;

            //Carerr Stage
            case "Ok":
                if (SceneManager.GetActiveScene().name == "Happy Graduate")
                    SceneManager.LoadSceneAsync(0);
                else if (SceneManager.GetActiveScene().name == "Career Stage")
                {
                    GameObject music_back = GameObject.Find("Audio Background"); //GameObject.FindWithTag("Back Music"); //.FindGameObjectWithTag("Back Music");
                    if (Career_Keyboard.Career_Index == 0)
                    {
                        if (music_back != null)
                            Destroy(music_back);
                        SceneManager.LoadSceneAsync(3);
                    }
                }
                break;

            //Dead Page
            case "Continue":
                SceneManager.LoadSceneAsync(4);
                break;
            case "End":
                SceneManager.LoadSceneAsync(0);
                break;
        }
    }
}
