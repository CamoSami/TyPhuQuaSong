using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class AIManager : MonoBehaviour
{
	//		Instance
	public static AIManager Instance {  get; private set; }

	//		Extra Class
	public class AlgorithmStep
	{
		public int numOfThieves;
		public int numOfMillionaires;
		public bool boatQuaSong;
		public int evaluatedPriority;
		public AlgorithmStep lastStep;

		public bool CheckIfStepValidate()
		{
			return this.numOfThieves >= 0 && this.numOfMillionaires >= 0 &&
				this.numOfThieves <= 3 && this.numOfMillionaires <= 3;
		}

		public bool CheckIfStepVisited(List<AlgorithmStep> listOfOtherSteps)
		{
			foreach (AlgorithmStep otherStep in listOfOtherSteps) {
				if (this.numOfMillionaires == otherStep.numOfMillionaires &&
					this.numOfThieves == otherStep.numOfThieves &&
					this.boatQuaSong == otherStep.boatQuaSong)
				{
					return true;
				}
			}

			return false;
		}
	}

	[Serializable]
	public class PossibleStep
	{
		public int thievesMove;
		public int millionairesMove;
	}

	//		Enum
	public enum AlgorithmType
	{
		BFS,
		DFS,
		AStar,
		BranchAndBound,
		None
	}

	public enum StateMachine
	{
		Idle,
		Auto
	}

	//		Unity Assigned Data
	[SerializeField] private List<PossibleStep> listOfPossibleSteps;
	[SerializeField] private float maxTime = 2f;

	//		Calculating Data
	private StateMachine currentState = StateMachine.Idle;
	private int index = 0;
	private float currentTime = 0f;
	private List<AlgorithmStep> listOfCorrectSteps;
	private List<AlgorithmStep> listOfStepsVisited;
	private List<AlgorithmStep> listOfStepsWillVisit;



	private void Awake()
	{
		//		Initiate
		Instance = this;
	}

	private void Start()
	{
		//		Initiate
		//this.InitiateAlgorithm();
	}

	private void Update()
	{
		switch (this.currentState)
		{
			case StateMachine.Idle:


				break;
			case StateMachine.Auto:
				this.currentTime += Time.deltaTime;

				if (this.currentTime > maxTime)
				{
					this.currentTime = 0f;

					PositionManager.Instance.Initiate(this.listOfCorrectSteps[this.index]);

					Debug.Log("Millionaires: " + this.listOfCorrectSteps[this.index].numOfMillionaires +
							", Thieves: " + this.listOfCorrectSteps[this.index].numOfThieves + "" +
							", BoatQuaSong: " + this.listOfCorrectSteps[this.index].boatQuaSong);

					this.index++;

					if (this.index == this.listOfCorrectSteps.Count)
					{
						this.currentState = StateMachine.Idle;
					}
				}

				break;
		}
	}



	public void InitiateAlgorithm(AlgorithmType algorithmType)
	{
		switch (algorithmType)
		{
			case AlgorithmType.BFS:
				//Debug.Log(
				//	"Thieves: " + PositionManager.Instance.GetTheivesNotQuaSong() +
				//	", Millionaires: " + PositionManager.Instance.GetMillionairesNotQuaSong()
				//	);

				this.listOfCorrectSteps = this.AlgorithmBFS(new AlgorithmStep
				{
					numOfThieves = PositionManager.Instance.GetTheivesNotQuaSong(),
					numOfMillionaires = PositionManager.Instance.GetMillionairesNotQuaSong(),
					boatQuaSong = BoatManager.Instance.IsQuaSong()
				});

				this.index = 0;
				this.currentTime = 0f;
				this.currentState = StateMachine.Auto;

				break;
			case AlgorithmType.DFS:
				//Debug.Log(
				//	"Thieves: " + PositionManager.Instance.GetTheivesNotQuaSong() +
				//	", Millionaires: " + PositionManager.Instance.GetMillionairesNotQuaSong()
				//	);

				this.listOfCorrectSteps = this.AlgorithmDFS(new AlgorithmStep
				{
					numOfThieves = PositionManager.Instance.GetTheivesNotQuaSong(),
					numOfMillionaires = PositionManager.Instance.GetMillionairesNotQuaSong(),
					boatQuaSong = BoatManager.Instance.IsQuaSong()
				});

				this.index = 0;
				this.currentTime = 0f;
				this.currentState = StateMachine.Auto;

				break;
			case AlgorithmType.AStar:

				this.listOfCorrectSteps = this.AlgorithmAStar(new AlgorithmStep
				{
					numOfThieves = PositionManager.Instance.GetTheivesNotQuaSong(),
					numOfMillionaires = PositionManager.Instance.GetMillionairesNotQuaSong(),
					boatQuaSong = BoatManager.Instance.IsQuaSong(),
					evaluatedPriority = 0
				});

				this.index = 0;
				this.currentTime = 0f;
				this.currentState = StateMachine.Auto;

				break;
			case AlgorithmType.BranchAndBound:

				this.listOfCorrectSteps = this.AlgorithmBranchAndBound(new AlgorithmStep
				{
					numOfThieves = PositionManager.Instance.GetTheivesNotQuaSong(),
					numOfMillionaires = PositionManager.Instance.GetMillionairesNotQuaSong(),
					boatQuaSong = BoatManager.Instance.IsQuaSong(),
					evaluatedPriority = 0
				});

				this.index = 0;
				this.currentTime = 0f;
				this.currentState = StateMachine.Auto;

				break;
			case AlgorithmType.None:

				this.index = 0;
				this.currentTime = 0f;
				this.currentState = StateMachine.Idle;

				break;
		}
	}

	public List<AlgorithmStep> AlgorithmBFS(AlgorithmStep initialStep)
	{
		Debug.Log("Algorithm BFS Ran!");
		
		Queue<AlgorithmStep> queueAlgorithmSteps = new Queue<AlgorithmStep>();
		queueAlgorithmSteps.Enqueue(initialStep);

		this.listOfStepsVisited = new List<AlgorithmStep>();

		int numOfMillionaires, numOfThieves;
		bool boatQuaSong;

		while (queueAlgorithmSteps.Count > 0)
		{
			AlgorithmStep step = queueAlgorithmSteps.Dequeue();

			if (step.CheckIfStepVisited(this.listOfStepsVisited))
			{
				continue;
			}

			this.listOfStepsVisited.Add(step);

			if (
				step.numOfMillionaires == 0 &&
				step.numOfThieves == 0 &&
				step.boatQuaSong
				)
			{
				//		Algorithm Finished
				//Debug.Log("Algorithm Finished! ");

				List<AlgorithmStep> listOfCorrectSteps = new List<AlgorithmStep>();
				step = step.lastStep;

				while (step.lastStep != null)
				{
					//Debug.Log("Millionaires: " + step.numOfMillionaires +
					//	", Thieves: " + step.numOfThieves + "" +
					//	", BoatQuaSong: " + step.boatQuaSong);

					listOfCorrectSteps.Add(step);

					step = step.lastStep;
				}

				//Debug.Log("Millionaires: " + step.numOfMillionaires +
				//		", Thieves: " + step.numOfThieves + "" +
				//		", BoatQuaSong: " + step.boatQuaSong);

				//Debug.Log("Visited: " + this.listOfStepsVisited.Count);

				listOfCorrectSteps.Add(step);

				return listOfCorrectSteps;
			}

			for (int i = 0; i < 5; i++)
			{
				numOfMillionaires = !step.boatQuaSong ?
					step.numOfMillionaires - this.listOfPossibleSteps[i].millionairesMove :
					step.numOfMillionaires + this.listOfPossibleSteps[i].millionairesMove;
				numOfThieves = !step.boatQuaSong ?
					step.numOfThieves - this.listOfPossibleSteps[i].thievesMove :
					step.numOfThieves + this.listOfPossibleSteps[i].thievesMove;
				boatQuaSong = !step.boatQuaSong;

				AlgorithmStep nextStep = new AlgorithmStep
				{
					numOfMillionaires = numOfMillionaires,
					numOfThieves = numOfThieves,
					boatQuaSong = boatQuaSong,
					lastStep = step
				};

				if (nextStep.CheckIfStepValidate() &&
					PositionManager.Instance.EvaluateIfLegal(pointEvaluation: true, numOfMillionaires, numOfThieves)
					)
				{
					queueAlgorithmSteps.Enqueue(nextStep);
				}
			}
		}

		return null;
	}

	public List<AlgorithmStep> AlgorithmDFS(AlgorithmStep initialStep)
	{
		Debug.Log("Algorithm DFS Ran!");

		Stack<AlgorithmStep> queueAlgorithmSteps = new Stack<AlgorithmStep>();
		queueAlgorithmSteps.Push(initialStep);

		this.listOfStepsVisited = new List<AlgorithmStep>();

		int numOfMillionaires, numOfThieves;
		bool boatQuaSong;

		while (queueAlgorithmSteps.Count > 0)
		{
			AlgorithmStep step = queueAlgorithmSteps.Pop();

			if (step.CheckIfStepVisited(this.listOfStepsVisited))
			{
				continue;
			}

			this.listOfStepsVisited.Add(step);

			if (
				step.numOfMillionaires == 0 &&
				step.numOfThieves == 0 &&
				step.boatQuaSong
				)
			{
				//Algorithm Finished
				//Debug.Log("Algorithm Finished! ");

				List<AlgorithmStep> listOfCorrectSteps = new List<AlgorithmStep>();
				step = step.lastStep;

				while (step.lastStep != null)
				{
					//Debug.Log("Millionaires: " + step.numOfMillionaires +
					//	", Thieves: " + step.numOfThieves + "" +
					//	", BoatQuaSong: " + step.boatQuaSong);

					listOfCorrectSteps.Add(step);

					step = step.lastStep;
				}

				//Debug.Log("Millionaires: " + step.numOfMillionaires +
				//		", Thieves: " + step.numOfThieves + "" +
				//		", BoatQuaSong: " + step.boatQuaSong);

				//Debug.Log("Visited: " + this.listOfStepsVisited.Count);

				listOfCorrectSteps.Add(step);

				return listOfCorrectSteps;
			}

			for (int i = 4; i >= 0; i--)
			{
				numOfMillionaires = !step.boatQuaSong ?
					step.numOfMillionaires - this.listOfPossibleSteps[i].millionairesMove :
					step.numOfMillionaires + this.listOfPossibleSteps[i].millionairesMove;
				numOfThieves = !step.boatQuaSong ?
					step.numOfThieves - this.listOfPossibleSteps[i].thievesMove :
					step.numOfThieves + this.listOfPossibleSteps[i].thievesMove;
				boatQuaSong = !step.boatQuaSong;

				AlgorithmStep nextStep = new AlgorithmStep
				{
					numOfMillionaires = numOfMillionaires,
					numOfThieves = numOfThieves,
					boatQuaSong = boatQuaSong,
					lastStep = step
				};

				if (nextStep.CheckIfStepValidate() &&
					PositionManager.Instance.EvaluateIfLegal(pointEvaluation: true, numOfMillionaires, numOfThieves)
					)
				{
					queueAlgorithmSteps.Push(nextStep);
				}
			}
		}

		return null;
	}

	public List<AlgorithmStep> AlgorithmAStar(AlgorithmStep initialStep)
	{
		//		TODO: Test with one set of List<Location> first!
		Debug.Log("Algorithm A Star Ran!");

		//		Initiate
		List<AlgorithmStep> priorListAlgorithmSteps = new List<AlgorithmStep>();
		priorListAlgorithmSteps.Add(initialStep);

		this.listOfStepsVisited = new List<AlgorithmStep>();

		int numOfMillionaires, numOfThieves, evaluatedPriorty;
		bool boatQuaSong;

		//		Algorithm
		while (priorListAlgorithmSteps.Count > 0)
		{
			AlgorithmStep step = priorListAlgorithmSteps[0];
			priorListAlgorithmSteps.RemoveAt(0);

			if (step.CheckIfStepVisited(this.listOfStepsVisited))
			{
				continue;
			}

			//		Add
			this.listOfStepsVisited.Add(step);

			//		Check if Won
			if (
				step.numOfMillionaires == 0 &&
				step.numOfThieves == 0 &&
				step.boatQuaSong
				)
			{
				//Algorithm Finished
				//Debug.Log("Algorithm Finished! ");

				List<AlgorithmStep> listOfCorrectSteps = new List<AlgorithmStep>();
				step = step.lastStep;

				while (step.lastStep != null)
				{
					//Debug.Log("Millionaires: " + step.numOfMillionaires +
					//	", Thieves: " + step.numOfThieves + "" +
					//	", BoatQuaSong: " + step.boatQuaSong);

					listOfCorrectSteps.Add(step);

					step = step.lastStep;
				}

				//Debug.Log("Millionaires: " + step.numOfMillionaires +
				//		", Thieves: " + step.numOfThieves + "" +
				//		", BoatQuaSong: " + step.boatQuaSong);

				//Debug.Log("Visited: " + this.listOfStepsVisited.Count);

				listOfCorrectSteps.Add(step);

				return listOfCorrectSteps;
			}

			//		Add more!
			for (int i = 4; i >= 0; i--)
			{
				numOfMillionaires = !step.boatQuaSong ?
					step.numOfMillionaires - this.listOfPossibleSteps[i].millionairesMove :
					step.numOfMillionaires + this.listOfPossibleSteps[i].millionairesMove;
				numOfThieves = !step.boatQuaSong ?
					step.numOfThieves - this.listOfPossibleSteps[i].thievesMove :
					step.numOfThieves + this.listOfPossibleSteps[i].thievesMove;
				boatQuaSong = !step.boatQuaSong;
				evaluatedPriorty = step.evaluatedPriority + 1;

				AlgorithmStep nextStep = new AlgorithmStep
				{
					numOfMillionaires = numOfMillionaires,
					numOfThieves = numOfThieves,
					boatQuaSong = boatQuaSong,
					lastStep = step,
					evaluatedPriority = evaluatedPriorty
				};

				if (nextStep.CheckIfStepValidate() &&
					PositionManager.Instance.EvaluateIfLegal(pointEvaluation: true, numOfMillionaires, numOfThieves)
					)
				{
					for (int j = 0; j < priorListAlgorithmSteps.Count; j++)
					{
						if (priorListAlgorithmSteps[j].evaluatedPriority > nextStep.evaluatedPriority)
						{
							priorListAlgorithmSteps.Add(nextStep);

							int tempMax = priorListAlgorithmSteps.Count - 1;

							while (tempMax != j)
							{
								priorListAlgorithmSteps[tempMax] = priorListAlgorithmSteps[tempMax-- - 1];
							}

							priorListAlgorithmSteps[j] = nextStep;

							break;
						}
					}

					if (!priorListAlgorithmSteps.Contains(nextStep))
					{
						priorListAlgorithmSteps.Add(nextStep);
					}
				}
			}
		}

		return null;
	}

	public List<AlgorithmStep> AlgorithmBranchAndBound(AlgorithmStep initialStep)
	{
		//		TODO: Test with one set of List<Location> first!
		Debug.Log("Algorithm Branch and Bound Ran!");

		//		Initiate

		List<AlgorithmStep> priorListAlgorithmSteps = new List<AlgorithmStep>();
		priorListAlgorithmSteps.Add(initialStep);

		this.listOfStepsVisited = new List<AlgorithmStep>();

		int numOfMillionaires, numOfThieves, evaluatedPriorty, minEvaluation = 999999;
		bool boatQuaSong;
		List<AlgorithmStep> listOfCorrectSteps = new List<AlgorithmStep>();

		//		Algorithm
		while (priorListAlgorithmSteps.Count > 0)
		{
			AlgorithmStep step = priorListAlgorithmSteps[0];
			priorListAlgorithmSteps.RemoveAt(0);

			if (
				step.CheckIfStepVisited(this.listOfStepsVisited) ||
				step.evaluatedPriority >= minEvaluation)
			{
				continue;
			}

			//		Add
			this.listOfStepsVisited.Add(step);

			//		Check if Won
			if (
				step.numOfMillionaires == 0 &&
				step.numOfThieves == 0 &&
				step.boatQuaSong
				)
			{
				//Algorithm Finished
				//Debug.Log("Algorithm Finished! ");
				minEvaluation = step.evaluatedPriority;
				step = step.lastStep;

				while (step.lastStep != null)
				{
					//Debug.Log("Millionaires: " + step.numOfMillionaires +
					//	", Thieves: " + step.numOfThieves + "" +
					//	", BoatQuaSong: " + step.boatQuaSong);

					listOfCorrectSteps.Add(step);

					step = step.lastStep;
				}

				//Debug.Log("Millionaires: " + step.numOfMillionaires +
				//		", Thieves: " + step.numOfThieves + "" +
				//		", BoatQuaSong: " + step.boatQuaSong);

				//Debug.Log("Visited: " + this.listOfStepsVisited.Count);

				listOfCorrectSteps.Add(step);
			}

			//		Add more!
			for (int i = 4; i >= 0; i--)
			{
				numOfMillionaires = !step.boatQuaSong ?
					step.numOfMillionaires - this.listOfPossibleSteps[i].millionairesMove :
					step.numOfMillionaires + this.listOfPossibleSteps[i].millionairesMove;
				numOfThieves = !step.boatQuaSong ?
					step.numOfThieves - this.listOfPossibleSteps[i].thievesMove :
					step.numOfThieves + this.listOfPossibleSteps[i].thievesMove;
				boatQuaSong = !step.boatQuaSong;
				evaluatedPriorty = step.evaluatedPriority + 1;

				AlgorithmStep nextStep = new AlgorithmStep
				{
					numOfMillionaires = numOfMillionaires,
					numOfThieves = numOfThieves,
					boatQuaSong = boatQuaSong,
					lastStep = step,
					evaluatedPriority = evaluatedPriorty
				};

				if (nextStep.CheckIfStepValidate() &&
					PositionManager.Instance.EvaluateIfLegal(pointEvaluation: true, numOfMillionaires, numOfThieves)
					)
				{
					for (int j = 0; j < priorListAlgorithmSteps.Count; j++)
					{
						if (priorListAlgorithmSteps[j].evaluatedPriority > nextStep.evaluatedPriority)
						{
							priorListAlgorithmSteps.Add(nextStep);

							int tempMax = priorListAlgorithmSteps.Count - 1;

							while (tempMax != j)
							{
								priorListAlgorithmSteps[tempMax] = priorListAlgorithmSteps[tempMax-- - 1];
							}

							priorListAlgorithmSteps[j] = nextStep;

							break;
						}
					}

					if (!priorListAlgorithmSteps.Contains(nextStep))
					{
						priorListAlgorithmSteps.Add(nextStep);
					}
				}
			}
		}

		return listOfCorrectSteps;
	}

	public void Initiate()
	{
		
	}
}
