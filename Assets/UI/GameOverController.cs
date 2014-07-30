using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{

	[SerializeField]
	Text txtScore;

	public void Init(int score)
	{
		txtScore.text = score.ToString();
	}
}
