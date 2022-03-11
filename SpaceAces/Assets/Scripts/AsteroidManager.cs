using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField]int numAsteroids = 10;
    [SerializeField]int spaceInBetweenMin = 100;
    [SerializeField]int spaceInBetweenMax = 200;
    [SerializeField]Asteroid asteroid;

    void Start()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        // Spawn asteroids at random distance between each other. Each loop represents an axis (x,y,z).
        for (int i = 0; i < numAsteroids; i++)
        {
            for (int j = 0; j < numAsteroids; j++)
            {
                for (int k = 0; k < numAsteroids; k++)
                {
                    int spaceInBetween =    Random.Range(spaceInBetweenMin, spaceInBetweenMax);
                    CreateAsteroid(i * spaceInBetween, j * spaceInBetween, k * spaceInBetween);
                }
            }
        }
            
    }

    void CreateAsteroid(int x, int y, int z)
    {
        // Another random variable to avoid patterns in Asteroid spawnings.
        float r = Random.Range(-numAsteroids / 2f, numAsteroids / 2f);

        // Create location based on passed x,y,z coordinates.
        Vector3 newLocation = new Vector3(transform.position.x + x + r, transform.position.y + y + r, transform.position.z + z + r);

        // Bring asteroid into a new location.
        Instantiate(asteroid, newLocation, Quaternion.identity, transform);
    }
}