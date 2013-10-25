using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour { //Script de géstion de la caméra à la souris
	
	private Vector3 mousePos;
	private Transform myTransform;
	private float axis;
	private bool reversed=false;
	public float rotateSpeed=0.1f;
	public float tolerance=20;

	// Use this for initialization
	void Start ()
	{
		if(reversed)
			axis = -1;
		else
			axis = 1;
		mousePos=Vector3.zero;
		myTransform = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mousePos==Vector3.zero)
			mousePos = Input.mousePosition;
		else
		if(Input.mousePresent)
		{
			float x = mousePos.x - Input.mousePosition.x;
			float y = mousePos.y - Input.mousePosition.y;
			
			if(y > tolerance || y < -tolerance)
				myTransform.Rotate(new Vector3(-y*axis*rotateSpeed,0,0)*Time.deltaTime);
			else
			if(x > tolerance || x < -tolerance)
				myTransform.Rotate(new Vector3(0,x*axis*rotateSpeed,0)*Time.deltaTime);
			mousePos = Input.mousePosition;
			
		}
	}
}
