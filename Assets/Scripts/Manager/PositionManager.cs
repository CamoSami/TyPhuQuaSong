using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DudeQuaSongScript;

public class PositionManager : MonoBehaviour
{
	//		Instance
	public static PositionManager Instance { get; private set; }

	//		Unity Assigned Data
	//			Position
	[SerializeField] private List<PositionScript> listOfPositionQuaSongNot;
	[SerializeField] private List<PositionScript> listOfPositionQuaSongEd;
	//			Dudes
	[SerializeField] private GameObject millionaireObject;
	[SerializeField] private GameObject thiefObject;
	//			Spawn Here
	[SerializeField] private Transform dudeSpawnHere;

	//		Calculating Data
	private List<DudeQuaSongScript> listOfMillionaires = new List<DudeQuaSongScript>();
	private List<DudeQuaSongScript> listOfThieves = new List<DudeQuaSongScript>();


	public void Awake()
	{
		//		Instance
		PositionManager.Instance = this;

		
	}

	public void Start()
	{
		this.Initiate();
	}

	public void Update()
	{
		
	}



	public void Initiate(AIManager.AlgorithmStep algorithmStep = null)
	{
		//		Delete
		foreach (DudeQuaSongScript dude in this.listOfMillionaires)
		{
			dude.GetPositionScript().SetDude(null);

			GameObject.Destroy(dude.gameObject);
		}

		foreach (DudeQuaSongScript dude in this.listOfThieves)
		{
			dude.GetPositionScript().SetDude(null);

			GameObject.Destroy(dude.gameObject);
		}

		this.listOfMillionaires = new List<DudeQuaSongScript>();
		this.listOfThieves = new List<DudeQuaSongScript>();

		//		Summoning
		if (algorithmStep != null)
		{
			//		Special
			int indexQuaSongNot = 0, indexQuaSongEd = 0;

			for (int i = 0; i < 3; i++)
			{
				GameObject millionaire = GameObject.Instantiate(
					this.millionaireObject,
					this.dudeSpawnHere
					);

				DudeQuaSongScript millionaireScript = millionaire.GetComponent<DudeQuaSongScript>();
				this.listOfMillionaires.Add(millionaireScript);

				if (i >= algorithmStep.numOfMillionaires)
				{
					millionaireScript.SetPosition(
						this.listOfPositionQuaSongNot[indexQuaSongNot],
						GeneralPosition.QuaSongNot
						);
					this.listOfPositionQuaSongNot[indexQuaSongNot].SetDude(millionaireScript);

					indexQuaSongNot++;
				}
				else
				{
					millionaireScript.SetPosition(
						this.listOfPositionQuaSongEd[indexQuaSongEd],
						GeneralPosition.QuaSongEd
						);
					this.listOfPositionQuaSongEd[indexQuaSongEd].SetDude(millionaireScript);

					indexQuaSongEd++;
				}
			}

			for (int i = 0; i < 3; i++)
			{
				GameObject thief = GameObject.Instantiate(
					this.thiefObject,
					this.dudeSpawnHere
					);

				DudeQuaSongScript thiefScript = thief.GetComponent<DudeQuaSongScript>();
				this.listOfThieves.Add(thiefScript);

				if (i >= algorithmStep.numOfThieves)
				{
					thiefScript.SetPosition(
						this.listOfPositionQuaSongNot[indexQuaSongNot],
						GeneralPosition.QuaSongNot
						);
					this.listOfPositionQuaSongNot[indexQuaSongNot].SetDude(thiefScript);

					indexQuaSongNot++;
				}
				else
				{
					thiefScript.SetPosition(
						this.listOfPositionQuaSongEd[indexQuaSongEd],
						GeneralPosition.QuaSongEd
						);
					this.listOfPositionQuaSongEd[indexQuaSongEd].SetDude(thiefScript);

					indexQuaSongEd++;
				}
			}

			BoatManager.Instance.MoveBoat(usualCheck: false, boatQuaSong: algorithmStep.boatQuaSong);
		}
		else
		{
			//		Usual
			//			Millionaire
			for (int i = 0; i < 3; i++)
			{
				GameObject millionaire = GameObject.Instantiate(
					this.millionaireObject,
					this.dudeSpawnHere
					);

				DudeQuaSongScript millionaireScript = millionaire.GetComponent<DudeQuaSongScript>();
				this.listOfMillionaires.Add(millionaireScript);

				millionaireScript.SetPosition(
					this.listOfPositionQuaSongNot[i],
					GeneralPosition.QuaSongNot
					);
				this.listOfPositionQuaSongNot[i].SetDude(millionaireScript);
			}

			//			Thief
			for (int i = 3; i < 6; i++)
			{
				GameObject thief = GameObject.Instantiate(
					this.thiefObject,
					this.dudeSpawnHere
					);

				DudeQuaSongScript thiefScript = thief.GetComponent<DudeQuaSongScript>();
				this.listOfThieves.Add(thiefScript);

				thiefScript.SetPosition(
					this.listOfPositionQuaSongNot[i],
					GeneralPosition.QuaSongNot
					);
				this.listOfPositionQuaSongNot[i].SetDude(thiefScript);
			}
		}
	}

