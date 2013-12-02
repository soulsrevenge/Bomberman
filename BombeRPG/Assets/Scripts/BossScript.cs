using UnityEngine;
using System.Collections;

public class BossScript : Character 
{

	// Use this for initialization
	protected override void Start () 
	{
		base.Start (50,0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void OnCollisionEnter(Collision hit) 
	{
		//10 degats pour une bombe
		if (hit.gameObject.layer.Equals (LayerMask.NameToLayer ("Explosion")))
		{
			BombScript b = hit.gameObject.transform.parent.GetComponent<BombScript>() as BombScript;
			if(b.StartExplosion)
				Hurt (10);
		}

		//1 degat pour une balle
		if (hit.gameObject.layer.Equals (LayerMask.NameToLayer ("Bullet")))
				Hurt (1);
		Debug.Log (lifePoints);
	}
}
