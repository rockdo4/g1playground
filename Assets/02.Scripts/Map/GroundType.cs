using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundType : MonoBehaviour
{
	public enum Type
	{
		None = -1,
		Grass,
		Sand,
		Village,
	}

	public Type type;

	public Type GetGroundType()
	{
		return type;
	}

}
