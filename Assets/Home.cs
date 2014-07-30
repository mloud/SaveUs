using UnityEngine;
using System.Collections;

public class Home : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnTriggerEnter(Collider other)
	{
		Ufo ufo = other.GetComponent<Ufo>();
		if (ufo)
		{
			Game.Instance.DamageHomeBy(ufo);
			ufo.Die();
		}
	}
}
