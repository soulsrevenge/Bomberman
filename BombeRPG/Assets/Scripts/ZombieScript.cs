﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameObject))]
public class ZombieScript : Character //Script de gestion du zombie
{

	public GameObject bomb;
	private GameObject toFollow;
	private Vector3 direction;
	private float hitDelay;
	private float lastHit;
	private bool canMove;
	// Use this for initialization
	protected override void Start ()
	{
		base.Start (3,1,2,3);
		hitDelay = 1;
		lastHit = 0;
		ChoosePlayer();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Move ();
	}
	
	protected override void OnCollisionEnter(Collision hit) 
	{
		base.OnCollisionEnter (hit);
		if (hit.gameObject.layer.Equals (LayerMask.NameToLayer ("Bullet"))) // si zombie touché par shoot
			Hurt (1);
	}

	private void OnCollisionStay(Collision hit)
	{
		if(hit.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
			BitePlayer(hit.gameObject);
	}



	
	private void ChoosePlayer() // IA choisissant le joueur le plus proche
	{
		float distance=-1;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(int i =0; i<players.Length;i++)
		{
			if(distance==-1 || Vector3.Distance(players[i].transform.position,myTransform.position) < distance)
			{
				distance = Vector3.Distance(players[i].transform.position,myTransform.position);
				toFollow = players[i];
			}
		}
	}

	private void BitePlayer(GameObject player)
	{
		Character c = player.GetComponent<PlayerScript>() as PlayerScript;
		if(c!=null)
		{
			if(Time.time > lastHit + hitDelay)
			{	c.Hurt(damage);
				lastHit = Time.time;
			}
		}
	}

	private void Move()
	{
		if(toFollow != null)
		{
			Vector3 playerPos = toFollow.transform.position;
			myBody.velocity=Vector3.zero;
			myTransform.LookAt(new Vector3(playerPos.x, myTransform.position.y , playerPos.z));
			direction =  playerPos - myTransform.position; //Déplacement du zombie selon un IA basique
			direction = direction.normalized;
			myTransform.Translate(direction*moveSpeed*Time.deltaTime,Space.World);

		}
	}

	protected override void Die ()
	{
		Instantiate (bomb, myTransform.position, Quaternion.identity); // on pose une bombe
		base.Die ();
	}

}
