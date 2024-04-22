using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInputManager : MonoBehaviour
{
	//		Instance
	public static MouseInputManager Instance { get; private set; }

	//		Unity Assigned Data
	[SerializeField] private Controls controls;
	[SerializeField] private UnityEngine.UI.Button moveBoatButton;
	[SerializeField] private UnityEngine.UI.Button resetButton;
	[SerializeField] private UnityEngine.UI.Button bfsButton;
	[SerializeField] private UnityEngine.UI.Button dfsButton;
	[SerializeField] private UnityEngine.UI.Button aStarButton;
	[SerializeField] private UnityEngine.UI.Button branchAndBoundButton;

	//		Calculating Data
	private DudeQuaSongScript highlightedDude;



	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		//		Initiate
		this.controls = new Controls();
		this.controls.MouseAndKB.Enable();

		//		Event
		this.controls.MouseAndKB.LeftClick.performed += 
			actionContext =>
			{
				if (this.highlightedDude != null)
				{
					PositionManager.Instance.MoveDude(this.highlightedDude);
				}
				else
				{
					//Debug.Log("Highlighted Dude = null!");
				}
			};

		this.moveBoatButton.onClick.AddListener(() =>
		{
			BoatManager.Instance.MoveBoat();
		});

		this.resetButton.onClick.AddListener(() =>
		{
			AIManager.Instance.InitiateAlgorithm(AIManager.AlgorithmType.None);
			BoatManager.Instance.Initiate();
			PositionManager.Instance.Initiate();
		});

		this.bfsButton.onClick.AddListener(() =>
		{
			AIManager.Instance.InitiateAlgorithm(AIManager.AlgorithmType.BFS);
		});

		this.dfsButton.onClick.AddListener(() =>
		{
			AIManager.Instance.InitiateAlgorithm(AIManager.AlgorithmType.DFS);
		});

		this.aStarButton.onClick.AddListener(() =>
		{
			AIManager.Instance.InitiateAlgorithm(AIManager.AlgorithmType.AStar);
		});

		this.branchAndBoundButton.onClick.AddListener(() =>
		{
			AIManager.Instance.InitiateAlgorithm(AIManager.AlgorithmType.BranchAndBound);
		});
	}

	private void Update()
	{
		if (this.CheckIfMouseClickDude(out DudeQuaSongScript dude))
		{
			this.highlightedDude = dude;
		}
		else
		{
			this.highlightedDude = null;
		}
	}



	public bool CheckIfMouseClickDude(out DudeQuaSongScript dude)
	{
		if (this.CheckIfMouseClickColliderRaycast(out RaycastHit hitInfo))
		{
			//Debug.Log("_MapManager.Start()\n" +
			//	"hitInfo: " + hitInfo.collider.name);

			Transform transform = hitInfo.transform;

			return transform.TryGetComponent<DudeQuaSongScript>(out dude);
		}
		else
		{
			dude = null;

			return false;
		}
	}



	public bool CheckIfMouseClickColliderRaycast(out RaycastHit hitInfo)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Physics.Raycast(ray, out hitInfo);

		if (hitInfo.collider == null)
		{
			//		Can not scan a Collider, that's all

			return false;
		}
		else if (EventSystem.current.IsPointerOverGameObject())
		{
			//		Mouse if currently over UI

			return false;
		}
		else
		{
			//		Works normally

			return true;
		}
	}
}
