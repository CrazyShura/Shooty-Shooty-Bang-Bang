using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Waypoint : MonoBehaviour
{
	#region Fields
	UnityEvent<Waypoint> WaypointReached = new UnityEvent<Waypoint>();
	UnityEvent<Waypoint> WaypointCleared = new UnityEvent<Waypoint>();
	List<Enemy> enemies = new List<Enemy>();
	#endregion

	#region Properties
	public Vector3 Postition { get => transform.position; }
	#endregion

	#region Methods#
	private void Awake()
	{
		EventManager.WaypointWasReached.AddInvoker(WaypointReached);
		EventManager.WaypointWasCleared.AddInvoker(WaypointCleared);
		int _childCount = transform.childCount;
		if (_childCount == 0)
		{
			Debug.Log("Waypoint has no enemies assigned (no nemies are a direct child of this object), Is this intended?");
		}
		for (int _i = 0; _i < _childCount; _i++)
		{
			Enemy _tempEnemy = transform.GetChild(_i).GetComponent<Enemy>();
			if (_tempEnemy != null)
			{
				enemies.Add(_tempEnemy);
			}
			else
			{
				Debug.LogError("Waypoint has child that is not an enemy, this is not allowed");
				throw new InvalidOperationException();
			}
		}
	}
	public void RemoveEnemy(Enemy enemy)
	{
		if (enemy == null)
		{
			throw new ArgumentNullException();
		}
		if (enemies.Contains(enemy))
		{
			enemies.Remove(enemy);
			if (enemies.Count == 0)
			{
				WaypointCleared.Invoke(this);
			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			WaypointReached.Invoke(this);
			if (enemies.Count == 0)
			{
				WaypointCleared.Invoke(this);
			}
		}
	}
	#endregion
}
