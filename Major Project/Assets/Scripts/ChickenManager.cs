using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChickenManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnChickens;
    [SerializeField] private List<Transform> spawnObstacles = new List<Transform>();

    private float randomDeviation = 4.5f;

    private float minDistance = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        RandomiseChickenPositions();
    }

    private void RandomiseChickenPositions()
    {
        Vector2 randomPosition;
        // For each chicken in the spawnChickens array
        foreach (Transform chicken in spawnChickens)
        {
            int attempts = 0;
            do
            {
                // Pick a random position
                randomPosition = new Vector2(Random.Range(-randomDeviation, randomDeviation), Random.Range(-randomDeviation, randomDeviation));
            }
            while (spawnObstacles.Select(x => Vector2.Distance(x.position, chicken.localPosition)).Any(x => x < minDistance) && attempts++ < 100);

            // Move the chicken to the random position
            chicken.localPosition = new Vector3(randomPosition.x, chicken.position.y, randomPosition.y);

            // Add the chicken to the obstacle list
            spawnObstacles.Add(chicken);
        }

    }
}
