using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreHudController : MonoBehaviour
{

	[SerializeField]
	Text txtScore;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		txtScore.text = Game.Instance.Score.ToString();
	}
}
