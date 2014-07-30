using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour {

	[SerializeField]
	Scrollbar scrollBar;

	// Use this for initialization
	void Start () 
	{
		scrollBar.value = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		scrollBar.size = 1.0f;
		scrollBar.size = Game.Instance.Energy / Game.Instance.MaxEnergy;
	}

	public void MakeDecreaseEffect()
	{
		StartCoroutine(MakeDecreaseEffectCoroutine());
	}

	private IEnumerator MakeDecreaseEffectCoroutine()
	{
		ColorBlock origColor = scrollBar.colors;

		int count = 5;
		
		for (int i = 0; i < count; ++i)
		{
			yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
			
			gameObject.SetActive(!gameObject.activeSelf);
		}

		scrollBar.colors = origColor;
	}


}
