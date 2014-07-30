using UnityEngine;
using System.Collections;

public class BarrierPoint : MonoBehaviour 
{

	public enum ToggleReason
	{
		ByPlayer,
		BySystem
	}

	[SerializeField]
	public Color PointColor;

	[SerializeField]
	public float UsageEnergyCost;

	[SerializeField]
	public float EnergyCostPerSec;


	[SerializeField]
	private float maxHaloSize;

	public enum Type
	{
		Red = 0,
		Yellow = 1,
		Blue = 2
	}

	public bool Activated { get; set; }

	[SerializeField]
	public Type CurrentType;

	private Behaviour effHalo;

	private float DstHalo { get; set; }


	// Use this for initialization
	void Start ()
	{
		effHalo = (Behaviour)GetComponent("Halo");

		Deactivate();
	}
	

	public void ToggleActive(ToggleReason reason)
	{
		if (Activated)
		{
			Deactivate();
			Game.Instance.OnBarrierPointStateChanged(this, reason);
		}
		else
		{
			if (Game.Instance.Energy >= UsageEnergyCost)
			{
				Activate();
				Game.Instance.OnBarrierPointStateChanged(this, reason);
			}
		}
	}


	private void Activate()
	{
		Activated = true;
		effHalo.enabled = Activated;
	}

	private void Deactivate()
	{
		Activated = false;
		effHalo.enabled = Activated;


	}

}
