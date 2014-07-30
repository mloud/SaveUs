using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionListener : MonoBehaviour
{

	public void OnButtonClick(Button button)
	{
		if (button.name == "BtnMenu")
		{
			Game.Instance.TogglePause();
			MUI.Instance.ToggleMainMenu();
		}
		else if (button.name == "BtnRestart")
		{
			Game.Instance.Restart();

			MUI.Instance.HideMainMenu();
		}
		else if (button.name == "BtnScore")
		{
			if (Game.Instance.Paused)
			{
				Game.Instance.TogglePause();
			}

			MUI.Instance.ToggleScore();
		}
		else if (button.name == "BtnResetScore")
		{
			Game.Instance.Options.ResetScore();
		}
		else if (button.name == "BtnCloseScore")
		{
			if (Game.Instance.Paused)
			{
				Game.Instance.TogglePause();
			}
			
			MUI.Instance.ToggleScore();
		}
		else if (button.name == "BtnCloseMenu")
		{
			Game.Instance.TogglePause();
			MUI.Instance.ToggleMainMenu();
		}
	}


}
