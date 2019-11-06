using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class HeroControll : MonoBehaviour {
    private Animator Hero_anim;
    private int Sliding_hash = Animator.StringToHash("Sliding_bool");
    private int Jump_hash = Animator.StringToHash("Jump_bool");
    private bool IsGround = true;                                           //是不是在地上
    private const int gravityScale = 2;
    private const int ForceJumpY = 500;

    // Hash State
    private AnimatorStateInfo Hero_anim_state;
    private int Hero_Running_State_Hash = Animator.StringToHash("Base Layer.Running");
    private int Hero_SlidingB_State_Hash = Animator.StringToHash("Base Layer.SlidingB");
    private int Hero_JumpF_State_Hash = Animator.StringToHash("Base Layer.JumpF");
    
    //Throw Items
    public GameObject Throw_Groups;
    private GameObject Coin_Org, Time_Org;
    private List<GameObject> ThrowList = new List<GameObject>();
    private float throw_speed = 10;
    
    public Text Coin_UI, Time_UI;
    private int Coin_Num = 100000;
    private int per_Coin = 1000;
    private int Time_Num = 100000;
    private int per_Time = 1000;
    public static float Liver_Num = 200;                                                //肝指數的值
    public const float Liver_Top = 200;                                                 //肝指數的最大值
    private float per_Hurt = 10;

    //Eat Sound
    private AudioSource EatAudio;
    private List<string> SkillGroupName = new List<string>();
    public GameObject SkillGroups;

    //Hurt Mode
    private bool HurtMode = false;
    private float HurtTime;
    private const float ConstHurtTime = 2;
    private SpriteRenderer[] HeroSpriteRenderer;
    void Start ()
    {
        Hero_anim = this.GetComponent<Animator>();

        Transform[] temp = Throw_Groups.GetComponentsInChildren<Transform>(true);
        Coin_Org = temp[1].gameObject;
        Time_Org = temp[2].gameObject;

        //Eat Name List Setting
        EatAudio = this.gameObject.GetComponent<AudioSource>();
        Transform[] t = SkillGroups.GetComponentsInChildren<Transform>(true);
        for (int i = 1; i < t.Length; i++)
            SkillGroupName.Add(t[i].gameObject.name);

        t = this.GetComponentsInChildren<Transform>(true);
        HeroSpriteRenderer = new SpriteRenderer[t.Length - 1];
        for (int i = 1; i < t.Length; i++)
            HeroSpriteRenderer[i - 1] = t[i].GetComponent<SpriteRenderer>();

        //Gravity Scale
        this.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }
	void Update () 
    {
        //Debug.Log(Coin.gameObject.name);
        Hero_anim_state = Hero_anim.GetCurrentAnimatorStateInfo(0);
        #region "滑行"
        if (Input.GetKeyUp(KeyCode.DownArrow))
            Hero_anim.SetBool(Sliding_hash, false);
        else if (Hero_anim_state.fullPathHash == Hero_Running_State_Hash)
            if (Input.GetKeyDown(KeyCode.DownArrow))
                Hero_anim.SetBool(Sliding_hash, true);
        #endregion
        #region "跳"
        if (Hero_anim_state.fullPathHash == Hero_Running_State_Hash || Hero_anim_state.fullPathHash == Hero_SlidingB_State_Hash)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && IsGround)
            {
                Hero_anim.SetBool(Jump_hash, true);
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceJumpY));
            }
        }
        else if (Hero_anim_state.fullPathHash == Hero_JumpF_State_Hash)
            Hero_anim.SetBool(Jump_hash, false);
        #endregion
        #region "Z 跟 X 的按鈕"
        if (Hero_anim_state.fullPathHash == Hero_Running_State_Hash)
            if (Input.GetKeyDown(KeyCode.Z) && Coin_Num > 0)
            {
                GameObject temp = Instantiate<GameObject>(Coin_Org);
                temp.name = "Coin";
                temp.SetActive(true);
                temp.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(1, 0, 0);
                temp.transform.parent = Throw_Groups.transform;
                temp.GetComponent<AudioSource>().Play();
                ThrowList.Add(temp);

                Coin_Num -= per_Coin;
                SetCoinNum();
            }
            else if (Input.GetKeyDown(KeyCode.X) && Time_Num > 0)
            {
                GameObject temp = Instantiate<GameObject>(Time_Org);
                temp.name = "Time";
                temp.SetActive(true);
                temp.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(1, 0, 0);
                temp.transform.parent = Throw_Groups.transform;
                temp.GetComponent<AudioSource>().Play();
                ThrowList.Add(temp);

                Time_Num -= per_Time;
                SetTimeNum();
            }

        int index = 0;
        while (index < ThrowList.Count)
        {
            if (ThrowList[index].transform.localPosition.x < 13)
            {
                ThrowList[index].transform.localPosition += new Vector3(throw_speed * Time.deltaTime, 0, 0);
                ThrowList[index].transform.Rotate(new Vector3(0, 0, Time.deltaTime * throw_speed * -50));
                index++;
            }
            else
            {
                Destroy(ThrowList[index]);
                ThrowList.RemoveAt(index);
            }
        }
        #endregion

        if(HurtMode == true)
        {
            if (HurtTime > ConstHurtTime)
            {
                HurtMode = false;
                SetHurtTimeAlpha(1);
            }
            else if (HurtTime <= ConstHurtTime / 4 || (ConstHurtTime / 2 < HurtTime && HurtTime <= ConstHurtTime * 3 / 4))
            {
                float temp = HurtTime - ((HurtTime > ConstHurtTime / 2) ? (ConstHurtTime / 2) : 0);
                SetHurtTimeAlpha(temp / ConstHurtTime * 2);
            }
            else
            {
                float temp = HurtTime - ((HurtTime > ConstHurtTime / 2) ? (ConstHurtTime / 2) : 0);
                SetHurtTimeAlpha(1 - temp / ConstHurtTime * 2);
            }
            HurtTime += Time.deltaTime;
        }


        if (Liver_Num <= 0)
        {
            Liver_Num = Liver_Top;
            SceneManager.LoadSceneAsync(5);                       //Hero 死掉的頁面
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        #region "碰到JumpCheck"
        if (IsGround && other.name == "JumpCheck")
        {
            //Debug.Log("Leave Ground");
            IsGround = false;
        }
        #endregion
        #region " 碰到怪物會扣寫"
         else if (TriggerList.CheckBarriersOrMonsters(0, other.name))
        {
            //Destroy(other);
            Liver_Num -= per_Hurt;
            HurtTime = 0;
            HurtMode = true;
        }
        #endregion
        #region "吃到東西，要發出聲音"
        else
        {
            bool eatplay = false;
            for (int i = 0; i < SkillGroupName.Count; i++)
                if (other.name == SkillGroupName[i])
                {
                    eatplay = true;
                    break;
                }
            if (eatplay == true)
                EatAudio.Play();
        }
        #endregion
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        string Cname = other.gameObject.name;
        if (!IsGround && Cname == "GroundCheck")
        {
            //Debug.Log("On Ground");
            IsGround = true;
        }
    }

    public void SetCoinNum()
    {
        Coin_UI.text = Coin_Num.ToString();
    }
    public void SetTimeNum()
    {
        Time_UI.text = Time_Num.ToString() + " hr";
    }

    
    private void SetHurtTimeAlpha(float num)
    {
        for(int i = 0 ; i < HeroSpriteRenderer.Length ; i++)
            HeroSpriteRenderer[i].color = new Color(
                HeroSpriteRenderer[i].color.r,
                HeroSpriteRenderer[i].color.g,
                HeroSpriteRenderer[i].color.b,
                num
            );
    }
}
