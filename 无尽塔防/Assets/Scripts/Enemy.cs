using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//负责敌人的基础数据
//负责碰撞检测
//负责检测是否到达水晶
public class Enemy : MonoBehaviour
{
    private int enemyId;
    private string enemyName;
    private int enemyType;
    private int enemyLevel = 1;
    private float enemyHp = 100;
    private float enemyAttack = 10;
    private float enemyDefensive = 10;
    private float enemyMoveSpeed = 5f;
    private float enemyInitSpeed;



    private int gold;  //金币

    private bool isMoving; //是否在移动中，用于寻路
    private Animator animator;
    private Rigidbody rigidbodyComp;

    private Transform[] pointList;
    private int index;     //寻路起始点
    private Transform nextPoint;

    private UIManager uIManager;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rigidbodyComp = this.GetComponent<Rigidbody>();
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {

        Move();
    }
    public void SetPathData(Transform[] pointList)
    {
        if(GameData.instance.levelID <= 0)
        {
            return;
        }

        this.pointList = pointList;
        isMoving = true;
    }
    //{"ID":"1","Health":"300","Attack":"5","Speed":"5","Gold":"100"}
    public void SetData(JsonData d)
    {
        this.enemyLevel = GameData.instance.levelID * 2;
        this.enemyHp = int.Parse(d[1].ToString());
        this.enemyAttack = int.Parse(d[2].ToString());
        this.enemyMoveSpeed = int.Parse(d[3].ToString());
        this.gold = int.Parse(d[4].ToString());;
        this.enemyInitSpeed = this.enemyMoveSpeed;
    }

    void Move()
    {
        //不在寻路状态中不巡路
        if (!isMoving) return;

        if(nextPoint == null)
        {
            index = 0;
            nextPoint = pointList[index];
        }
        //判断距离
        if(Vector3.Distance(transform.position, nextPoint.position) >= 1.0f)
        {
            transform.LookAt(nextPoint.position, Vector3.up);  //朝向目标点
            rigidbodyComp.velocity = transform.forward * enemyMoveSpeed;  //

           // Debug.Log(Vector3.Distance(transform.position, nextPoint.position));
        }
        else
        {
            index++;

            //当路径点超出寻路数组长度，表示怪物到达水晶
            if (index >= pointList.Length)
            {
                rigidbodyComp.velocity = Vector3.zero;
                if(GameData.instance.homeHP > 0)
                {
                    GameData.instance.homeHP -= enemyAttack;
                    //更新UI
                }
                if(GameData.instance.homeHP <= 0)
                {
                    uIManager.ShowGameResult(false);
                    EnemyManager.instance.Stop();
                }

                //
                this.gameObject.SetActive(false);
                return;
            }
            nextPoint = pointList[index];
        }

    }

    //碰撞的双方必须要带有Collider,其中运动的一方带有rigibody
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if (enemyHp <= 0) return;

            enemyHp -= 50;
            if(enemyHp <= 0)
            {
                isMoving = false;
                animator.Play("death_1");
                if(EnemyManager.instance != null)
                {
                    EnemyManager.instance.enemyList.Remove(this.gameObject);
                }
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().enabled = false;  //关闭碰撞
                GameData.instance.killCount += 1;
                GameData.instance.coins += gold;
                uIManager.UpdateBattleData();
                if(collision.gameObject.GetComponent<ProjectileMoveScript>() != null)
                {
                    collision.gameObject.GetComponent<ProjectileMoveScript>().tower.target = null;
                }
                //如果击杀怪物的总数量 = 怪物总数量，游戏胜利
                if(GameData.instance.killCount == GameData.instance.allEnemyCount)
                {
                    uIManager.ShowGameResult(true);
                }
                
                GameObject.Destroy(gameObject, 2.0f);
            }
        }
    }


    /*
     双方带有Collider，其中运动一方带有rigidbody，其中一方的istrigger选项需要勾选
     */
    //当触碰到冰冻特效
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Effect"))
        {
            if(enemyInitSpeed == enemyMoveSpeed)
            {
                enemyMoveSpeed /= 2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
             enemyMoveSpeed = enemyInitSpeed;
        }
    }

    public void Stop()
    {
        isMoving = false;
        rigidbodyComp.Sleep();
        animator.Play("idle01");
    }
}
