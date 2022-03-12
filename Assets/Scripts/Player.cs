using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //属性值
    public float moveSpeed = 3;
    public Vector3 bullectEulerAngles;
    private float defendTime = 3;
    private bool isDefended = true;
    private float timeVal;

    //引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite;//上，右，下，左
    public GameObject bullectPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //是否处于无敌状态
        if (isDefended)
        {
            defendEffectPrefab.SetActive(true);
            defendTime -= Time.deltaTime;
            if (defendTime <= 0)
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }

        

    }   

    private void FixedUpdate()
    {
        if (PlayerMananger.Instance.isDefeat)
        {
            return;
        }
        Move();
        //Attack();

        //攻击的CD
         if (timeVal >= 0.1f)
         {
             Attack();
         }
         else
         {
             timeVal += Time.fixedDeltaTime;
         }
    }

    /// <summary>
    /// 坦克攻击方法
    /// </summary>
    private void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //子弹产生的角度：当前坦克的角度+子弹应该旋转的角度
            Instantiate(bullectPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bullectEulerAngles));
           // timeVal = 0;
        }
    }

    /// <summary>
    /// 坦克移动方法
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (v < 0)
        {
            sr.sprite = tankSprite[2];
            bullectEulerAngles = new Vector3(0, 0, -180);

        }
        else if (v > 0)
        {
            sr.sprite = tankSprite[0];
            bullectEulerAngles = new Vector3(0, 0, 0);
        }
        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            moveAudio.Play();
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

        if (v != 0)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bullectEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bullectEulerAngles = new Vector3(0, 0, -90);
        }
        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            moveAudio.Play();
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            moveAudio.Play();
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
    }

   
    /// <summary>
    /// 坦克死亡
    /// </summary>
    private void Die()
    {
        if (isDefended)
        {
            return;
        }
        PlayerMananger.Instance.isDead = true;
        //产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        //死亡
        Destroy(gameObject);
    }  
}
