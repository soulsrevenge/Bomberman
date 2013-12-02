using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameObject))]
public class ArraytoMapScript : MonoBehaviour // Script qui servira a creer des map a partir d'une liste
{
	int xmax = 3;
	int ymax = 3;
	int zmax = 3;
	int[,,] arraymap;
	
	[SerializeField]
	private int taille;

	public int Taille {
		get {
			return this.taille;
		}
		set {
			taille = value;
		}
	}

	public GameObject breakable;
	public GameObject unbreakable;
	// Use this for initialization
	
	void Start () {
		
		arraymap = new int[xmax,ymax,zmax];
		arraymap[1,1,1] = 1;
		arraymap[0,1,1] = 2;
		arraymap[1,1,0] = 2;
		arraymap[1,0,1] = 2;
		arraymap[2,1,1] = 2;
		arraymap[1,1,2] = 2;
		arraymap[1,2,1] = 2;
		
		for(int i=0;i<xmax;i++)
		{
			for(int j=0;j<ymax;j++)
			{
				for(int k=0;k<zmax;k++)
				{
					switch(arraymap[i,j,k])
					{
					case 1 : Instantiate(unbreakable,new Vector3(i,j,k)*taille,Quaternion.identity);
						break;
					case 2 : Instantiate(breakable,new Vector3(i,j,k)*taille,Quaternion.identity);
						break;
					}
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
