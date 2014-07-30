using UnityEngine;
using System.Collections;

public class UfoArea : MonoBehaviour {

	[SerializeField]
	Transform Tr1; // spawn point 1
	
	[SerializeField]
	Transform Tr2; // spawn point 2
	

	// Random position between two spawn points
	public Vector3 GetRandomStartPosition()
	{
		Vector3 segment = Tr2.position - Tr1.position;

		return Tr1.position + segment.normalized *  Random.Range(0, segment.magnitude); 
	}



}
