using UnityEngine;
using System.Collections;

public class SpotController : MonoBehaviour
{
	public BarrierPoint Point { get; private set; }


	void Start () 
	{
		Init ();
	}
	
	void Update ()
	{
	
	}


	public void OnClick()
	{
		if (Point)
		{
			Point.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
		}
	}


	void Attach(BarrierPoint point)
	{
		// insert under hierarchy
		point.gameObject.transform.parent = gameObject.transform;
		point.gameObject.transform.position = gameObject.transform.position;

		// set for faster access
		Point = point;
	}

	void Init()
	{
		if (gameObject.transform.childCount == 1)
		{
			Point = gameObject.transform.GetChild(0).GetComponent<BarrierPoint>();
		}
	}

	void OnMouseDown()
	{
		if (!Config.Instance.BarrierBySwipe)
		{
			if (Point)
			{
				Point.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
			}
		}
	}


}
