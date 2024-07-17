using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GoldPickUpPack
{

	public class OverviewManager : MonoBehaviour {
		public Transform main_camera;
		public TextMesh text_fx_name;
		public Transform[] cam_positions;
		public GameObject[] gold_prefabs;
		public GameObject[] pickingup_vfx_prefabs;
		public AudioSource[] gold_sfx;
		public float spawn_cooldown = 0.5f;


		private Ray ray;
		private RaycastHit ray_cast_hit;
		public int count_to_end_demo = 150;


		void Start () {
			StartCoroutine("SpawnGold");
			Destroy(GameObject.Find("Instructions"), 14.5f);
		}
		

		void Update () {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if ( Physics.Raycast (ray.origin, ray.direction, out ray_cast_hit, 1000f) )			
			{
				if(ray_cast_hit.transform.tag == "gold")
				{
					PickUpGold( ray_cast_hit.transform.name, ray_cast_hit.transform.position, ray_cast_hit.transform.gameObject);	
				}
			}
			//////////////////
			//Keyboard Stuff..
			if ( Input.GetKeyDown("1") )
			{
				print("asdasdas 1");
				main_camera.position = cam_positions[0].position;
				main_camera.rotation = cam_positions[0].rotation;
			}
			if ( Input.GetKeyDown("2") )
			{
				main_camera.position = cam_positions[1].position;
				main_camera.rotation = cam_positions[1].rotation;
			}
			if ( Input.GetKeyDown("3") )
			{
				main_camera.position = cam_positions[2].position;
				main_camera.rotation = cam_positions[2].rotation;
			}
			if ( Input.GetKeyDown("r") )
			{
				SceneManager.LoadScene("Overview");
			}
			//Keyboard Stuff/////////////
			/////////////////////////////			
		}


		IEnumerator SpawnGold()
	    {
	    	bool GO = true;
	    	int random_prefab;	
	    	yield return new WaitForSeconds(2);
	    	while(GO){
	    		random_prefab = Random.Range(0,15);
	    		yield return new WaitForSeconds(spawn_cooldown);
	        	GameObject g = Instantiate(gold_prefabs[ random_prefab ], new Vector3(Random.Range(-5.62f,5.62f), 0, Random.Range(-5.84f,4.64f)), transform.rotation);
	        	g.name = gold_prefabs[ random_prefab ].name;
	        	count_to_end_demo--;
	        	if(count_to_end_demo <= 0)
	        		GO = false;
	    	}
	        
	    }

	    void PickUpGold(string name, Vector3 gold_pos, GameObject gold_o)
	    {
	    	PlayGoldSoundFX();
	    	GameObject g;
	    	Destroy(gold_o);
	    	switch(name){
	    		case "Coin":
	    			g = Instantiate(pickingup_vfx_prefabs[ 0 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 0 ].name;
	    			break;
	    		case "Coin Scrambled 1":
	    			g = Instantiate(pickingup_vfx_prefabs[ 1 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 1 ].name;
	    			break;
	    		case "Coin Scrambled 2":
	    			g = Instantiate(pickingup_vfx_prefabs[ 2 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 2 ].name;
	    			break;
	    		case "Coin Scrambled 3":
	    			g = Instantiate(pickingup_vfx_prefabs[ 3 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 3 ].name;
	    			break;
	    		case "Coint Stack 1":
	    			g = Instantiate(pickingup_vfx_prefabs[ 4 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 4 ].name;
	    			break;
	    		case "Coint Stack 2":
	    			g = Instantiate(pickingup_vfx_prefabs[ 5 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 5 ].name;
	    			break;
	    		case "Coint Stack 3":
	    			g = Instantiate(pickingup_vfx_prefabs[ 6 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 6 ].name;
	    			break;
	    		case "Coint Stack 4":
	    			g = Instantiate(pickingup_vfx_prefabs[ 7 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 7 ].name;
	    			break;
	    		case "Ingot":
	    			g = Instantiate(pickingup_vfx_prefabs[ 8 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 8 ].name;
	    			break;
	    		case "Ingot Stack":
	    			g = Instantiate(pickingup_vfx_prefabs[ 9 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 9 ].name;
	    			break;
	    		case "Mixed 1":
	    			g = Instantiate(pickingup_vfx_prefabs[ 10 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 10 ].name;
	    			break;
	    		case "Mixed 2":
	    			g = Instantiate(pickingup_vfx_prefabs[ 11 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 11 ].name;
	    			break;
	    		case "Mixed 3":
	    			g = Instantiate(pickingup_vfx_prefabs[ 12 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 12 ].name;
	    			break;
	    		case "Mixed 4":
	    			g = Instantiate(pickingup_vfx_prefabs[ 13 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 13 ].name;
	    			break;
	    		case "Mixed 5":
	    			g = Instantiate(pickingup_vfx_prefabs[ 14 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 14 ].name;
	    			break;
	    		default:
	    			g = Instantiate(pickingup_vfx_prefabs[ 14 ], gold_pos, transform.rotation);
	        		g.name = gold_prefabs[ 14 ].name;
	    			break;
	    	}
	    }


	    void PlayGoldSoundFX()
	    {
	    	int i = Random.Range(0, gold_sfx.Length);
	    	gold_sfx[i].Play();
	    }
	}
	
}