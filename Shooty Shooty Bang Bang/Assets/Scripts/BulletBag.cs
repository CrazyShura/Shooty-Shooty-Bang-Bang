using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pull for bullets. Use GiveMeBullet() to get a bullet, do not forget to set bullet up.
/// </summary>
public class BulletBag : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Bullet bulletPrefab;
	[SerializeField]
	int initialCapacity;
	[SerializeField, Min(.1f), Tooltip("Speed of all bullets used provided by this bag")]
	float bulletSpeed;//This might be better at a Gun script but not gonna put connection for one field
	[SerializeField, Min(1f), Tooltip("How long bullet from this bag will remain active")]
	float bulletLifeTime;//TODO Ok now there is two fields that suld go on gun... GREAT

	Queue<Bullet> bullets = new Queue<Bullet>();
	Transform bulletMama;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Awake()
	{
		bulletMama = new GameObject("BulletBag").transform;
		bulletMama.parent = transform;
	}
	private void Start()
	{
		for (int _i = 0; _i < initialCapacity; _i++)
		{
			Bullet _tempBullet = Instantiate(bulletPrefab, bulletMama);
			_tempBullet.SetUp(this, bulletSpeed, bulletLifeTime);
			bullets.Enqueue(_tempBullet);
		}
	}
	public Bullet GiveMeBullet()
	{
		Bullet _bulletToGive;
		if (bullets.Count > 0)
		{
			_bulletToGive = bullets.Dequeue();
		}
		else
		{
			_bulletToGive = Instantiate(bulletPrefab, bulletMama);
			_bulletToGive.SetUp(this, bulletSpeed, bulletLifeTime);
		}
		return _bulletToGive;
	}
	public void ReturnBullet(Bullet lostChild)
	{
		bullets.Enqueue(lostChild);
	}
	#endregion
}
