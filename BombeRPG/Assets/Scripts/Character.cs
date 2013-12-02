using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
	[SerializeField]
	protected int lifePoints;
	protected int lifeMax;
	public int LifePoints 
	{
		get { return lifePoints;  }
		set { lifePoints = value; }
	}

	[SerializeField]
	protected int damage;
	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}

	[SerializeField]
	protected float moveSpeed;
	[SerializeField]
	protected float maxVelocity;

	protected Transform myTransform;
	public Transform MyTransform 
	{
		get { return myTransform;  }
		set { myTransform = value; }
	}

	protected Rigidbody myBody;
	public Rigidbody MyBody
	{
		get { return myBody;	}
		set { myBody = value;   }
	}

	protected virtual void Start()
	{
		myTransform = this.transform;
		myBody = this.rigidbody;
	}

	protected virtual void Start(int life,int damage,float speed, float max)
	{
		myTransform = this.transform;
		myBody = this.rigidbody;
		lifePoints = life;
		lifeMax = life;
		this.damage = damage;
		moveSpeed = speed;
		maxVelocity = max;
	}

	public virtual void Hurt(int damage)
	{
		if(lifePoints>=damage)
			lifePoints -= damage;
		else
			lifePoints=0;
		if(lifePoints==0)
			Die ();
	}

	protected virtual void Die()
	{
		GameObject.Destroy (this.transform.gameObject);
	}

	protected virtual void OnCollisionEnter(Collision hit) 
	{
		if (hit.gameObject.layer.Equals (LayerMask.NameToLayer ("Explosion")))
		{
			BombScript b = hit.gameObject.transform.parent.GetComponent<BombScript>() as BombScript;
			if(b.StartExplosion)
				Die();
		}
	}
}

