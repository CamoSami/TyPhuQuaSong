using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeQuaSongScript : MonoBehaviour
{
	//		Enum
	public enum PersonRole
	{
		Thief,
		Millionaire
	}

	public enum GeneralPosition
	{
		QuaSongEd,
		OnBoat,
		QuaSongNot,
	}

	//		Unity Assigned Data
	[SerializeField] private PersonRole role;

	//		Calculating Data
	private GeneralPosition generalPosition = GeneralPosition.QuaSongNot;
	private PositionScript currentPosition;



	private void Awake()
	{
		
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		
	}

	//		Setter
	public void SetPersonRole(PersonRole role)
	{
		this.role = role;
	}

	public void SetPosition(PositionScript currentPosition, GeneralPosition generalPosition)
	{
		this.generalPosition = generalPosition;
		this.currentPosition = currentPosition;

		this.gameObject.transform.position = currentPosition.transform.position;
	}



	//		Getter
	public PositionScript GetPositionScript()
	{
		return this.currentPosition;
	}

	public PersonRole GetRole()
	{
		return this.role;
	}

	public GeneralPosition GetGeneralPosition() 
	{
		return this.generalPosition;
	}
}
