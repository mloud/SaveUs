using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour 
{
	[System.Serializable]
	public class ScoreVisualItem
	{
		public RectTransform PnlContainer;
		public Text TxtPosition;
		public Text TxtScore;
	}

	[SerializeField]
	List<ScoreVisualItem> visualItems;

	void Init () 
	{
		List<int> scoreList = new List<int>(Game.Instance.Options.ScoreList);
		scoreList.Sort();
		scoreList.Reverse();

		for (int i = 0; i < visualItems.Count; ++i)
		{
			if (i < scoreList.Count)
			{
				visualItems[i].TxtPosition.text = (i + 1).ToString();
				visualItems[i].TxtScore.text = scoreList[i].ToString();
			}
			else
			{
				visualItems[i].PnlContainer.gameObject.SetActive(false);
			}
		}
	}

	void OnEnable()
	{
		Init();
	}

	void Start()
	{
		Init();
	}

	void Update ()
	{
	
	}
}
