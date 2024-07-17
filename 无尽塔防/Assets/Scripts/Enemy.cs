using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//������˵Ļ�������
//������ײ���
//�������Ƿ񵽴�ˮ��
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



    private int gold;  //���

    private bool isMoving; //�Ƿ����ƶ��У�����Ѱ·
    private Animator animator;
    private Rigidbody rigidbodyComp;

    private Transform[] pointList;
    private int index;     //Ѱ·��ʼ��
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
        //����Ѱ·״̬�в�Ѳ·
        if (!isMoving) return;

        if(nextPoint == null)
        {
            index = 0;
            nextPoint = pointList[index];
        }
        //�жϾ���
        if(Vector3.Distance(transform.position, nextPoint.position) >= 1.0f)
        {
            transform.LookAt(nextPoint.position, Vector3.up);  //����Ŀ���
            rigidbodyComp.velocity = transform.forward * enemyMoveSpeed;  //

           // Debug.Log(Vector3.Distance(transform.position, nextPoint.position));
        }
        else
        {
            index++;

            //��·���㳬��Ѱ·���鳤�ȣ���ʾ���ﵽ��ˮ��
            if (index >= pointList.Length)
            {
                rigidbodyComp.velocity = Vector3.zero;
                if(GameData.instance.homeHP > 0)
                {
                    GameData.instance.homeHP -= enemyAttack;
                    //����UI
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

    //��ײ��˫������Ҫ����Collider,�����˶���һ������rigibody
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
                this.GetComponent<Collider>().enabled = false;  //�ر���ײ
                GameData.instance.killCount += 1;
                GameData.instance.coins += gold;
                uIManager.UpdateBattleData();
                if(collision.gameObject.GetComponent<ProjectileMoveScript>() != null)
                {
                    collision.gameObject.GetComponent<ProjectileMoveScript>().tower.target = null;
                }
                //�����ɱ����������� = ��������������Ϸʤ��
                if(GameData.instance.killCount == GameData.instance.allEnemyCount)
                {
                    uIManager.ShowGameResult(true);
                }
                
                GameObject.Destroy(gameObject, 2.0f);
            }
        }
    }


    /*
     ˫������Collider�������˶�һ������rigidbody������һ����istriggerѡ����Ҫ��ѡ
     */
    //��������������Ч
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
