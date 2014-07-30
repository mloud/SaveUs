using UnityEngine;
using System.Collections;

public class MUI : MonoBehaviour
{
	[SerializeField]
	Canvas canvas;

	[SerializeField]
	GameObject pnlMenu;

	[SerializeField]
	GameObject pnlScore;
	
	[SerializeField]
	GameObject pnlGameOver;
	

	public static MUI Instance { get {return _instance;} }

	private static MUI _instance;

	void Awake()
	{
		_instance = this;
	}

	public void ToggleMainMenu()
	{
		pnlMenu.SetActive(!pnlMenu.activeSelf);
	}

	public void HideMainMenu()
	{
		pnlMenu.SetActive(false);
	}


	public void ToggleScore()
	{
		pnlScore.SetActive(!pnlScore.activeSelf);
	}

	public void ShowGameOver(int score)
	{
		pnlGameOver.SetActive(true);
		pnlGameOver.GetComponent<GameOverController>().Init(score);
	}

	public void CloseAll()
	{
		pnlMenu.SetActive(false);
		pnlScore.SetActive(false);
		pnlGameOver.SetActive(false);

	}

}
