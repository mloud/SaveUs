using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Game : MonoBehaviour, MTouch.ITouchListener
{

	[SerializeField]
	UfoArea ufoArea;

	[SerializeField]
	float UfoDelay;

	[SerializeField]
	float UfoDelayInterval;

	[SerializeField]
	public float MaxEnergy;

	[SerializeField]
	public float StartEnergy;

	[SerializeField]
	public float EnergyAddonPerSec;

	[SerializeField]
	public float StartHp;

	[SerializeField]
	public float CrossPenalty;


	public static Game Instance { get { return _instance; }}

	public bool Paused { get; private set; }

	public Options Options { get; private set; }

	public float Energy { get; set; }

	public float Hp { get; set; }

	public int Score { get; set; }

	private List<SpotController> SpotList { get; set; }

	private static Game _instance;

	private List<BarrierPoint> ActivatedPoints { get; set; }

	private List<BarrierController> Barriers { get; set; }

	private float UfoTimer { get; set; }

	private float UfoDelayChangeInterval { get; set; }
	private float UfoDelayChangeTimer { get; set; }

	private bool GameOver { get; set; }


	private BarrierPoint TouchedBarrierPoint { get; set; }
	private BarrierController TmpBarrier {get; set;}


	void Awake()
	{
		_instance = this;
	}



	void OnDestroy()
	{
		MTouch.TouchManager.Instance.Unregister(this);
	}


	void Start () 
	{
		MTouch.TouchManager.Instance.Register(this);

		Options = new Options();
		Options.Load();

		SpotList = new List<SpotController>(GameObject.FindObjectsOfType<SpotController>());
		ActivatedPoints = new List<BarrierPoint>();
		Barriers = new List<BarrierController>();
	
		UfoTimer = Time.time + UfoDelay;
	
		UfoDelayChangeTimer = Time.time + UfoDelayChangeInterval;

		Restart();
	}


	private void UpdateEnergy()
	{

		if (ActivatedPoints.Count == 0)
		{
			Energy += EnergyAddonPerSec * Time.deltaTime;
		}

		for (int i = 0; i < ActivatedPoints.Count; ++i)
		{
			Energy -= ActivatedPoints[i].EnergyCostPerSec * Time.deltaTime;
		}

		if (Energy > MaxEnergy)
		{
			Energy = MaxEnergy;
		}
		else if (Energy < 0)
		{
			Energy = 0;
		
			//deactivate all
			for (int i = 0; i < ActivatedPoints.Count; ++i)
			{
				ActivatedPoints[i].ToggleActive(BarrierPoint.ToggleReason.BySystem);
			}
		}


	}

	void Update()
	{
		if (!GameOver)
		{
			// update energy
			UpdateEnergy();

			if (Time.time > UfoTimer)
			{
				GenerateUfo();


				float delay = Random.Range(0, UfoDelayInterval);
				delay -= delay * 0.5f;
				delay += UfoDelay;

				UfoTimer = Time.time + delay;

				//UfoDelay -= 0.2f;

				if (Time.time > UfoDelayChangeTimer)
				{
					UfoDelay -= 0.03f;
					UfoDelayChangeTimer = Time.time + UfoDelayChangeInterval;
				}

				if (UfoDelay < 1.5f)
				{
					UfoDelay = 1.5f;
				}
			}
		}
	}

	private void GenerateUfo()
	{
		GameObject prefab = Prefabs.Instance.FlyingUfoListPrefabs[Random.Range(0, Prefabs.Instance.FlyingUfoListPrefabs.Count)];

		GameObject ufoInstance = Instantiate(prefab) as GameObject;

		ufoInstance.transform.position = ufoArea.GetRandomStartPosition();

	}

	private void ShakeCamera()
	{
		Shaker[] shakers = GameObject.FindObjectsOfType<Shaker>();
		
		for (int i = 0; i < shakers.Length; ++i)
		{
			shakers[i].Shake(0.1f, 0.1f);
		}
	}

	public void DamageHomeBy(Ufo ufo)
	{
		Hp -= ufo.DamageHp;

		Energy -= ufo.DamageEnergy;

		MaxEnergy -= ufo.DamageEnergyMax;

		ShakeCamera();

		if (MaxEnergy < 0)
		

		if (MaxEnergy <= 0 || Energy <= 0)
		{
			Energy = 0;
			EndGame();
		
		}

//		if (Hp <= 0)
//		{
//			Hp = 0;
//
//			EndGame();
//		}
	}

	private void EndGame()
	{
		Options.AddScore(Score);
	
		GameOver = true;

		MUI.Instance.ShowGameOver(Score);
	}

	public void Restart()
	{
		MUI.Instance.CloseAll();

		TouchedBarrierPoint = null;

		if (TmpBarrier != null)
		{
			Destroy(TmpBarrier.gameObject);
			TmpBarrier = null;
		}



		BarrierController[] barriers = FindObjectsOfType<BarrierController>();
		for (int i = 0; i < barriers.Length; ++i)
			Destroy(barriers[i].gameObject);
		Barriers.Clear();

		Ufo[] ufos = FindObjectsOfType<Ufo>();
		for (int i = 0; i < ufos.Length; ++i)
			Destroy(ufos[i].gameObject);
	
		BarrierPoint[] barrierPoints = FindObjectsOfType<BarrierPoint>();
		for (int i = 0; i < barrierPoints.Length; ++i)
		{
			if (barrierPoints[i].Activated)
			{
				barrierPoints[i].ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
			}
		}
		ActivatedPoints.Clear();

		Energy = StartEnergy;
		Hp = StartHp;
		Score = 0;



		if (Paused)
			TogglePause();


		GameOver = false;


	}

	public void AddScore(int score)
	{
		if (!GameOver)
		{
			Score += score;
		}
	}

	public void TogglePause()
	{
		Paused = !Paused;

		if (Paused)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1.0f;
		}
	}


	private List<BarrierController> FindBarriersWithPoint(BarrierPoint point)
	{
		List<BarrierController> barriers = new List<BarrierController>();

		for (int i = 0; i < Barriers.Count; ++i)
		{
			if (Barriers[i].Point1 == point || Barriers[i].Point2 == point)
			{
				barriers.Add(Barriers[i]);
			}
		}

		return barriers;
	}

	private BarrierController CreateTmpBarrier(BarrierPoint p1)
	{
		GameObject barrierGo = Instantiate(Prefabs.Instance.BarrierPrefab) as GameObject;
		BarrierController barrierCtrl = barrierGo.GetComponent<BarrierController>();
		barrierCtrl.Init (p1, p1.transform.position);

		return barrierCtrl;
	}


	private void CreateBarrier(BarrierPoint p1, BarrierPoint p2)
	{
		GameObject barrierGo = Instantiate(Prefabs.Instance.BarrierPrefab) as GameObject;
		BarrierController barrierCtrl = barrierGo.GetComponent<BarrierController>();
		barrierCtrl.Init (p1, p2);
		Barriers.Add(barrierCtrl);

		Vector2 pos1 = p1.transform.position; // vec3 -> vec2
		Vector2 pos2 = p2.transform.position;


		for (int i = 0; i <  Barriers.Count; ++i)
		{
			Vector2 pos3 = new Vector2(Barriers[i].Point1.transform.position.x, Barriers[i].Point1.transform.position.y);
			Vector2 pos4 = new Vector2(Barriers[i].Point2.transform.position.x, Barriers[i].Point2.transform.position.y);

			Vector2 inter = new Vector2();
			if (Utils.LineIntersection(pos1, pos2, pos3, pos4, ref inter))
			{
				StartCoroutine(OnTwoBarriersCollision(barrierCtrl, Barriers[i]));
				break;
			}
		}
	}




	public void OnBarrierPointControllerMouseDown(BarrierPoint point)
	{
		// already activated
		if (point.Activated)
		{
			List<BarrierController> barriers = FindBarriersWithPoint(point);

			for (int i = 0; i < barriers.Count; ++i)
			{
				if (barriers[i].Point1 != point)
					barriers[i].Point1.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);

				if (barriers[i].Point2 != point)
					barriers[i].Point2.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
			}

			point.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
		}
		else
		{
			TouchedBarrierPoint = point;

			TmpBarrier = CreateTmpBarrier(point);

		}
	}

	public void OnBarrierPointControllerMouseUp(BarrierPoint point)
	{
		Debug.Log("OnBarrierPointControllerMouseUp: " + point.transform.position);
		if (TouchedBarrierPoint != null)
		{
			if (point != TouchedBarrierPoint && point.CurrentType == TouchedBarrierPoint.CurrentType)
			{
				point.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
				TouchedBarrierPoint.ToggleActive(BarrierPoint.ToggleReason.ByPlayer);
			}
			TouchedBarrierPoint = null;
		}

	}



	public void OnBarrierUfoCollision(BarrierController barrier, Ufo ufo)
	{
		if (ufo.CurrentState == Ufo.State.Flying)
		{
			// same color collision
			if (ufo.CurrentType == barrier.CurrentType)
			{
				Game.Instance.AddScore(1);
			
				ufo.Die();
		
				Destroy (ufo.gameObject, 1.0f);
			}
			// different color collision
			else
			{
				if (Config.Instance.DestroyBarrierAfterDifferenUforHit)
				{
					barrier.Point1.ToggleActive(BarrierPoint.ToggleReason.BySystem);
					barrier.Point2.ToggleActive(BarrierPoint.ToggleReason.BySystem);
				}
			}
		}
	}


	public IEnumerator OnTwoBarriersCollision(BarrierController barrier1, BarrierController barrier2)
	{
		ShakeCamera();

		yield return new WaitForSeconds(0.1f);

		barrier1.Point1.ToggleActive(BarrierPoint.ToggleReason.BySystem);
		barrier1.Point2.ToggleActive(BarrierPoint.ToggleReason.BySystem);

		barrier2.Point1.ToggleActive(BarrierPoint.ToggleReason.BySystem);
		barrier2.Point2.ToggleActive(BarrierPoint.ToggleReason.BySystem);

		Energy -= CrossPenalty;
	}

	public void OnBarrierPointStateChanged(BarrierPoint point, BarrierPoint.ToggleReason reason)
	{
		if (point.Activated)
		{
			// CHECK FOR ENOUGH ENERGY
			if (Energy >= point.UsageEnergyCost)
			{

				ActivatedPoints.Add(point);
			
				
				Game.Instance.Energy -= point.UsageEnergyCost;


				for (int i = 0; i < ActivatedPoints.Count - 1; ++i)
				{
					BarrierPoint point1 = ActivatedPoints[i];

					if (point1.CurrentType == point.CurrentType)
					{
						CreateBarrier(point1, point);
					}
				}
			}
		
		}
		else
		{
			// check for existing barrier
			BarrierController barriedCtrl = Barriers.Find(x=>x.Point1 == point || x.Point2 == point);

			if (barriedCtrl != null)
			{
				Barriers.Remove(barriedCtrl);

				barriedCtrl.Deactivate(reason == BarrierPoint.ToggleReason.BySystem);
			}

			Game.Instance.Energy += point.UsageEnergyCost;

			ActivatedPoints.Remove(point);
		}

	}


	///////////////    TOUCHES   //////////////////
	public void TouchBegan(MTouch.Touch touch)
	{
		List<SpotController> spots = MTouch.TouchManager.Instance.GetGameObjectsAt<SpotController>(touch.Position);

		if (spots.Count > 0)
		{
			OnBarrierPointControllerMouseDown(spots[0].Point);
		}
	}

	public void TouchMoved(MTouch.Touch touch)
	{
		if (TmpBarrier)
		{
			TmpBarrier.EndPosition = Camera.main.ScreenToWorldPoint(touch.Position);;
		}
	}

	public void TouchEnded(MTouch.Touch touch)
	{
		List<SpotController> spots = MTouch.TouchManager.Instance.GetGameObjectsAt<SpotController>(touch.Position);
		
		if (spots.Count > 0)
		{
			OnBarrierPointControllerMouseUp(spots[0].Point);
		}

		if (TmpBarrier != null)
		{
			Destroy(TmpBarrier.gameObject);
			TmpBarrier = null;
		}
	}

}
