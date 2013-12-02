using UnityEngine;
using UnityEditor;
using System.Collections;


//Script qui servira a tramsformer une map en un simple tableau afin d'alleger les transferts
public class UnityTool : MonoBehaviour
{ 
	public enum id
	{
		EMPTY_BLOCK,
		UNBREAKABLE_BLOCK,
		BREAKABLE_BLOCK,
		SPIKE,
		SPEEDER
	}

	// structure destinée à contenir les solutions pour un collider resize
	public class Solution
	{
		public int x;
		public int y;
		public int z;
		public int cubes;

		public Solution(int x,int y,int z, int cubes)
		{
			this.x=x;
			this.y=y;
			this.z=z;
			this.cubes=cubes;
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ", " + z + ", "  +cubes + ")";
		}
	}

	GameObject child;
	static id[,,] objectsId;
	static GameObject[,,] sceneObjects;
	static Vector3 dimensions;


	[MenuItem("Tools/Reset Colliders")]
	public static void ResetColliders()
	{
		SetDimensions ();
		CreateObjectsArray();
		for(int i=0;i<dimensions.x;i++)
			for(int j=0;j<dimensions.y;j++)
				for(int k=0;k<dimensions.z;k++)
					if(sceneObjects [i,j,k] != null)
					{
						BoxCollider box = (BoxCollider)sceneObjects [i,j,k].collider;
						box.size = new Vector3(1,1,1);
						Debug.Log (box.size);
					}
	}



	//parcourt les objets de la scène et récupère les dimensions du tableau les représentant (coordonnées positives)
	public static void SetDimensions()
	{
		Vector3 maximum = Vector3.zero,
				current;
		GameObject[] everything = FindObjectsOfType<GameObject> ();
		current = everything[0].transform.position;
		maximum += new Vector3 (Mathf.Round (current.x), Mathf.Round (current.y), Mathf.Round (current.z));
		for (int i=0; i<everything.Length; i++)
		{
			if(everything[i].transform.parent == null)
			{
				current = everything[i].transform.position;
				if(current.x > maximum.x)
					maximum.x = current.x;
				if(current.y > maximum.y)
					maximum.y=current.y;
				if(current.z > maximum.z)
					maximum.z = current.z;
			}
		}

		dimensions = maximum + new Vector3(1,1,1);
	}

	//Place les objets à des coordonnées entières positives pour pouvoir les convertir en tableau
	[MenuItem("Tools/Relocate Items")]
	public static void RelocateMap()
	{
		Vector3 minimum = Vector3.zero, 
		maximum = Vector3.zero,
		relocate = Vector3.zero,
		current;
		GameObject[] everything = FindObjectsOfType<GameObject> ();
		current = everything[0].transform.position;
		minimum += new Vector3 (Mathf.Round (current.x), Mathf.Round (current.y), Mathf.Round (current.z));
		maximum += minimum;
		//on cherche un minimum et on arrondit les positions
		for (int i=0; i<everything.Length; i++)
		{
			current = everything[i].transform.position;
			if(everything[i].transform.parent == null)
				everything[i].transform.position=new Vector3(Mathf.Round(current.x),Mathf.Round(current.y),Mathf.Round(current.z));
			if(current.x < minimum.x)
				minimum.x = (float)Mathf.RoundToInt(current.x);
			if(current.y < minimum.y)
				minimum.y = (float)Mathf.RoundToInt(current.y);
			if(current.z < minimum.z)
				minimum.z = (float)Mathf.RoundToInt(current.z);
		}

		//on prend la valeur absolue du minimum trouvé
		if (minimum.x < 0)
			relocate.x += Mathf.Abs (minimum.x);
		else
			relocate.x = -minimum.x;
		if (minimum.y < 0)
			relocate.y += Mathf.Abs (minimum.y);
		else
			relocate.y = -minimum.y;
		if(minimum.z < 0)
			relocate.z +=  Mathf.Abs (minimum.z);
		else
			relocate.z = -minimum.z;
		
		//enfin on décale tous les objets en récupérant au passage la coordonnée max
		for(int i=0; i<everything.Length; i++)
		{
			if(everything[i].transform.parent == null)
			{
				current = (everything[i].transform.position += relocate);
				if(current.x > maximum.x)
					maximum.x = current.x;
				if(current.y > maximum.y)
					maximum.y=current.y;
				if(current.z > maximum.z)
					maximum.z = current.z;
			}
		}
		Debug.Log(relocate);
		dimensions = maximum + new Vector3(1,1,1);
	}


	//crée un tableau ordonné d'objets, où ils correspondent à leurs coordonnées
	public static void CreateObjectsArray()
	{
		Vector3 pos;
		GameObject[] everything = FindObjectsOfType<GameObject> ();
		GameObject[,,] objects = new GameObject[(int)dimensions.x, (int)dimensions.y, (int)dimensions.z];
		for (int i=0; i<everything.Length; i++) 
		{
			if(everything[i].transform.parent == null)
			{
				pos = everything[i].transform.position;
				objects[(int)pos.x,(int)pos.y,(int)pos.z] = everything[i].gameObject;
			}
		}
		sceneObjects = objects;
	}



	/*
	 * Les fonctions qui suivent n'ont pas pu être exploitées par manque de temps mais visaient 
	 * à être intégrées à l'outil afin d'optimiser le terrain en redimensionnant les colliders 
	 * de manière optimale, automatiquement:
	 */

	//renvoie une solution de redimensionnement pour une ligne de cubes
	public static Solution Solution1D(int x, int y, int z)
	{
		int width=1;
		if (sceneObjects [x, y, z] == null)
			return new Solution (0, 0, 0, 0);
		else
			// tant que le bout de la lgine n'est pas arrivé, on compare avec l'objet d'après
			for (int i = x; i<dimensions.x-1 && sceneObjects [i,y,z] != null; i++)
				if(sceneObjects [i+1,y,z] != null && sceneObjects [i,y,z].name.Equals(sceneObjects [i+1,y,z].name))
					width++;
		return new Solution (width,1,1,width);
	}
	
	//renvoie une solution de redimensionnement pour un plan optimal de cubes
	public static Solution Solution2D(int x,int y, int z)
	{
		int width=-1,height=1,wtemp=1;
		if (sceneObjects [x, y, z] == null)
			return new Solution (0, 0, 0, 0);
		else
			// on parcourt en montant
			for(int j = y; j<dimensions.y-1; j++)
		{	
			wtemp=1;
			//a chaque étage on regarde si la ligne donne plus de cubes
			for (int i = x; i<dimensions.x-1 && sceneObjects [i,j,z] != null; i++)
				if(sceneObjects [i+1,j,z] != null && sceneObjects [i,j,z].name.Equals(sceneObjects [i+1,j,z].name))
					wtemp++;
			if(wtemp<width)
			{	
				if(width*height < wtemp*height)
					width=wtemp;
				else
					//si on perd des cubes avec ce chemin on revient en arrière et on arrête
					return new Solution(width,height-1,z,width*(height-1));
			}
			else
				if(width==-1)
					width=wtemp;
			height++;
		}
		Debug.Log ("here");
		return new Solution(width, height,z, width*height);
	}


}
