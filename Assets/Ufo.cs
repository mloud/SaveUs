using UnityEngine;
using System.Collections;

public class Ufo : MonoBehaviour 
{	
	[SerializeField]
	public GameObject ExplosionEffect;

	[SerializeField]
	public BarrierPoint.Type CurrentType;

	[SerializeField]
	Vector3 Speed;       // output speedpeed 

	[SerializeField]
	Vector3 SpeedVariant; // speed variant speed = speed -+ (rand(0, speedVariant) 0.5f

	[SerializeField]
	public float DamageHp; // how much it damage it deals after planet hit

	[SerializeField]
	public float DamageEnergy; // how much energy it takes after planet hit

	[SerializeField]
	public float DamageEnergyMax; // how much energy it takes from energy capacity

	public enum State
	{
		Flying,
		Dying
	}

	public State CurrentState { get; private set; }

	// Use this for initialization
	void Start () 
	{
		Speed += new Vector3((Random.Range(0,2) == 0? -1 : 1) * Random.Range(0, SpeedVariant.x),
		                     (Random.Range(0,2) == 0? -1 : 1) * Random.Range(0, SpeedVariant.y),
		                     (Random.Range(0,2) == 0? -1 : 1) * Random.Range(0, SpeedVariant.z));
	
		CurrentState = State.Flying;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += Speed * Time.deltaTime;
	}


	public void Die()
	{
		GameObject effect = Instantiate(ExplosionEffect) as GameObject;
		effect.transform.position = gameObject.transform.position;

		Speed = Vector3.zero;
		animation.Play();
		CurrentState = State.Dying;
	}

	public void OnAnimationEvent(string eventName)
	{
		if (eventName == "burnFinished")
		{
			Destroy (gameObject);
		}
	}
}
