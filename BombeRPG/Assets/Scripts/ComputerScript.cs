using UnityEngine;
using System.Collections;

public class ComputerScript : PlayerScript
{
	private struct Aim
	{
		public int index;
		public GameObject obj;
		public Aim(int val, GameObject o)
		{
			index = val;
			obj = o;
		}
	}

	private float lastTime = 0;
	private float waitTime = 1;
	private GameObject target;
	private Vector3 direction;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start (5,1,2,2);
		rotateSpeed = 90f;
		direction = Vector3.zero;
		Component[] children = this.gameObject.GetComponentsInChildren<Component>();
		for(int i=0; i<children.Length; i++)
		{
			if(children[i].gameObject.tag.Equals("PlayerSpawner"))
			{
				shootSpawn = children[i].gameObject.transform;
				break;
			}
		}
		LocateElements ();
	}
	
	// Update is called once per frame
	public override void FixedUpdate () 
	{
		if(target != null)
			if(target.layer.Equals (LayerMask.NameToLayer("NPC")))
				Attack();

		if (Time.time - lastTime > waitTime || target == null)
		{
			lastTime = Time.time;
			LocateElements();
		}
		else
			Move (target);

	}

	public override void MoveLeft()
	{
		myTransform.Rotate (new Vector3 (0, -90, 0), Space.Self);
	}

	public override void MoveRight()
	{
		myTransform.Rotate (new Vector3 (0, 90, 0), Space.Self);
	}

	private void LocateElements()
	{
		Ray[] detectors = new Ray[6]
		{
			new Ray (myTransform.position, Vector3.forward),
			new Ray (myTransform.position, Vector3.back),
			new Ray (myTransform.position, Vector3.up),
			new Ray (myTransform.position, Vector3.down),
			new Ray (myTransform.position, Vector3.left),
			new Ray (myTransform.position, Vector3.right)
		};
		RaycastHit hitInfo;
		ArrayList hitList = new ArrayList ();
		for (int i=0; i<detectors.Length; i++)
		{
			if(Physics.Raycast(detectors[i], out hitInfo))
			{
				if(hitInfo.transform.gameObject.layer.Equals(LayerMask.NameToLayer("NPC")))
					target = hitInfo.transform.gameObject;
				if(Vector3.Distance(hitInfo.transform.position,myTransform.position) > 1)
					hitList.Add(hitInfo.transform.gameObject);
			}
		}
		if (hitList.Count > 0)
		{
			int random = Random.Range (0, hitList.Count);
			target = hitList [random] as GameObject;
		} else
			target = null;
	}

	private void Move(GameObject toFollow)
	{
		if(toFollow != null)
		{
			Vector3 position = toFollow.transform.position;
			myBody.velocity=Vector3.zero;
			direction =  position - myTransform.position; //Déplacement du zombie selon un IA basique
			direction = direction.normalized;
			myTransform.LookAt(new Vector3(position.x, myTransform.position.y , position.z));
			myTransform.Translate(direction.normalized*moveSpeed*Time.deltaTime,Space.World);
			
		}
	}

	private void Attack()
	{
		myTransform.LookAt (target.transform);
		Shoot ();
	}
}
