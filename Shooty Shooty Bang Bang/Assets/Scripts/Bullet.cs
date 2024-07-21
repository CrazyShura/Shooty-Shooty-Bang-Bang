using System;
using UnityEngine;

/// <summary>
/// Bullets. Suld be used in only in tandom wiht BulletBag class. To make bullet actually do bullet stuff use Go(Vector3 from, Vector3 to).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	#region Fields
	bool setup = false;
	bool going = false;
	BulletBag host;
	float speed;
	float lifeTime;
	Rigidbody rgbd;

	float timeLeft;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Update()
	{
		if (going)
		{
			timeLeft -= Time.deltaTime;
			if (timeLeft <= 0)
			{
				ReturnHome();
			}
		}
	}
	public void SetUp(BulletBag mom, float speed, float lifeTime)
	{
		if (setup)
		{
			Debug.LogError("Something went very wrong. Trying to setup bullet that was already set up");
			throw new InvalidOperationException();
		}
		host = mom;
		this.speed = speed;
		this.lifeTime = lifeTime;
		rgbd = GetComponent<Rigidbody>();
		gameObject.SetActive(false);
		setup = true;
	}

	/// <summary>
	/// Makes bullet teleport to position "from" and start going to position "to"
	/// </summary>
	public void Go(Vector3 from, Vector3 to)
	{
		transform.parent = null;
		transform.position = from;
		transform.forward = (to - from).normalized;
		rgbd.velocity = transform.forward * speed;
		gameObject.SetActive(true);
		timeLeft = lifeTime;
		going = true;
	}

	void ReturnHome()
	{
		transform.parent = host.BulletMama;
		gameObject.SetActive(false);
		going = false;
		rgbd.velocity = Vector3.zero;
		rgbd.angularVelocity = Vector3.zero;
		host.ReturnBullet(this);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<Enemy>().Ouch(); 
		}
		ReturnHome();
	}
	#endregion
}
