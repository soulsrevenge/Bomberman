using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the MultiplayerManager and it 
/// is the foundation for our multiplayer system.
/// </summary>

public class MultiplayerScript : MonoBehaviour {
	
	
	[SerializeField]
    private GameObject spawnerZombie;
    public GameObject SpawnerZombie
    {
        get { return spawnerZombie; }
        set { spawnerZombie = value; }
    }
	
	[SerializeField]
	private GameObject player;

	public GameObject Player {
		get {
			return this.player;
		}
		set {
			player = value;
		}
	}	
	//Variables Start___________________________________
	
	private string titleMessage = "BombeRPG Man";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 26300;
	private bool useNAT = false; //se renseigner sur les NAT...
	private string ipAddress;
	private string port;
	private int numberOfPlayers = 2;
	public string playerName;
	public string serverName;
	public string serverNameForClient;
	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;
	private bool iWantToPlaySolo = false;
	private bool playingSolo = false;
	public Vector3 respawnPosition;
	public GameObject computer;
	
	
	
	//These variables are used to define the main
	//window.
	
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndent;
	private int topIndent;
	
	//These variables are used to define the server
	//shutdown window.	
	private Rect serverDisWindowRect;
	private int serverDisWindowWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent = 10;

	//These variables are used to define the client
	//disconnect window.
	private Rect clientDisWindowRect;
	private int clientDisWindowWidth = 300;
	private int clientDisWIndowHeight = 170;
	private bool showDisconnectWindow = false;
	
