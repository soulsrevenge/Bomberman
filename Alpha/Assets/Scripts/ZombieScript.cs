using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameObject))]
public class ZombieScript : MonoBehaviour { //Script de gestion du zombie

	public GameObject bomb;
	public float moveSpeed=2f;
	private GameObject toFollow;
	private Vector3 direction;
	private Transform myTransform;
	// Use this for initialization
	void Start ()
	{
		myTransform = this.transform;
		ChoosePlayer();
	}
	
	// Update is called once per frame
	void Update ()
	{
		direction = myTransform.position - toFollow.transform.position; //Déplacement du zombie selon un IA basique
		direction = new Vector3(direction.x,-direction.y,direction.z).normalized;
		myTransform.Translate(direction*moveSpeed*Time.deltaTime);
	}
	
	void OnCollisionEnter(Collision hit) 
	{
		Debug.Log("OMG ZOMBIES ");
		if(hit.gameObject.layer == LayerMask.NameToLayer("Bullet"))// si zombie touché par shoot
		{
			Debug.Log ("Zombie is hitten");
			GameObject obj = Instantiate(bomb,myTransform.position,Quaternion.identity) as GameObject; // on pose une bombe
			GameObject.Destroy(this.gameObject);
			BombScript script = obj.GetComponent("BombScript") as BombScript;
			script.Blow();
		}
		else
		if(hit.gameObject.layer == LayerMask.NameToLayer("Explosion"))// sinon
		{
			Debug.Log ("Zombie is blown");
			GameObject.Destroy(this.gameObject); // zombie detruit mais pas de bombe
		}
	}
	
	void ChoosePlayer() // IA choisissant le joueur le plus proche
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
}
