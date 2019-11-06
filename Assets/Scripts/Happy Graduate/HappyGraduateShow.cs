using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HappyGraduateShow : MonoBehaviour 
{
    private const float ConstStageTime = 2;
    private float times;
    private int stage_index = 0;
    public GameObject button;

    public GameObject[] img;
    void Update()
    {
        if (stage_index == 4)
        {
            button.SetActive(true);
            Destroy(this);
        }
        else if (stage_index <= 3)
        {
            Transform[] t = img[stage_index].GetComponentsInChildren<Transform>();
            Image s;
            for (int i = 0; i < t.Length; i++)
            {
                s = t[i].GetComponent<Image>();
                s.color = new Color(
                    s.color.r,
                    s.color.g,
                    s.color.b,
                    times / ConstStageTime * 2);
            }
            times += Time.deltaTime;
            if (times > ConstStageTime)
            {
                stage_index++;
                times = 0;
            }
        }
    }
}
