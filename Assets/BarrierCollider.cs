using UnityEngine;
using System.Collections;

public class BarrierCollider : MonoBehaviour {

	[SerializeField]
	public BarrierController barrierCtrl;

	void OnTriggerEnter(Collider other)
	{
		Ufo ufo = other.GetComponent<Ufo>();

		if (ufo)
		{
			barrierCtrl.OnUfoCollision(ufo);
		}
		else
		{
			BarrierCollider barrierCollider = other.GetComponent<BarrierCollider>();

			if (barrierCollider != null)
			{
				barrierCtrl.OnBarrierCollision(barrierCollider.barrierCtrl);
			}
		}


	}
}
