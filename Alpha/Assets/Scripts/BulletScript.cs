using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class BulletScript : MonoBehaviour
{
	
	private float created;
	
	public float shootSpeed = 50f;
	// Use this for initialization
	void Start ()
	{
		created = Time.time;
		this.gameObject.rigidbody.AddRelativeForce(new Vector3(0,0,shootSpeed),ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.time - created > 1)
			GameObject.Destroy(this.gameObject);

	}
	
	
	void OnCollisionEnter(Collision hit)
	{
		if(hit.gameObject.layer == LayerMask.NameToLayer("Field"))
		{
			Debug.Log("Bullet hits field");
			GameObject.Destroy(this.gameObject);
		}
	}
}
