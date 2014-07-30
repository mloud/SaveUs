using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour 
{
	[SerializeField]
	public bool DestroyBarrierAfterDifferenUforHit; // Destroy ufo after hit of barrier of different color

	[SerializeField]
	public bool DisableCrossedBarriers; // Not allow to create barrier crossing other

	[SerializeField]
	public bool BarrierBySwipe; // If true, barrier is made swipe from point1 to point2


	public static Config Instance { get { return _instance; } }

	private static Config _instance;

	private void Awake()
	{
		_instance = this;
	}


}