	public void MoveDude(DudeQuaSongScript dude)
	{
		this.MoveDudeToPosition(dude.GetGeneralPosition(), dude);
	}

	private void MoveDudeToPosition(DudeQuaSongScript.GeneralPosition generalPosition, DudeQuaSongScript dude)
	{
		switch (generalPosition)
		{
			case GeneralPosition.QuaSongEd:
				//		Move from ... to Boat
				if (!BoatManager.Instance.IsQuaSong())
				{
					//		Can't
					Debug.Log("Can't go to Boat");

					return;
				}

				if (!BoatManager.Instance.MoveDudeToBoat(dude))
				{
					Debug.Log("Boat is Full!");

					return;
				}

				break;
			case GeneralPosition.QuaSongNot:
				//		Move from ... to Boat
				if (BoatManager.Instance.IsQuaSong())
				{
					//		Can't
					Debug.Log("Can't go to Boat");

					return;
				}

				if (!BoatManager.Instance.MoveDudeToBoat(dude))
				{
					Debug.Log("Boat is Full!");

					return;
				}

				break;
			case GeneralPosition.OnBoat:
				//		Move from Boat to ...

				if (!BoatManager.Instance.MoveDudeFromBoat(dude))
				{
					Debug.Log("Dude not on Boat xd");
				}
				
				if (BoatManager.Instance.IsQuaSong())
				{
					//		Qua Song Ed

					foreach (PositionScript position in this.listOfPositionQuaSongEd)
					{
						if (!position.IsTaken())
						{
							position.SetDude(dude);

							dude.SetPosition(
								position,
								DudeQuaSongScript.GeneralPosition.QuaSongEd
								);

							break;
						}
					}
				}
				else
				{
					//		Not Qua Song

					foreach (PositionScript position in this.listOfPositionQuaSongNot)
					{
						if (!position.IsTaken())
						{
							position.SetDude(dude);

							dude.SetPosition(
								position, 
								DudeQuaSongScript.GeneralPosition.QuaSongNot
								);

							break;
						}
					}
				}
				
				break;
		}
	}

	public bool EvaluateIfLegal(bool pointEvaluation, int numOfMillionaires = 0, int numOfThieves = 0)
	{
		if (pointEvaluation)
		{
			if (numOfMillionaires == 0 && numOfThieves == 0)
			{
				return true;
			}
			else if (numOfMillionaires >= numOfThieves)
			{
				if (numOfMillionaires == numOfThieves)
				{
					return true;
				}
				else if (numOfThieves == 0 && numOfMillionaires > 0 && numOfMillionaires != 3)
				{
					return false;
				}
				else if (numOfMillionaires == 3)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				if (numOfMillionaires == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		else
		{
			//		Qua Song Not
			numOfMillionaires = 0;
			numOfThieves = 0;

			foreach (PositionScript position in this.listOfPositionQuaSongNot)
			{
				DudeQuaSongScript dude = position.GetDude();

				if (dude)
				{
					if (dude.GetRole() == PersonRole.Millionaire)
					{
						numOfMillionaires++;
					}
					else
					{
						numOfThieves++;
					}
				}
			}

			if (numOfMillionaires < numOfThieves && numOfMillionaires != 0)
			{
				//		ILLEGAL
				Debug.Log("You LOST" +
					", millionaires: " + numOfMillionaires +
					", thieves: " + numOfThieves);

				return false;
			}

			//		Qua Song Ed
			numOfMillionaires = 0;
			numOfThieves = 0;

			foreach (PositionScript position in this.listOfPositionQuaSongEd)
			{
				DudeQuaSongScript dude = position.GetDude();

				if (dude)
				{
					if (dude.GetRole() == PersonRole.Millionaire)
					{
						numOfMillionaires++;
					}
					else
					{
						numOfThieves++;
					}
				}
			}

			if (numOfMillionaires < numOfThieves && numOfMillionaires != 0)
			{
				//		ILLEGAL
				Debug.Log("You LOST" +
					", millionaires: " + numOfMillionaires +
					", thieves: " + numOfThieves);

				return false;
			}

			int tong = numOfMillionaires + numOfThieves;
			if (tong >= 6)
			{
				//		WON!
				Debug.Log("You won, congrats" +
					", millionaires: " + numOfMillionaires +
					", thieves: " + numOfThieves);

				return true;
			}

			return true;
		}
	}

	public int GetTheivesNotQuaSong()
	{
		int numOfThieves = 0;

		foreach (PositionScript position in this.listOfPositionQuaSongNot)
		{
			if (
				position.GetDude()!= null &&
				position.GetDude().GetRole() == DudeQuaSongScript.PersonRole.Thief
				)
			{
				numOfThieves++;
			}
		}

		return numOfThieves;
	}

	public int GetMillionairesNotQuaSong()
	{
		int numOfMillionaires = 0;

		foreach (PositionScript position in this.listOfPositionQuaSongNot)
		{
			if (
				position.GetDude() != null && 
				position.GetDude().GetRole() == DudeQuaSongScript.PersonRole.Millionaire
				)
			{
				numOfMillionaires++;
			}
		}

		return numOfMillionaires;
	}
}
