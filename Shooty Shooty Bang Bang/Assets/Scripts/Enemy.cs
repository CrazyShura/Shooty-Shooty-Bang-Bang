using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Fields
	Waypoint host;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Awake()
	{
		host = transform.GetComponentInParent<Waypoint>();
		if(host == null)
		{
			Debug.LogError("Enemy has no waypoint assigned (Its not a child of any waypoint). This is not allowed");
			throw new ArgumentNullException();
		}
	}
	public void Ouch()
	{
		host.RemoveEnemy(this);
		Destroy(gameObject);
	}
	#endregion
}
