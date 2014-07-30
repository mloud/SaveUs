using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Options 
{
	public List<int> ScoreList { get; private set ;}

	public Options()
	{
		ScoreList = new List<int>();
	}

	public void AddScore(int score)
	{
		if (score > 0)
		{
			ScoreList.Add(score);

			Save();
		}
	}

	public void ResetScore()
	{
		ScoreList.Clear();
		Save();
	}

	private void SaveScore()
	{
		System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

		for (int i = 0; i < ScoreList.Count; ++i)
		{
			strBuilder.Append(ScoreList[i]);
			strBuilder.Append(";");
		}

		PlayerPrefs.SetString("Score", strBuilder.ToString());
	}


	private void LoadScore()
	{
		if (PlayerPrefs.HasKey("Score"))
		{
			string strScoreList = PlayerPrefs.GetString("Score");

			int delimitIndex = strScoreList.IndexOf(";");
			int startIndex = 0;
			while (delimitIndex != -1)
			{
				string score = strScoreList.Substring(startIndex, delimitIndex - startIndex);

				int numScore = -1;
				int.TryParse(score, out numScore);
				ScoreList.Add(numScore);
			
				startIndex = delimitIndex + 1;
						
				delimitIndex = strScoreList.IndexOf(";", startIndex);
			}


		}
	}

	public void Load()
	{
		LoadScore();
	}

	public void Save()
	{
		SaveScore();
	}
}
