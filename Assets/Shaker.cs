using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour 
{
	private Vector3 _originalPosition;

	private float FinishTime { get; set; } 
	private float Amount { get; set; }


	private void Start()
	{
		FinishTime = -1;
	}


	public void Shake(float time, float amount)
	{
		FinishTime = Time.time + time;
		Amount = amount;

		_originalPosition = gameObject.transform.position;
	}


	void Update () 
	{
		if (FinishTime > 0)
		{
			if (Time.time > FinishTime)
			{
				FinishTime = -1;

				gameObject.transform.position = _originalPosition;
			}
			else
			{
				gameObject.transform.position = _originalPosition + Random.insideUnitSphere * Amount;
			}
		}
	}
}
