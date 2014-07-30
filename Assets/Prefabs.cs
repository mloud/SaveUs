using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prefabs : MonoBehaviour
{
	public static Prefabs Instance { get { return _instance; } }

	private static Prefabs _instance;

	[SerializeField]
	public GameObject BarrierPrefab;
	
	[SerializeField]
	public List<GameObject> FlyingUfoListPrefabs;

	[SerializeField]
	public GameObject UfoExplosionEffect;


	private void Start()
	{
		_instance = this;
	}

}
