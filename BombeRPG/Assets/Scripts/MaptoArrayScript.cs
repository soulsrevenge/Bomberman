using UnityEngine;
using System.Collections;


//Script qui servira a tramsformer une map en un simple tableau afin d'alleger les transferts
public class MaptoArrayScript : MonoBehaviour
{ 
	public enum id
	{
		EMPTY_BLOCK,
		BIG_UNBREAKABLE_BLOCK,
		MEDIUM_UNBREAKABLE_BLOCK,
		SMALL_UNBREAKABLE_BLOCK,
		BREAKABLE_BLOCK,
		SPIKE,
		SPEEDER,
		BURNING
	}
	
	GameObject child;
	
	id[,,] objectsId;
	GameObject[,,] sceneObjects;
	Vector3 dimensions;
	// Use this for initialization
	void Start ()
	{
		dimensions = RelocateMap ();
		sceneObjects = CreateObjectsArray ();
		objectsId = CreateNumbersArray ();
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	private Vector3 RelocateMap()
	{
		Vector3 minimum = Vector3.zero, 
		maximum = Vector3.zero,
		relocate = Vector3.zero,
		current;
		GameObject[] everything = FindObjectsOfType<GameObject> ();
		current = everything[0].transform.position;
		minimum += new Vector3 (Mathf.Round (current.x), Mathf.Round (current.y), Mathf.Round (current.z));
		maximum += minimum;
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
		return maximum;
	}
	
	private GameObject[,,] CreateObjectsArray()
	{
		Vector3 pos;
		GameObject[] everything = FindObjectsOfType<GameObject> ();
		GameObject[,,] objects = new GameObject[(int)dimensions.x+1, (int)dimensions.y+1, (int)dimensions.z+1];
		for (int i=0; i<everything.Length; i++) 
		{
			if(everything[i].transform.parent == null)
			{
				pos = everything[i].transform.position;
				objects[(int)pos.x,(int)pos.y,(int)pos.z] = everything[i].gameObject;
			}
		}
		
		return objects;
	}
	
	private id[,,] CreateNumbersArray()
	{
		GameObject[] everything = FindObjectsOfType<GameObject> ();
		id[,,] objects = new id[(int)dimensions.x+1, (int)dimensions.y+1, (int)dimensions.z+1];
		Vector3 pos;
		for(int i=0;i<everything.Length;i++)
		{
			if(everything[i].transform.parent == null)
			{
				pos = everything[i].transform.position;
				if(everything[i].gameObject.layer.Equals(LayerMask.NameToLayer("Field")))
				{
					if(everything[i].GetComponentInChildren<BreakableScript>() != null)
						objects[(int)pos.x,(int)pos.y,(int)pos.z] = id.BREAKABLE_BLOCK;
					else
						objects[(int)pos.x,(int)pos.y,(int)pos.z] = id.BIG_UNBREAKABLE_BLOCK;
				}
			}
		}
		return objects;
	}
}