	//Variables End_____________________________________
	
	
	// Use this for initialization
	void Start () 
	{	
		//Load the last used serverName from registry.
		//If the serverName is blank then use "Server"
		//as a default name.
		Application.runInBackground = true;
		Time.timeScale = 0;
		serverName = PlayerPrefs.GetString("serverName");
		
		if(serverName == "")
		{
			serverName = "Serveur";	
		}
		
		
		//Load the last used playerName from registry.
		//If the playerName is blank then use "Player"
		//as a default name.
		
		playerName = PlayerPrefs.GetString("playerName");
		
		if(playerName == "")
		{
			playerName = "Joueur";	
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			showDisconnectWindow = !showDisconnectWindow;	
		}
	}
	
	
	void ConnectWindow(int windowID)
	{
		//Leave a gap from the header.
		GUILayout.Space(15);
		//When the player launches the game they have the option to play alone,
		//to create a server or join a server. The variables iWantToPlaySolo
		//iWantToSetupAServer and iWantToConnectToAServer start as
		//false so the player is presented with 3 buttons
		//"Jouer Solo", "Creer un serveur" and "Se Connecter a un serveur". 
		
		if(iWantToSetupAServer == false && iWantToConnectToAServer == false && iWantToPlaySolo == false)
		{
			if(GUILayout.Button("Jouer Solo", GUILayout.Height(buttonHeight)))
			{
				iWantToPlaySolo = true;	
			}
			GUILayout.Space(10);
			if(GUILayout.Button("Creer un serveur", GUILayout.Height(buttonHeight)))
			{
				iWantToSetupAServer = true;	
			}
			GUILayout.Space(10);
			if(GUILayout.Button("Se Connecter a un serveur", GUILayout.Height(buttonHeight)))
			{
				iWantToConnectToAServer = true;
			}			
			GUILayout.Space(10);
			if(Application.isWebPlayer == false && Application.isEditor == false)
			{
				if(GUILayout.Button("Quitter BombeRPG Man", GUILayout.Height(buttonHeight)))
				{
					Application.Quit();	
				}
			}
		}
		
		
		if(iWantToSetupAServer == true)
		{
			//The user can type a name for their server into
			//the textfield.
			
			GUILayout.Label("Entrer un nom de Serveur");
			serverName = GUILayout.TextField(serverName);
			GUILayout.Space(5);
			
			
			//The user can type in the Port number for their server
			//into textfield. We defined a default value above in the 
			//variables as 26300.
			GUILayout.Label("Port du Serveur");			
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			GUILayout.Space(10);
			if(GUILayout.Button("Demarrer le Serveur", GUILayout.Height(30)))
			{
				//Create the server
				Network.InitializeSecurity();
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);				
				//Save the serverName using PlayerPrefs.				
				PlayerPrefs.SetString("serverName", serverName);				
				iWantToSetupAServer = false;
			}
			
			if(GUILayout.Button("Retour", GUILayout.Height(30)))
			{
				iWantToSetupAServer = false;	
			}
		}
		
		
		if(iWantToConnectToAServer == true)
		{
			//The user can type their player name into the
			//textfield.
			GUILayout.Label("Entrer votre nom de Joueur");
			playerName = GUILayout.TextField(playerName);			
			GUILayout.Space(5);			
			//The player can type the IP address for the server
			//that they want to connect to into the textfield.
			GUILayout.Label("Entrer l'IP du Serveur");
			connectToIP = GUILayout.TextField(connectToIP);
			GUILayout.Space(5);			
			//The player can type in the Port number for the server
			//they want to connect to into the textfield.			
			GUILayout.Label("Port du Serveur");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));			
			GUILayout.Space(5);
			//The player clicks on this button to establish a connection.
			
			if(GUILayout.Button("Connection", GUILayout.Height(25)))
			{
				//Ensure that the player can't join a game with an empty name
				if(playerName == "")
					playerName = "Joueur";	
				//If the player has a name that isn't empty then attempt to join 
				//the server.
				
				if(playerName != "")
				{
					//Connect to a server with the IP address contained in
					//connectToIP and with the port number contained
					//in connectionPort.
					
					Network.Connect(connectToIP, connectionPort);
					PlayerPrefs.SetString("playerName", playerName);
				}
			}
			
			GUILayout.Space(5);
			
			if(GUILayout.Button("Retour", GUILayout.Height(25)))
			{
				iWantToConnectToAServer = false;
			}
			
		}

		if (iWantToPlaySolo == true) 
		{
			Instantiate(computer,respawnPosition,Quaternion.identity);
			Time.timeScale = 1;
			iWantToPlaySolo = false;
			playingSolo = true;
		}
		
	}
	
	
	void ServerDisconnectWindow(int windowID)
	{
		GUILayout.Label("Nom du Serveur : " + serverName);	
		//Show the number of players connected.		
		GUILayout.Label("Nombre de Joueurs connectes : " + Network.connections.Length);		
		//If there is at least one connection then show the average ping.		
		if(Network.connections.Length >= 1)
		{
			GUILayout.Label("Ping : " + Network.GetAveragePing(Network.connections[0]));	
		}
		//Shutdown the server if the user clicks on the Shutdown server button.
		if(GUILayout.Button("Arreter le Serveur"))
		{
			Network.Disconnect();	
		}
	}
	
	
	
	void ClientDisconnectWindow(int windowID)
	{
		//Show the player the server they are connected to and the
		//average ping of their connection.
		
		GUILayout.Label("Connecte au Server : " + serverName);
		GUILayout.Label("Ping : " + Network.GetAveragePing(Network.connections[0]));
		GUILayout.Space(7);

		//The player disconnects from the server when they press the 
		//Disconnect button.
		
		if(GUILayout.Button("Deconnection", GUILayout.Height(25)))
		{
			Network.Disconnect();	
		}
		GUILayout.Space(5);
		//This button allows the player using a webplayer who has can gone 
		//fullscreen to be able to return to the game. Pressing escape in
		//fullscreen doesn't help as that just exits fullscreen.
		
		if(GUILayout.Button("Retour au Jeu", GUILayout.Height(25)))
		{
			showDisconnectWindow = false;	
		}
	}
	
	
	void OnDisconnectedFromServer()
	{
		//If a player loses the connection or leaves the scene then
		//the level is restarted on their computer.
		
		Application.LoadLevel(Application.loadedLevel);
	}
	
	
	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		//When the player leaves the server delete them across the network
		//along with their RPCs so that other players no longer see them.
		
		Network.RemoveRPCs(networkPlayer);
			
		Network.DestroyPlayerObjects(networkPlayer);	
	}
	
	
	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		networkView.RPC("TellPlayerServerName", networkPlayer, serverName);	
		
		if (Network.connections.Length == 2)
        {
            /*Network.Instantiate(SpawnerZombie,new Vector3(10,3,10),
                Quaternion.identity, 0);
			
			Network.Instantiate(SpawnerZombie,new Vector3(10,18,10),
                Quaternion.identity, 0);*/
			Network.Instantiate(Player,respawnPosition,Quaternion.identity,1);
			Time.timeScale = 1;
        }
	}
	 void OnConnectedToServer()
	{
		/*if(Network.connections.Length == 1)
			Network.Instantiate(Player, new Vector3(5,2,1),
				Quaternion.identity, 0);
		if(Network.connections.Length == 2)
			Network.Instantiate(Player, new Vector3(10,18,15),
				Quaternion.identity, 0);*/
	}
	
	
	void OnGUI()
	{
		//If the player is disconnected then run the ConnectWindow function.
		
		if(Network.peerType == NetworkPeerType.Disconnected && playingSolo == false)
		{
			//Determine the position of the window based on the width and 
			//height of the screen. The window will be placed in the middle
			//of the screen.
			
			leftIndent = Screen.width / 2 - connectionWindowWidth / 2;
			
			topIndent = Screen.height / 2 - connectionWindowHeight / 2;
			
			connectionWindowRect = new Rect(leftIndent, topIndent, connectionWindowWidth,
			                                connectionWindowHeight);
			
			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow,
			                                        titleMessage);
		}
		
		
		//If the game is running as a server then run the ServerDisconnectWindow
		//function.
		
		if(Network.peerType == NetworkPeerType.Server)
		{
			//Defining the Rect for the server's disconnect window.
			
			serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent,
			                               serverDisWindowWidth, serverDisWindowHeight);
			
			serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
		}
		
		
		//If the connection type is a client (a player) then show a window that allows
		//them to disconnect from the server.
		
		if(Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true)
		{
			clientDisWindowRect = new Rect(Screen.width / 2 - clientDisWindowWidth / 2,
			                               Screen.height / 2 - clientDisWIndowHeight / 2,
			                               clientDisWindowWidth, clientDisWIndowHeight);
			
			clientDisWindowRect = GUILayout.Window(1, clientDisWindowRect, ClientDisconnectWindow, "");
		}
			
	}
	
	
	//Used to tell the MultiplayerScript in connected players the serverName. Otherwise
	//players connecting wouldn't be able to see the name of the server.
	
	[RPC]
	void TellPlayerServerName (string servername)
	{
		serverName = servername;	
	}
	
	
	
	
	
	
	
	
	
}
