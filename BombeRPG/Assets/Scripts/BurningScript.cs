using UnityEngine;
using System.Collections;

public class BurningScript : MonoBehaviour {

	private float lastHit = 0;
	private float hitDelay = 1;
	private int damage = 3;

	void OnCollisionEnter(Collision hit)
	{

		if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			GameObject player = hit.transform.gameObject;
			Character c = player.GetComponent<PlayerScript>() as PlayerScript;
			if(c!=null)
			{
				if(Time.time > lastHit + hitDelay)
				{	c.Hurt(damage);
					lastHit = Time.time;
				}
			}
		}
	}

}
