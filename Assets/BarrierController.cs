using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrierController : MonoBehaviour {

	[SerializeField]
	BarrierCollider barrierCollider;

	[SerializeField]
	LineRenderer lineRenderer;

	[SerializeField]
	Transform mesh;

	public BarrierPoint Point1 { get; set; }
	public BarrierPoint Point2 { get; set; }
	public BarrierPoint.Type CurrentType { get { return Point1.CurrentType; }}

	public Vector3 EndPosition;

	void Start ()
	{}
	
	void Update () 
	{
		Vector3 pos1 = Point1.gameObject.transform.position;
		Vector3 pos2 = Point2 != null ? Point2.gameObject.transform.position : EndPosition;

		lineRenderer.SetPosition(0, pos1);

		lineRenderer.SetPosition(1, pos2);


		if (mesh != null)
		{
			Vector3 center = pos1 + (pos2 - pos1) * 0.5f;

			Vector3 scale = mesh.localScale;
			scale.y = (pos1 - pos2).magnitude;

			mesh.position = center;
			mesh.localScale = scale;

			mesh.rotation = Quaternion.LookRotation(pos2 - pos1);
			mesh.Rotate(new Vector3(0,0,90), Space.World);
		}
	}

	public void Init(BarrierPoint point1, BarrierPoint point2)
	{
		Point1 = point1;
		Point2 = point2;

		lineRenderer.SetPosition(0, point1.gameObject.transform.position);
		lineRenderer.SetPosition(1, point2.gameObject.transform.position);

		lineRenderer.SetColors(point1.PointColor, point2.PointColor);
	
		Update();
	}
	public void Init(BarrierPoint point1, Vector3 endPosition)
	{
		Point1 = point1;
		EndPosition = endPosition;
		
		lineRenderer.SetPosition(0, point1.gameObject.transform.position);
		lineRenderer.SetPosition(1, endPosition);
		
		lineRenderer.SetColors(point1.PointColor, point1.PointColor);
	
		Update();
	}

	public void OnUfoCollision(Ufo ufo)
	{
		Game.Instance.OnBarrierUfoCollision(this, ufo);
	}

	public void OnBarrierCollision(BarrierController barrier)
	{
		Game.Instance.OnTwoBarriersCollision(this, barrier);
	}

	public void Deactivate(bool useEffect)
	{
		StartCoroutine(DeactivateCoroutine(useEffect));
	}


	private IEnumerator DeactivateCoroutine(bool useEffect)
	{
		Destroy (barrierCollider.gameObject);

		if (useEffect)
		{
			int count = 5;

			for (int i = 0; i < count; ++i)
			{
				yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
			
				gameObject.SetActive(!gameObject.activeSelf);
			}
		}

		Destroy (gameObject);
	}


}
