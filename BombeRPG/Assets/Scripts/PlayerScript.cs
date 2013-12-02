using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GameObject))]
public class PlayerScript : Character
{	
	public GameObject prefab;
	[SerializeField]
	private Camera[] cameras;
	private int activeCamera;
	protected float jumpSpeed = 0.4f;
	protected float rotateSpeed = 90f;
	protected float lastShoot=0;
	protected Transform shootSpawn;
	public Rect windowRect;
	private int windowLeft = 10;
	private int windowTop = 175;
	private int windowWidth = 200;
	private int windowHeight = 50;
	private GUIStyle myStyle = new GUIStyle();




	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start (5,1,2,8);
		activeCamera = 0;
		Component[] children = this.gameObject.GetComponentsInChildren<Component>();
		for(int i=0; i<children.Length; i++)
		{
			if(children[i].gameObject.tag.Equals("PlayerSpawner"))
			{
				shootSpawn = children[i].gameObject.transform;
				break;
			}
		}
	}


	public virtual void FixedUpdate () 
	{
			
			if (Input.GetKey (KeyCode.Z))
				MoveFront ();
			if (Input.GetKey (KeyCode.S))
				MoveBack ();
			if (Input.GetKeyUp (KeyCode.Z) || Input.GetKeyUp(KeyCode.S))
				myBody.velocity = Vector3.zero;
			if (Input.GetKey(KeyCode.Q))
				this.MoveLeft ();
			if (Input.GetKey(KeyCode.D))
				this.MoveRight ();
			if (Input.GetKey (KeyCode.Space))
				MoveUp ();
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				Shoot ();
			if (Input.GetKeyDown (KeyCode.V))
			{
				if(activeCamera<cameras.Length-1)
					activeCamera++;
				else
				activeCamera=0;
				switchCameras (cameras, activeCamera);
			}

			
	}
	public void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
						Time.timeScale = 0;
		if(Input.GetKeyUp (KeyCode.Escape))
		   Time.timeScale = 1;
	}
	
	public void MoveFront() // Fonction pour déplacer les joueurs et tirer
	{
		//myTransform.Translate(new Vector3(0,0,1) * moveSpeed * Time.deltaTime,Space.Self);
		if(myTransform.InverseTransformDirection(myBody.velocity).z < maxVelocity)
		   myBody.AddRelativeForce (0,0,moveSpeed,ForceMode.Impulse);
	}
	public void MoveBack()
	{
		//myTransform.Translate(new Vector3(0,0,-1) * moveSpeed * Time.deltaTime,Space.Self);
		if(myTransform.InverseTransformDirection(myBody.velocity).z > -maxVelocity)
			myBody.AddRelativeForce (0,0,-moveSpeed,ForceMode.Impulse);
	}
	public virtual void MoveRight()
	{
		myBody.velocity = new Vector3(0, myBody.velocity.y, 0);
		myTransform.localEulerAngles += new Vector3(0,1,0)*rotateSpeed*Time.deltaTime;
	}
	public virtual void MoveLeft()
	{
		myBody.velocity = new Vector3(0, myBody.velocity.y, 0);
		myTransform.localEulerAngles -= new Vector3(0,1,0)*rotateSpeed*Time.deltaTime;
	}
	public void MoveUp()
	{
		if(myTransform.InverseTransformDirection(myBody.velocity).y < maxVelocity)
				myBody.AddForce(new Vector3(0,jumpSpeed,0),ForceMode.VelocityChange);
	}
	public void Shoot()
	{
		if(Time.time - lastShoot > 0.2)
		{
			Instantiate (prefab, shootSpawn.position, shootSpawn.rotation);
			lastShoot = Time.time;
		}
	}

	protected override void OnCollisionEnter(Collision hit) 
	{
		Debug.Log (hit.gameObject.name);
		base.OnCollisionEnter (hit);

	}

	private void switchCameras(Camera[] cam, int active)
	{
		for (int i = 0; i<cam.Length; i++)
		{
			if(i!=active)
				cam[i].depth=0;
			else
				cam[i].depth=1;
		}

	}
	private void PlayerInterface(int windowID)
	{
		//GUILayout.Space (5);
		//GUILayout.BeginHorizontal ();
		//for (int i = 0; i<lifePoints; i++) {
						if ((double)lifePoints / (double)lifeMax > 0.75)
								GUI.color = Color.green;
						else {
								if ((double)lifePoints / (double)lifeMax < 0.75 && (double)lifePoints / (double)lifeMax > 0.5)
										GUI.color = Color.yellow;
								else {
										if ((double)lifePoints / (double)lifeMax < 0.5 && (double)lifePoints / (double)lifeMax > 0.25)
												GUI.color = new Color (1, 0.5f, 0);
										else {
												if ((double)lifePoints / (double)lifeMax < 0.25)
														GUI.color = Color.red;		
										}
								}
						}
						GUILayout.Box (lifePoints + "/" + lifeMax);
		//		}
	}

	void OnGUI()
	{
		windowRect = new Rect(windowLeft, windowTop, windowWidth, windowHeight);
		windowRect = GUI.Window(4, windowRect, PlayerInterface, "Player HP");

	}

}
