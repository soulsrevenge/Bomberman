using UnityEngine;
using System.Collections;



public class MaptoArrayScript : MonoBehaviour { //Script qui servira a tramsformer une map en un simple tableau afin d'alleger les transferts

	public const int EMPTY_BLOCK = 0;
	public const int UNBREAKABLE_BLOCK = 1;
	public const int BREAKABLE_BLOCK = 2;
	GameObject child;
	
	int[,,] maparray;
	int[,,] temp = new int[100,100,	100];
	int taille = 1;
	int xmax = 0;
	int ymax = 0;
	int zmax = 0;
	int x = 0;
	int y = 0;
	int z = 0;
	// Use this for initialization
	void Start () {
		Component[] children = this.gameObject.GetComponentsInChildren<Component>();
	    for(int i=0;i<children.Length;i++)
	    {
			child = children[i].gameObject;	
	     	if(child.layer == LayerMask.NameToLayer("Field"))
	     	{
				x = (int)child.transform.position.x/taille;
				y = (int)child.transform.position.y/taille;
				z = (int)child.transform.position.z/taille;
				if(child.GetComponentInChildren<BreakableScript>() != null)
				{
					temp[x,y,z] = BREAKABLE_BLOCK;
				}
				else temp[x,y,z] = UNBREAKABLE_BLOCK;
	    	}
	    }
		for(int i=0;i<100;i++)
		{
			for(int j=0;j<100;j++)
			{
				for(int k=0;k<100;k++)
				{
					if(temp[i,j,k]!=0)
					{
						if(i>xmax) xmax=i;
						if(j>ymax) ymax=j;
						if(k>zmax) zmax=k;
					}
				}
			}
		}
		xmax++;
		ymax++;
		zmax++;
		Debug.Log("xmax ="+xmax);
		Debug.Log("ymax ="+ymax);
		Debug.Log("zmax ="+zmax);
		
		maparray = new int[xmax,ymax,zmax];
		for(int i=0;i<xmax;i++)
		{
			for(int j=0;j<ymax;j++)
			{
				for(int k=0;k<zmax;k++)
				{
					maparray[i,j,k] = temp[i,j,k];
					Debug.Log(maparray[i,j,k]);
				}
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
