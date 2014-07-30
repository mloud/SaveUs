using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpController : MonoBehaviour {

	[SerializeField]
	Text txtHp;

	// Use this for initialization
	void Start () 
	{
		txtHp.text = Game.Instance.Hp.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		txtHp.text = Game.Instance.Hp.ToString();
	}
}
