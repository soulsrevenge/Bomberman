using UnityEngine;
using System.Collections;

public class SpeederScript : MonoBehaviour
{
	
	void OnCollisionStay(Collision hit)
	{
		if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			GameObject player = hit.transform.gameObject;
			Character c = player.GetComponent<PlayerScript> () as PlayerScript;
			if (c != null)
			{
				Rigidbody b = c.MyBody;
				Transform t = c.MyTransform;
				Vector3 direction = transform.InverseTransformDirection(new Vector3(0,0,transform.parent.transform.position.z));
				b.velocity -= new Vector3(0,b.velocity.y,0);
				t.Translate(Vector3.forward,Space.Self);
				if(b.velocity.x < 8 && b.velocity.z < 8)
					b.AddRelativeForce(direction.normalized*2,ForceMode.Impulse);
			}
		}
	}
}
