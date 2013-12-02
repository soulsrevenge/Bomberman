using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManagerScript : MonoBehaviour {

	
	[SerializeField]
	private GameObject player1;

	public GameObject Player1 
	{
		get {return this.player1;}
		set {player1 = value;}
	}
	[SerializeField]
	private GameObject player2;

	public GameObject Player2 
	{
		get {return this.player2;}
		set {player2 = value;}
	}
	
	private PlayerScript script1, script2;

    class PlayerIntents //actions possibles
    {
        public bool wantToMoveUp = false;
        public bool wantToMoveFront = false;
		public bool wantToMoveRight = false;
		public bool wantToMoveLeft = false;		
		public bool wantToMoveBack = false;
		public bool wantToShoot = false;
    }

    private Dictionary<NetworkPlayer, PlayerIntents> playersIntents;
    private Dictionary<NetworkPlayer, PlayerIntents> PlayersIntents
    {
        get { return playersIntents; }
        set { playersIntents = value; }
    }

    private NetworkView myNetworkView = null;

	void Start () {
        PlayersIntents = new Dictionary<NetworkPlayer, PlayerIntents>();
        myNetworkView = this.gameObject.GetComponent<NetworkView>();
		script1 = player1.GetComponent("CharacterScript") as PlayerScript; //gestion mouvement joueur 1
		script2 = player2.GetComponent("CharacterScript") as PlayerScript; //gestion mouvement joueur 2
	
	}

    void OnPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new PlayerIntents()); // ajout du joueur sur le serveur
        myNetworkView.RPC("NewPlayerConnected", RPCMode.Server, p);// demande a l'ajout sur le jeu de tout les autres
    }
	

    [RPC]
    void NewPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new PlayerIntents()); // ajout d'un nouveau joueur
    }
	
	void Update () {
		if(Network.isClient){ // si le joueur est le client
			
            if (Input.GetKeyDown(KeyCode.Z))
                myNetworkView.RPC("PlayerWantToMoveFront", RPCMode.Server, Network.player, true); //demande a bouger vers l'avant
				
            if (Input.GetKeyUp(KeyCode.Z))
                myNetworkView.RPC("PlayerWantToMoveFront", RPCMode.Server, Network.player, false);
				
			if (Input.GetKeyDown(KeyCode.Q))
                myNetworkView.RPC("PlayerWantToMoveLeft", RPCMode.Server, Network.player, true);//demande a bouger vers la gauche
				
			if (Input.GetKeyDown(KeyCode.S))
                myNetworkView.RPC("PlayerWantToMoveBack", RPCMode.Server, Network.player, true);//demande a bouger vers l'arriere
				
            if (Input.GetKeyUp(KeyCode.S))
                myNetworkView.RPC("PlayerWantToMoveBack", RPCMode.Server, Network.player, false);
				
			if (Input.GetKeyDown(KeyCode.D))
                myNetworkView.RPC("PlayerWantToMoveRight", RPCMode.Server, Network.player, true);//demande a bouger vers la droite
				
            if (Input.GetKeyDown(KeyCode.Space))
                myNetworkView.RPC("PlayerWantToMoveUp", RPCMode.Server, Network.player, true); //demande a bouger vers le haut
			
            if (Input.GetKeyUp(KeyCode.Space))
                myNetworkView.RPC("PlayerWantToMoveUp", RPCMode.Server, Network.player, false);
            
			if(Input.GetKeyDown(KeyCode.LeftShift))
				myNetworkView.RPC ("PlayerWantToShoot",RPCMode.Server,Network.player,true); //demande de tir
				
			if(Input.GetKeyUp(KeyCode.LeftShift))
				myNetworkView.RPC ("PlayerWantToShoot",RPCMode.Server,Network.player,false);
		} else // sinon c'est le serveur
		{	   // déplace directement son joueur
			if (Input.GetKey(KeyCode.Z))
				script1.MoveFront(); // avant
			
			if (Input.GetKeyDown(KeyCode.Q))
			//	script1.MoveLeft(); // gauche
			
			if (Input.GetKey(KeyCode.S))
				script1.MoveBack(); // arriere
			
			if (Input.GetKeyDown(KeyCode.D))
			//	script1.MoveRight(); // droite
			
            if (Input.GetKey(KeyCode.Space))
				script1.MoveUp(); // haut
            
			if(Input.GetKeyDown(KeyCode.LeftShift))
				script1.Shoot(); // tir
		}
			
	}

    void FixedUpdate() //déplacement du client
    {
        int i = 0;
        foreach (var p in PlayersIntents)
        {			
            if (i == 0 && p.Value.wantToMoveLeft)
			{
				//script2.MoveLeft();
				p.Value.wantToMoveLeft = false;
			}

			if (i == 0 && p.Value.wantToMoveRight)
			{	
				//script2.MoveRight();
				p.Value.wantToMoveRight = false;
			}
			
            if (i == 0 && p.Value.wantToMoveBack)
               script2.MoveBack();
			 
            if (i == 0 && p.Value.wantToMoveUp)
                script2.MoveUp();
			
            if (i == 0 && p.Value.wantToMoveFront)
                script2.MoveFront();
			
			if (i == 0 && p.Value.wantToShoot)
				script2.Shoot();
			
            i++;
        }
    }

    [RPC] // Fonction lié aux demandes du client
    void PlayerWantToMoveUp(NetworkPlayer p, bool b)
    {
        PlayersIntents[p].wantToMoveUp = b;
        if (Network.isServer)
            myNetworkView.RPC("PlayerWantToMoveUp", RPCMode.Others, p, b);
    }

    [RPC]
    void PlayerWantToMoveRight(NetworkPlayer p, bool b)
    {
        PlayersIntents[p].wantToMoveRight = b;
        if (Network.isServer)
            myNetworkView.RPC("PlayerWantToMoveRight", RPCMode.Others, p, b);
        
    }
	[RPC]
    void PlayerWantToMoveLeft(NetworkPlayer p, bool b)
    {
        PlayersIntents[p].wantToMoveLeft = b;
        if (Network.isServer)
            myNetworkView.RPC("PlayerWantToMoveLeft", RPCMode.Others, p, b);
    }
	[RPC]
    void PlayerWantToMoveBack(NetworkPlayer p, bool b)
    {
        PlayersIntents[p].wantToMoveBack = b;
        if (Network.isServer)
            myNetworkView.RPC("PlayerWantToMoveBack", RPCMode.Others, p, b);
    }
	[RPC]
    void PlayerWantToMoveFront(NetworkPlayer p, bool b)
    {
        PlayersIntents[p].wantToMoveFront = b;
        if (Network.isServer)
            myNetworkView.RPC("PlayerWantToMoveFront", RPCMode.Others, p, b);
    }
	[RPC]
    void PlayerWantToShoot(NetworkPlayer p, bool b)
    {
        PlayersIntents[p].wantToShoot = b;
        if (Network.isServer)
            myNetworkView.RPC("PlayerWantToShoot", RPCMode.Others, p, b);
    }
	
}
