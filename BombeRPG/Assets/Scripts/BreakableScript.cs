using UnityEngine;
using System.Collections;

public class BreakableScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	//les objects destructible réagissent aux explosions
	void OnCollisionEnter(Collision hit)
	{
		if(hit.gameObject.layer == LayerMask.NameToLayer("Explosion"))
		{
			Debug.Log ("Breakable is hit");
			GameObject.Destroy(this.gameObject);
			GameObject.Destroy (hit.gameObject);
		}
	}

}
