using UnityEngine;
using System.Collections;

public class ZombieSpawnScript : MonoBehaviour { // Script de gestion du spawner de zombies

	private Transform myTransform;
	public float delay = 10f; //un zombie toute les 10 secondes
	private float lastZomb = 9f;// avec le premier dés la 1ere seconde
	public float upGradeTime = 30f;// reduction du delay d'une seconde toutes les 30 secondes
	private float lastUpgrade;// donc les zombies apparaitront de plus en plus vite
	private float startTime;
	
	[SerializeField]
	public GameObject zomb;
	
	void Start()
	{
	  myTransform = this.gameObject.transform;
		lastUpgrade = Time.time;
		startTime = Time.time;
	}
	void Update()
	{
	   if(Time.time - lastZomb > delay)
	   {
	       Instantiate(zomb, myTransform.position, Quaternion.identity);
	       lastZomb=Time.time;
	   }
		if (Time.time - lastUpgrade > upGradeTime) 
		{
			lastUpgrade = Time.time;
			if (delay > 1)
				delay = delay - 1;
		}
		if (Time.time - startTime > 300)
						delay = 0.5f;
	}
}
