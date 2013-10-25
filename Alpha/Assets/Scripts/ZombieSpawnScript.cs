using UnityEngine;
using System.Collections;

public class ZombieSpawnScript : MonoBehaviour { // Script de gestion du spawner de zombies

	private Transform myTransform;
	public float delay = 3f; //un zombie toute les 3 secondes
	private float lastZomb = 2f;// avec le premier dés la 1ere seconde
	
	[SerializeField]
	public GameObject zomb;
	
	void Start()
	{
	  myTransform = this.gameObject.transform;
	}
	void Update()
	{
	   if(Time.time - lastZomb > delay)
	   {
	       Instantiate(zomb, myTransform.position, Quaternion.identity);
	       lastZomb=Time.time;
	   }
	}
}
