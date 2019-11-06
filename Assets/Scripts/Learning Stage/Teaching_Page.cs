using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Teaching_Page : MonoBehaviour 
{
    //開始時要設定
    public Road_Move road_scripts;                              //路要動的程式碼
    public GameObject Designer;
    public GameObject Teacher;
    private int Ready_point_X = -9;                             //Designer 預備的位置 X
    public const int Running_point_X = -7;                      //Designer 跑步的位置 X
    private int Designer_Y = 1;                                 //Designer Y的紹$    
    /*
     *計數 
     * 0 人物由左邊走進來，走到定點遇到老師
     * 1 老師淡進
     * 2 
     * | 老師講話
     * 5
     * 6 人跑到後面
     * 
     ********* 教學模式 *********
     * 7 透明布+紅綠燈 出現
     * 8 障礙物1要出來，時間結束，文字1最後要看不見
     * 9 要按鈕和文字出現
     * 10 可以按下Z
     * 11 顯示空洞和文字
     * 12 要按鈕和文字出現
     * 13 可以按下X
     * 14 顯示耍費怪和文字
     * 15 顯示上下按鈕
     * 16 顯示肝 1
     * 17        |
     * 18 先是肝 3
     * 
     * 20 讓Designer從最左邊跑過來
     * 21 開始跑
     */

    //Teaching Items
    public SpriteRenderer[] stage2to5_say_items;
    public Image stage7_back;
    public SpriteRenderer[] Traffic_Light;
    public Image[] stage8_Items;
    public Image[] stage9_Items;
    public Image[] stage11_Items;
    public Image[] stage12_Items;
    public Image[] stage14_Items;
    public Image[] stage15_Items;
    public Image[] stage16_Items;
    public Image[] stage17_Items;
    public Image[] stage18_Items;
    public GameObject Teaching_items;                              //結束時  要刪掉
    public Animator Coin_anim;                                     //Coin丟下去得動畫
    public Animator Barrier1_anim;                                 //障礙物被coin打到的動畫
    public Animator Time_anim;
    public GameObject Filled_Groups;
    private int Coin_Drop_Hash = Animator.StringToHash("Base Layer.Coin Animation drop");
    private int Empty_Hash = Animator.StringToHash("Base Layer.Empty");

    public Text Coin_Text;                                          //設定文字
    public Text Time_Text;                                          //設定文字
    //這個要關掉
    public GameObject Button_ok;     

    private int stage_index = 0;
    private const float finished_times = 2f;                        //每次間隔多久
    private float times = 0f;                                       //計算時間

    //讓玩家可以操控
    public HeroControll hero_controll;

    void Start()
    {
        road_scripts.enabled = false;
        Teacher.transform.position = new Vector3(5, -2, 0);
        Teacher.GetComponent<Animator>().enabled = false;
        Designer.transform.position = new Vector3(Ready_point_X, Designer_Y, 0);
        stage_index = 0;
        
        //略過教學
        //stage_index = 18;
        //Designer.transform.localPosition = new Vector3(-7, 1, 0);
    }

    void FixedUpdate()
    {
        switch (stage_index)
        {
            #region "人物由左邊走進來"
            case 0:
                if (finished_times < times)
                {
                    times = 0;
                    Designer.transform.position = new Vector3(Running_point_X,Designer_Y,0);
                    Designer.GetComponent<Animator>().enabled = false;
                    stage_index++;
                }
                else
                {
                    times += Time.deltaTime;
                    Designer.transform.position = new Vector3((Running_point_X - Ready_point_X) * times / finished_times + Ready_point_X,Designer_Y,0);
                }
                break;
            #endregion
            #region "老師淡進"
            case 1:
                if (finished_times < times)
                {
                    times = 0;
                    Teacher.GetComponent<Animator>().enabled = true;
                    stage_index++;
                }
                else
                {
                    times += Time.deltaTime;
                    Transform[] temp = Teacher.GetComponentsInChildren<Transform>(true);
                    Teacher.transform.position += new Vector3(0, 3.8f, 0) * Time.deltaTime / finished_times;
                    for (int i = 1; i < temp.Length - stage2to5_say_items.Length; i++)
                    {
                        SpriteRenderer temp_render = temp[i].GetComponent<SpriteRenderer>();
                        temp_render.color = new Color(temp_render.color.r,temp_render.color.g,temp_render.color.b, times/finished_times);
                    }
                }
                break;
            #endregion
            #region "老師講話"
            case 2:
            case 3:
            case 4:
            case 5:
                times += Time.deltaTime;
                if (2 * finished_times < times)
                {
                    times = 0;
                    stage2to5_say_items[stage_index - 2].color = new Color(
                        stage2to5_say_items[stage_index - 2].color.r,
                        stage2to5_say_items[stage_index - 2].color.g,
                        stage2to5_say_items[stage_index - 2].color.b,
                        0); 
                    if(stage_index == 5)
                        Designer.GetComponent<Animator>().enabled = true;
                    stage_index++;
                }
                else if (2 * finished_times - 0.5 < times)
                {
                    float temp = times - 2 * finished_times + 0.5f;
                    stage2to5_say_items[stage_index - 2].color = new Color(
                        stage2to5_say_items[stage_index - 2].color.r,
                        stage2to5_say_items[stage_index - 2].color.g,
                        stage2to5_say_items[stage_index - 2].color.b,
                        (1 - temp) / 0.5f);
                }
                else if(0.5 <= times && times <= 2*finished_times - 0.5)
                {
                    stage2to5_say_items[stage_index - 2].color = new Color(
                        stage2to5_say_items[stage_index - 2].color.r,
                        stage2to5_say_items[stage_index - 2].color.g,
                        stage2to5_say_items[stage_index - 2].color.b,
                        1);
                }
                else if (times < 0.5)
                {
                    stage2to5_say_items[stage_index - 2].color = new Color(
                        stage2to5_say_items[stage_index - 2].color.r,
                        stage2to5_say_items[stage_index - 2].color.g,
                        stage2to5_say_items[stage_index - 2].color.b,
                        times / 0.5f);
                }
                break;
            #endregion
            #region "人跑到後面"
            case 6:
                if (finished_times < times)
                {
                    times = 0;
                    stage_index = 7;
                    Designer.transform.position = new Vector3(Ready_point_X, Designer_Y, 0);
                    Destroy(Teacher);
                }
                else
                {
                    times += Time.deltaTime;
                    Designer.transform.position = new Vector3((-Ready_point_X- Running_point_X)* times / finished_times+Running_point_X, Designer_Y, 0); 
                }
                break;
            #endregion

            #region "透明布+紅綠燈 出現"
            case 7:
                if (finished_times < times)
                {
                    times = 0;
                    stage_index = 8;
                }
                else
                {
                    times += Time.deltaTime;
                    stage7_back.color = new Color(
                        stage7_back.color.r,
                        stage7_back.color.g,
                        stage7_back.color.b,
                        times / finished_times * 2);
                    SpriteRenderer temp;
                    for (int i = 0; i < Traffic_Light.Length; i++)
                    {
                        temp = Traffic_Light[i].GetComponent<SpriteRenderer>();
                        temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, times / finished_times);
                    }
                }
                break;
            #endregion
            #region "障礙物1要出來，時間結束，文字1最後要看不見"
            case 8:
                if (finished_times < times)
                {
                    times = 0;
                    stage_index = 9;
                    for (int i = stage8_Items.Length - 1; i >= 0; i--)
                        if (stage8_Items[i].gameObject.name == "Text 1")
                        {
                            stage8_Items[i].gameObject.SetActive(false);
                            break;
                        }
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage8_Items.Length; i++)
                        stage8_Items[i].color = new Color(
                            stage8_Items[i].color.r,
                            stage8_Items[i].color.g,
                            stage8_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion
            #region "要按鈕和文字出現"
            case 9:
                if (finished_times < times)
                {
                    times = 0;
                    stage_index = 10;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage9_Items.Length; i++)
                        stage9_Items[i].color = new Color(
                            stage9_Items[i].color.r,
                            stage9_Items[i].color.g,
                            stage9_Items[i].color.b,
                            times / finished_times);
                }
                break;
            #endregion
            #region "可以按下Z"
            case 10:
                if (Coin_anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Empty_Hash && times > finished_times/2 )
                {
                    stage_index = 11;
                    for (int i = 0; i < stage8_Items.Length; i++)
                        stage8_Items[i].enabled = false;
                    for (int i = 0; i < stage9_Items.Length; i++)
                        stage9_Items[i].enabled = false;
                    times = 0;
                }
                else if (Coin_anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Empty_Hash)
                {
                    times += Time.deltaTime;
                }
                else if (Coin_anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Coin_Drop_Hash)
                {
                    Barrier1_anim.SetBool("Hit", true);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Coin_Text.text = "100000";
                    Coin_anim.gameObject.SetActive(true);
                }
                break;
            #endregion
            #region "顯示空洞和文字"
            case 11:
                if (finished_times < times)
                {
                    times = 0;
                    stage_index = 12;
                    for (int i = stage11_Items.Length - 1; i >= 0; i--)
                        if (stage11_Items[i].gameObject.name == "Text 3")
                        {
                            stage11_Items[i].gameObject.SetActive(false);
                            break;
                        }
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage11_Items.Length; i++)
                        stage11_Items[i].color = new Color(
                            stage11_Items[i].color.r,
                            stage11_Items[i].color.g,
                            stage11_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion
            #region "要按鈕和文字出現"
            case 12:
                if (finished_times < times)
                {
                    times = 0;
                    stage_index = 13;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage12_Items.Length; i++)
                        stage12_Items[i].color = new Color(
                            stage12_Items[i].color.r,
                            stage12_Items[i].color.g,
                            stage12_Items[i].color.b,
                            times / finished_times );
                }
                break;
            #endregion
            #region "可以按下X"
            case 13:
                if (times > finished_times / 1.5f)
                {
                    for (int i = 0; i < stage11_Items.Length; i++)
                        stage11_Items[i].enabled = false;
                    for (int i = 0; i < stage12_Items.Length; i++)
                        stage12_Items[i].enabled = false;
                    Filled_Groups.SetActive(false);
                    stage_index = 14;
                    times = 0;
                }
                else if (times > finished_times / 2)
                {
                    Filled_Groups.SetActive(true);
                    Time_anim.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    Time_Text.text = "100000 hr";
                    Time_anim.gameObject.SetActive(true);
                }
                else if (Time_Text.text == "100000 hr")
                    times += Time.deltaTime;
                break;
            #endregion
            #region "顯示耍費怪和文字"
            case 14:
                if (times > finished_times)
                {
                    stage14_Items[stage14_Items.Length - 1].enabled = false;
                    stage_index = 15;
                    times = 0;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage14_Items.Length; i++)
                        stage14_Items[i].color = new Color(
                            stage14_Items[i].color.r,
                            stage14_Items[i].color.g,
                            stage14_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion
            #region "顯示上下按鈕"
            case 15:
                if (times > finished_times * 1.5f)
                {
                    for (int i = 0; i < stage14_Items.Length; i++)
                        stage14_Items[i].enabled = false;
                    for (int i = 0; i < stage15_Items.Length; i++)
                        stage15_Items[i].enabled = false;

                    stage_index = 16;
                    times = 0;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage15_Items.Length; i++)
                        stage15_Items[i].color = new Color(
                            stage15_Items[i].color.r,
                            stage15_Items[i].color.g,
                            stage15_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion
            #region "顯示肝 1"
            case 16:
                if (times > finished_times)
                {
                    stage16_Items[0].enabled = false;
                    stage16_Items[1].enabled = false;
                    stage_index = 17;
                    times = 0;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage16_Items.Length; i++)
                        stage16_Items[i].color = new Color(
                            stage16_Items[i].color.r,
                            stage16_Items[i].color.g,
                            stage16_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion
            #region "顯示肝 2"
            case 17:
                if (times > finished_times)
                {
                    stage_index = 18;
                    times = 0;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage17_Items.Length; i++)
                        stage17_Items[i].color = new Color(
                            stage17_Items[i].color.r,
                            stage17_Items[i].color.g,
                            stage17_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion
            #region "顯示肝 3"
            case 18:
                if (times > finished_times)
                {
                    stage_index = 19;
                    times = 0;
                }
                else
                {
                    times += Time.deltaTime;
                    for (int i = 0; i < stage18_Items.Length; i++)
                        stage18_Items[i].color = new Color(
                            stage18_Items[i].color.r,
                            stage18_Items[i].color.g,
                            stage18_Items[i].color.b,
                            times / finished_times * 2);
                }
                break;
            #endregion

            #region "讓Designer從最左邊跑過來"
            case 20:
                if (times > finished_times)
                {
                    stage_index = 21;
                    times = 0;
                    Designer.transform.localPosition = new Vector3(Running_point_X, Designer_Y, 0);
                    Designer.GetComponent<Animator>().enabled = false;
                }
                else
                {
                    Designer.transform.localPosition = new Vector3((Running_point_X- Ready_point_X) * times / finished_times + Ready_point_X, Designer_Y, 0);
                    times += Time.deltaTime;
                }
                break;
            #endregion
            #region "縮排"
            case 21:
                if (times > finished_times)
                {
                    stage_index = 22;
                    times = 0;
                    Designer.GetComponent<Animator>().enabled = true;
                    hero_controll.enabled = true;
                    road_scripts.enabled = true;
                }
                else
                    times += Time.deltaTime;
                break;
            #endregion*/
        }

        if (20 > stage_index && Input.GetKeyDown(KeyCode.UpArrow))
            Game_ok_Down();
    }

    public void Game_ok_Down()
    {
        #region "前面的Setting"
        if (stage_index <= 6)
        {
            Designer.GetComponent<Animator>().enabled = true;
            Designer.transform.position = new Vector3(Ready_point_X, Designer_Y, 0);
            Destroy(Teacher);
        }
        if (stage_index <= 7)
        {
            SpriteRenderer temp;
            for (int i = 0; i < Traffic_Light.Length; i++)
            {
                temp = Traffic_Light[i].GetComponent<SpriteRenderer>();
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 1);
            }
        }
        #endregion

        Coin_Text.text = "100000";
        Time_Text.text = "100000 hr";

        Teacher.GetComponent<Animator>().enabled = true;
        Destroy(Teaching_items);
        Destroy(Button_ok);
        stage_index = 20;
        Designer.SetActive(true);
    }
}
