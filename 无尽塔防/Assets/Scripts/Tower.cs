using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    
    public float coin = 500;  //建造消耗的金币
    public float attack;
    public float attackSpeed;
    public float attackRanage;

    public float rotateSpeed;
    public float recharge;



    public List<Transform> muzzle;

    public Transform turret;

    public GameObject bullet;
    [HideInInspector]
    public Transform target;  //
    private float lastShootTime;

    private bool isStartShoot;

    void Start()
    {
        isStartShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (turret == null)
            return;


        //确定攻击目标
        GetAttackTarget();
        //旋转炮塔
        RotateEnemy();
        if (!isStartShoot && bullet != null)
        {
            RaycastHit hitInfo;
            
            //返回检测到的物体的名字
            if(muzzle.Count > 1)
            {
                if (Physics.Raycast(muzzle[1].transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, 10f, LayerMask.GetMask("TowerBase"))
                && hitInfo.collider.tag == "Enemy")
                {

                    isStartShoot = true;
                }
            }
            else
            {
                if (Physics.Raycast(muzzle[0].transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, 10f, LayerMask.GetMask("TowerBase"))
    && hitInfo.collider.tag == "Enemy")
                {

                    isStartShoot = true;
                }
            }

        }
        //开始攻击
        Shooting();
    }

    void GetAttackTarget()
    {
        //判断攻击目标不为空
        if(target != null)
        {
            //判断炮塔和攻击目标是否是在炮塔攻击范围内，如果不在不进行攻击
            if(Vector3.Distance(target.transform.position, transform.position) <= attackRanage)
            {
                return;
            }
            else
            {
                
                target = null;
                return;
            }
        }
        else
        {
            //isStartShoot = false;
            for (int i = 0; i < EnemyManager.instance.enemyList.Count; i++)
            {
                GameObject enemy = EnemyManager.instance.enemyList[i];
                if (Vector3.Distance(enemy.transform.position, transform.position) <= attackRanage)
                {
                    target = enemy.transform;
                    return;
                }
            }
        }
    }


    void RotateEnemy()
    {
        if(target != null)
        {
            Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, target.transform.position - turret.position);
            Quaternion rote = Quaternion.Lerp(turret.rotation, quaternion , Time.deltaTime * rotateSpeed * attackSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rote.eulerAngles.y,
                transform.eulerAngles.z);    //控制炮塔的左右旋转

            turret.eulerAngles = new Vector3(rote.eulerAngles.x, turret.eulerAngles.y, turret.eulerAngles.z);//控制炮塔上下移动

        }
    }

    void Shooting()
    {
        if(target != null)
        {
            //计算炮口和炮身的位置角度
            Vector3 turrentA = target.position - turret.position + turret.forward;
            Vector3 turrentB = target.position - turret.position;
            //当角度小于10的时候就可以进行攻击，控制攻击的旋转范围
            if(Vector3.Angle(turrentA, turrentB) <= 10)
            {
               
                //判断攻击是否在攻击间隔中
                if (Time.time - lastShootTime <= recharge / attackSpeed)
                {
                    return;
                }
                else
                {
                    if(bullet != null && isStartShoot)
                    {
                        lastShootTime = Time.time;
                        //遍历枪口
                        for(int i = 0; i < muzzle.Count; i++)
                        {
                            GameObject go = GameObject.Instantiate(bullet);
                            go.tag = "Bullet";
                            go.transform.position = muzzle[i].position;
                            go.transform.eulerAngles = muzzle[i].eulerAngles;

                            go.transform.localScale *= 0.1f;
                            if(go.GetComponent<ProjectileMoveScript>() != null)
                            {
                                go.GetComponent<ProjectileMoveScript>().tower = this;
                            }
                            else
                            {
                                Debug.Log("erro");
                            }
                            AudioSource audioSource = go.AddComponent<AudioSource>();
                            if (PlayerPrefs.HasKey("voiceValue"))
                            {
                                audioSource.volume = PlayerPrefs.GetFloat("voiceValue");
                            }
                            else
                            {
                                audioSource.volume = 1;
                            }
                            audioSource.clip = Resources.Load<AudioClip>("audio/attack");

                        }
                    }
                }
            }
        }
    }
}
