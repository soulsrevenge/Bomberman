using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {
	
	private Transform myTransform;
	private bool startExplosion = false;
	public bool StartExplosion 
	{
		get { return startExplosion; }
		set { startExplosion = value; }
	}

	public float moveSpeed = 10f;
	public float range = 20;
	public float timer = 2;
	// Use this for initialization
	void Start ()
	{
		myTransform = this.transform;
		Init();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(startExplosion)
		{
				Component[] children = this.gameObject.GetComponentsInChildren<Component>();
				for(int i=0;i<children.Length;i++)
				{
					if(children[i].gameObject.layer == LayerMask.NameToLayer("Explosion"))
					{
						children[i].gameObject.GetComponentInChildren<Renderer>().enabled=true;
						if(children[i].gameObject.transform.position.x - myTransform.position.x > range ||
						   children[i].gameObject.transform.position.y - myTransform.position.y > range ||
						   children[i].gameObject.transform.position.z - myTransform.position.z > range
						  )
						{
							startExplosion = false;
							GameObject.Destroy(this.gameObject);
							break;
						}
						children[i].gameObject.transform.Translate(new Vector3(0,0,moveSpeed)*Time.deltaTime,Space.Self);
					}
				}
		}
	}
	
	public void Init()
	{
		StartCoroutine(Blow(timer));
	}
	
	IEnumerator Blow(float sec)
	{
		yield return new WaitForSeconds(sec);
		startExplosion = true;
	}
}
