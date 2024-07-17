using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript : MonoBehaviour {

    public bool rotate = false;
    public float rotateAmount = 45;
    public bool bounce = false;
    public float bounceForce = 10;
    public float speed;
	public float fireRate;
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;
	public List<GameObject> trails;

    private Vector3 startPos;
	private float speedRandomness;
	private bool collided;
	private Rigidbody rb;
    private RotateToMouseScript rotateToMouse;
    private GameObject target;
    //获得发射子弹的炮塔
    public Tower tower;

	void Start () {
        startPos = transform.position;
        rb = GetComponent <Rigidbody> ();

        //枪口的特效啊 如果不等于空 
		if (muzzlePrefab != null) {
            //进行实例化
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
            //设置方向
			muzzleVFX.transform.forward = gameObject.transform.forward;
            //删除枪口特效 根据粒子组件上配置的时间
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}
	}

	void FixedUpdate () {
        //如果目标不等于空 插值旋转 注视目标
        if (target != null)
            rotateToMouse.RotateToMouse (gameObject, target.transform.position);
        //如果进行旋转 则根据配置进行旋转
        if (rotate)
            transform.Rotate(0, 0, rotateAmount, Space.Self);
        //移动控制 当速度不等于0 并且刚体不等于空的时候 
        if (speed != 0 && rb != null)
			rb.position += (transform.forward) * (speed * Time.deltaTime);   
    }

    //当碰撞的时候
	void OnCollisionEnter (Collision co) {
        //如果不进行反弹
        if (!bounce)
        {
            //判断标签 是子弹的话 并且未进行计算碰撞 collided通过该标志位控制 防止进行多次碰撞计算,而导致类似重复扣血的bug
            if (co.gameObject.tag != "Bullet" && !collided)
            {
                collided = true;
                //拖尾 轨迹 先停止 销毁
                if (trails.Count > 0)
                {
                    for (int i = 0; i < trails.Count; i++)
                    {
                        trails[i].transform.parent = null;
                        var ps = trails[i].GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            ps.Stop();
                            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                        }
                    }
                }
                
                speed = 0;
                GetComponent<Rigidbody>().isKinematic = true;

                ContactPoint contact = co.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                //如果碰撞后 要生成的特效不等于空 类似爆炸 火花等 
                if (hitPrefab != null)
                {
                    //则实例化生成碰撞的特效
                    var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;
                    //然后根据碰撞预设上粒子模块的配置时间进行销毁 碰撞后生成的粒子
                    var ps = hitVFX.GetComponent<ParticleSystem>();
                    if (ps == null)
                    {
                        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVFX, psChild.main.duration);
                    }
                    else
                        Destroy(hitVFX, ps.main.duration);
                }
                //再销毁自己
                StartCoroutine(DestroyParticle(0f));
            }
        }
        else
        {
            rb.useGravity = true;
            rb.drag = 0.5f;
            ContactPoint contact = co.contacts[0];
            //通过传递入向量和法向量 得到反射的方向 *力度 则可以让粒子按反射的方向进行运动
            rb.AddForce (Vector3.Reflect((contact.point - startPos).normalized, contact.normal) * bounceForce, ForceMode.Impulse);
            //销毁该脚本
            Destroy ( this );
        }
	}

    //销毁
	public IEnumerator DestroyParticle (float waitTime) {
        //子物体数量大于0 并且等待时间不等于0
		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		
            //等待所有的子物体缩放都为0
			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
	
		yield return new WaitForSeconds (waitTime);
        //再进行删除该物体
        Destroy(gameObject);
	}

    public void SetTarget (GameObject trg, RotateToMouseScript rotateTo)
    {
        target = trg;
        rotateToMouse = rotateTo;
    }
}
