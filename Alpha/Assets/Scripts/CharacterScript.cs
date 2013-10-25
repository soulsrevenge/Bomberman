using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GameObject))]
public class CharacterScript : MonoBehaviour 
{
	
	private Transform myTransform;
	private Rigidbody myBody;
	private float lastShoot=0;
	private Transform shootSpawn;
	
	public GameObject prefab;
	public static float jumpSpeed = 0.4f;
	public static float moveSpeed = 5f;
	public static float maxVelocity = 4f;
	// Use this for initialization
	void Start ()
	{
		myTransform = this.transform;
		myBody = this.rigidbody;
		Component[] children = this.gameObject.GetComponentsInChildren<Component>();
		for(int i=0; i<children.Length; i++)
		{
			if(children[i].gameObject.layer != LayerMask.NameToLayer("Spawner"))
			{
				shootSpawn = children[i].gameObject.transform;
				break;
			}
		}
	}
	
	void OnCollisionEnter(Collision hit)
	{
		if(hit.gameObject.layer == LayerMask.NameToLayer("Explosion"))
	   	GameObject.Destroy(this.gameObject); 
	}
	public void MoveFront() // Fonction pour déplacer les joueurs et tirer
	{
		myTransform.Translate(new Vector3(0,0,1) * moveSpeed * Time.deltaTime,Space.Self);
	}
	public void MoveBack()
	{
		myTransform.Translate(new Vector3(0,0,-1) * moveSpeed * Time.deltaTime,Space.Self);
	}
	public void MoveRight()
	{
		myTransform.localEulerAngles += new Vector3(0,90,0);
	}
	public void MoveLeft()
	{
		myTransform.localEulerAngles -= new Vector3(0,90,0);
	}
	public void MoveUp()
	{
		if(myBody.velocity.y < maxVelocity)
				myBody.AddForce(new Vector3(0,jumpSpeed,0),ForceMode.VelocityChange);
	}
	public void Shoot()
	{
		if(Time.time - lastShoot > 0.2)
			{
				Debug.Log ("SHOOT!");
				Network.Instantiate (prefab, shootSpawn.position, myTransform.rotation,0);
				lastShoot = Time.time;
			}
	}
	
}
