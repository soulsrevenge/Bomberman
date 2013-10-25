using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {	
	
	[SerializeField]
    private bool isServer = true;
    public bool IsServer
    {
        get { return isServer; }
        set { isServer = value; }
    }

	void Start () {
        Application.runInBackground = true;

        if (IsServer) // si serveur, alors création de celui-ci
        {
            Network.InitializeSecurity();
            Network.InitializeServer(2, 1800, true);
        }
        else // sinon connection à celui-ci
        {
            Network.Connect("127.0.0.1", 1800);
        }
	}
	
}
