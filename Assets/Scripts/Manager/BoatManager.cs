using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    //		Instance
	public static BoatManager Instance { get; private set; }

	//		Unity Assigned Script
	[SerializeField] private PositionScript position1;
	[SerializeField] private PositionScript position2;
	[SerializeField] private GameObject boatObject;
	[SerializeField] private Vector3 boatPositionQuaSongNot;
	[SerializeField] private Vector3 boatPositionQuaSongEd;

	//		Calculating Data
	private bool isQuaSong;



	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		this.Initiate();
	}

	private void Update()
	{
		
	}



	public void Initiate()
	{
		this.isQuaSong = false;
		this.boatObject.gameObject.transform.position = this.boatPositionQuaSongNot;
	}

	public void MoveBoat(bool usualCheck = true, bool boatQuaSong = false)
	{
		if (!this.position1.GetDude() &&
			!this.position2.GetDude() &&
			usualCheck)
		{
			Debug.Log("Boat is Empty!");

			return;
		}
		else if (!usualCheck)
		{
			this.isQuaSong = boatQuaSong;

			if (!this.isQuaSong)
			{
				this.boatObject.gameObject.transform.position = this.boatPositionQuaSongNot;
			}
			else
			{
				this.boatObject.gameObject.transform.position = this.boatPositionQuaSongEd;
			}
		}

		this.isQuaSong = !this.isQuaSong;

		if (!this.isQuaSong)
		{
			this.boatObject.gameObject.transform.position = this.boatPositionQuaSongNot;
		}
		else
		{
			this.boatObject.gameObject.transform.position = this.boatPositionQuaSongEd;
		}

		if (this.position1.GetDude())
		{
			PositionManager.Instance.MoveDude(this.position1.GetDude());
		}
		if (this.position2.GetDude())
		{
			PositionManager.Instance.MoveDude(this.position2.GetDude());
		}

		PositionManager.Instance.EvaluateIfLegal(pointEvaluation: false);
	}

	public bool MoveDudeToBoat(DudeQuaSongScript dude)
	{
		if (!position1.IsTaken())
		{
			dude.GetPositionScript().SetDude(null);

			this.position1.SetDude(dude);

			dude.SetPosition(
				this.position1,
				DudeQuaSongScript.GeneralPosition.OnBoat
				);

			return true;
		}
		else if (!position2.IsTaken())
		{
			dude.GetPositionScript().SetDude(null);

			this.position2.SetDude(dude);

			dude.SetPosition(
				this.position2,
				DudeQuaSongScript.GeneralPosition.OnBoat
				);

			return true;
		}
		else
		{
			return false;
		}
	}

	public bool MoveDudeFromBoat(DudeQuaSongScript dude)
	{
		if (this.position1.GetDude() &&
			this.position1.GetDude() == dude)
		{

			this.position1.SetDude(null);

			return true;
		}
		else if (this.position2.GetDude() &&
			this.position2.GetDude() == dude)
		{

			this.position2.SetDude(null);

			return true;
		}
		else
		{
			return false;
		}
	}

	//		Setter
	public void SetPosition(Vector3 position, bool isQuaSong)
	{
		this.gameObject.transform.position = position;
		this.isQuaSong = isQuaSong;
	}

	//		Getter
	public bool IsQuaSong()
	{
		return this.isQuaSong;
	}
}
