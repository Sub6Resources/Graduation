using System;
using UnityEngine;
using Random = System.Random;

public class SpawnStuffScript : MonoBehaviour
{

	public float SpawnDelay = 5f;
	public float Decay = 0.001f;
	public GameObject[] Spawnables;
	private float _spawnDelay;
	private Random _random;

	// Use this for initialization
	void Start ()
	{
		_random = new Random();
		_spawnDelay = SpawnDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_spawnDelay -= Time.deltaTime;
		Console.WriteLine(Time.deltaTime);
		SpawnDelay -= Decay * Time.deltaTime * 2 * Time.realtimeSinceStartup;
		if (_spawnDelay <= 0)
		{
			_spawnDelay = SpawnDelay;
			SpawnRandomItem();
		}
	}

	private void SpawnRandomItem()
	{
		Vector2 position = new Vector2(15, _random.Next(-2, 5));
		GameObject toSpawn = Spawnables[_random.Next(0, Spawnables.Length)];
		GameObject spawnedObject = Instantiate(toSpawn);
		spawnedObject.transform.position = position;
	}
}
