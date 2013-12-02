using UnityEngine;
using System.Collections;

//Script de géstion de la caméra à la souris
public class CameraScript : MonoBehaviour
{ 
	
	private Vector3 mousePos;
	private Transform myTransform;
	private Transform parentTransform;
	private float rotateSpeed=50f;
	public float zoomSpeed=10f;

	// Use this for initialization
	void Start ()
	{
		mousePos=Vector3.zero;
		myTransform = this.gameObject.transform;
		parentTransform = myTransform.parent.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mousePos==Vector3.zero || !Input.GetMouseButton(0))
			mousePos = Input.mousePosition;
		else
		if(Input.mousePresent && Input.GetMouseButton(0))
		{
			float x = mousePos.x - Input.mousePosition.x;
			float y = mousePos.y - Input.mousePosition.y;
			
			if(Mathf.Abs(y)>Mathf.Abs(x))
					parentTransform.Rotate(new Vector3(y*rotateSpeed,0,0)*Time.deltaTime, Space.Self);
			else
				if(Mathf.Abs(x)>Mathf.Abs(y))
					parentTransform.Rotate(new Vector3(0,-x*rotateSpeed,0)*Time.deltaTime, Space.World);
			mousePos = Input.mousePosition;
			
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
			myTransform.position += (myTransform.position-parentTransform.position).normalized*zoomSpeed*Time.deltaTime;
		else
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
			myTransform.position -= (myTransform.position-parentTransform.position).normalized*zoomSpeed*Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.F))
			parentTransform.localRotation=Quaternion.identity;

	}

	public Vector3 GetRotation()
	{
		return parentTransform.rotation.eulerAngles;
	}
}
