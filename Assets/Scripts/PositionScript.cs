using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScript : MonoBehaviour
{
    private bool isTaken = false;
	private DudeQuaSongScript dudeQuaSong = null;

	public void SetDude(DudeQuaSongScript dudeQuaSong)
	{
		this.dudeQuaSong = dudeQuaSong;

		if (this.dudeQuaSong != null)
		{
			this.isTaken = true;
		}
		else
		{
			this.isTaken = false;
		}
	}

	public bool IsTaken()
	{
		return this.isTaken;
	}

	public DudeQuaSongScript GetDude()
	{
		return this.dudeQuaSong;
	}

	public Vector3 GetPosition()
	{
		return this.gameObject.transform.position;
	}
}
