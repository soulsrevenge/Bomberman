using UnityEngine;
using System.Collections;

public class EditorScript: MonoBehaviour
{

	private class EventList
	{
		private int maxElts;
		private ArrayList elements;

		public EventList(int max)
		{
			maxElts = max;
			elements = new ArrayList();
		}

		public void Add(GameObject o)
		{
			if (elements.Count == maxElts)
					elements.RemoveAt (0);
			elements.Add (o);
		}

		public void RemoveLast()
		{
			if (elements.Count > 0) 
			{
				GameObject.Destroy (elements [elements.Count - 1] as GameObject);
				elements.RemoveAt (elements.Count - 1);
			}
		}

	}

	public Texture[] textures;
	public GameObject[] objects;
	public Camera[] cam;
	private int toCreate = 0;

	private Material myMaterial;
	private GameObject currentObject=null;
	private EventList events;
	private Vector3 position;
	private Vector3 rotation;

	// Use this for initialization
	void Start ()
	{
		events = new EventList (20);
		position = Vector3.zero;
		rotation = Vector3.zero;
		myMaterial = this.transform.renderer.material;
		myMaterial.mainTexture = textures [toCreate];
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hitInfo = new RaycastHit ();

		if (Input.GetMouseButtonDown(0))
		{
			//Interactions avec le menu éditeur
			Ray r = cam[0].ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(r,out hitInfo))
			{
			
				if(hitInfo.transform.name.Equals("LeftArrow"))
				{
					if(toCreate>0)
						toCreate--;
					else
						toCreate=textures.Length-1;

					myMaterial.mainTexture = textures[toCreate];
					rotation=Vector3.zero;
					if(currentObject != null)
					{
						position=currentObject.transform.position;
						events.Add (currentObject.transform.gameObject);
						currentObject=null;
					}
				}
				else if(hitInfo.transform.name.Equals("RightArrow"))
				{
					if(toCreate<textures.Length-1)
						toCreate++;
					else
						toCreate=0;

					myMaterial.mainTexture = textures[toCreate];
					rotation=Vector3.zero;
					if(currentObject != null)
					{
						position=currentObject.transform.position;
						events.Add (currentObject.transform.gameObject);
						currentObject=null;
					}
				}

			}

			//Interactions avec les objets déjà en place
			r = cam[1].ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(r,out hitInfo))
			{
				currentObject=hitInfo.transform.gameObject;
			}

		}

		//Validation ou création d'un objet
		if (Input.GetKeyDown (KeyCode.Return))
		{
			if(currentObject==null)
				currentObject = Instantiate(objects[toCreate], position, Quaternion.Euler(rotation)) as GameObject;
			else
			{
				position=currentObject.transform.position;
				events.Add (currentObject.transform.gameObject);
				currentObject = null;
			}
		}

		if(currentObject!=null)
		{
			if(Input.GetKey(KeyCode.R))
			{
				//Rotation de l'objet en cours
				if (Input.GetKeyDown (KeyCode.LeftArrow))
					RotateObject(currentObject.transform,new Vector3(0,-90,0));
				if (Input.GetKeyDown (KeyCode.RightArrow))
					RotateObject(currentObject.transform,new Vector3(0,90,0));
				if (Input.GetKeyDown (KeyCode.UpArrow))
					RotateObject(currentObject.transform,new Vector3(90,0,0));
				if (Input.GetKeyDown (KeyCode.DownArrow))
					RotateObject(currentObject.transform,new Vector3(-90,0,0));
			}
			else
			//Déplacements gauche, droite, haut, bas de l'objet en cours
			{
				CameraScript obj = cam[1].GetComponent ("CameraScript") as CameraScript;
				Vector3[] vectors;
				if(obj.GetRotation ().y < 65)
				{
					vectors = new Vector3[6]
					{
						new Vector3(-1,0,0),		//LeftArrow
						new Vector3(1,0,0),			//RightArrow
						new Vector3(0,1,0),			//UpArrow + Shift
						new Vector3(0,-1,0),		//DownArrow + Shift
						new Vector3(0,0,1),			//UpArrow
						new Vector3(0,0,-1)			//DownArrow
					};
					MoveObject(currentObject.transform,vectors);
				}
				else if(obj.GetRotation().y < 160 )
				{
					vectors = new Vector3[6]
					{
						new Vector3(0,0,1),
						new Vector3(0,0,-1),
						new Vector3(0,1,0),
						new Vector3(0,-1,0),
						new Vector3(1,0,0),
						new Vector3(-1,0,0)
					};
					MoveObject(currentObject.transform,vectors);
				}
				else if(obj.GetRotation().y <225)
				{
					vectors = new Vector3[6]
					{
						new Vector3(1,0,0),		
						new Vector3(-1,0,0),	
						new Vector3(0,1,0),		
						new Vector3(0,-1,0),	
						new Vector3(0,0,-1),	
						new Vector3(0,0,1)		
					};
					MoveObject(currentObject.transform,vectors);
				}
				else 
				{
					vectors = new Vector3[6]
					{
						new Vector3(0,0,-1),
						new Vector3(0,0,1),
						new Vector3(0,1,0),
						new Vector3(0,-1,0),
						new Vector3(-1,0,0),
						new Vector3(1,0,0)
					};
					MoveObject(currentObject.transform,vectors);
				}

				
			}

			//Suppression de l'objet sélectionné
			if (Input.GetKeyDown (KeyCode.Backspace))
			{
				GameObject.Destroy(currentObject);
				currentObject=null;
			}
		}

		//gestion du Ctrl+z
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.Z))
		{
			if(currentObject!=null)
				GameObject.Destroy(currentObject);
			else
				events.RemoveLast();
		}

	}

	private void RotateObject(Transform t, Vector3 angle)
	{
		t.Rotate(angle, Space.World);
		rotation+=angle;
		rotation=new Vector3(rotation.x%360,rotation.y%360, rotation.z%360);
	}

	private void MoveObject(Transform t, Vector3[] move)
	{
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			currentObject.transform.Translate(move[0], Space.World);
		if (Input.GetKeyDown (KeyCode.RightArrow))
			currentObject.transform.Translate(move[1], Space.World);
		
		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			if(Input.GetKey(KeyCode.LeftShift))
				currentObject.transform.Translate(move[2], Space.World);
			else
				currentObject.transform.Translate(move[4], Space.World);
		}
		
		if (Input.GetKeyDown (KeyCode.DownArrow))
		{
			if(Input.GetKey(KeyCode.LeftShift))
				currentObject.transform.Translate(move[3], Space.World);
			else
				currentObject.transform.Translate(move[5], Space.World);
		}
	}
}
