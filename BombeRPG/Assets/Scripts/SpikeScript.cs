using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {
	
	void OnCollisionEnter(Collision hit)
	{
		if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			GameObject player = hit.transform.gameObject;
			Character c = player.GetComponent<PlayerScript> () as PlayerScript;
			if (c != null)
			{
					if (c.LifePoints > 1)
							c.LifePoints = 1;
					else
							c.Hurt (1);
			}
		}
	}
}
