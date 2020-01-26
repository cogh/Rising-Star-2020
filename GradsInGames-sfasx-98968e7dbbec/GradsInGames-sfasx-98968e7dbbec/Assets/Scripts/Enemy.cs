using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float SingleNodeMoveTime = 0.5f;
    private bool busy = false;
    private Game gameScript;

    public EnvironmentTile CurrentPosition { get; set; }
    public Environment map;
    public bool dead;

    Time time;

    private void Start()
    {
        GameObject game = GameObject.Find("Game");
        gameScript = game.GetComponent<Game>();
    }

    private void Update()
    {
        /*if (Time.frameCount % 240 == 0)
        {
            EnvironmentTile target_tile = map.GetRandom();
            UseTile(target_tile);
            Debug.Log(CurrentPosition + " " + target_tile);
        }*/
        if (CurrentPosition == map.PlayerSpawn)
        {
            gameScript.health--;
            dead = true;
        }
        if (dead == true)
        {
            gameScript.enemy_list.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator DoMove(Vector3 position, Vector3 destination)
    {
        // Move between the two specified positions over the specified amount of time
        if (position != destination)
        {
            transform.rotation = Quaternion.LookRotation(destination - position, Vector3.up);

            Vector3 p = transform.position;
            float t = 0.0f;
            while (t < SingleNodeMoveTime)
            {
                t += Time.deltaTime;
                p = Vector3.Lerp(position, destination, t / SingleNodeMoveTime);
                transform.position = p;
                yield return null;
            }
        }
    }

    private IEnumerator DoGoTo(List<EnvironmentTile> route)
    {
        // Move through each tile in the given route
        if (route != null)
        {
            Vector3 position = CurrentPosition.Position;
            for (int count = 0; count < route.Count; ++count)
            {
                Vector3 next = route[count].Position;
                yield return DoMove(position, next);
                CurrentPosition = route[count];
                position = next;
            }
        }
    }

    private IEnumerator DoHarvest(EnvironmentTile tile)
    {
        for (int i = 0; i < 60; i++)
        {
            yield return null;
        }
        ManagePlacement script = tile.GetComponent<ManagePlacement>();
        script.Harvest();
        Debug.Log("Harvested");
        tile.IsAccessible = true;
        busy = false;
        yield return null;
    }

    private IEnumerator DoUseTile(EnvironmentTile tile)
    {
        // Go to
        List<EnvironmentTile> route = map.Solve(CurrentPosition, tile);
        if (route != null)
        {
            yield return DoGoTo(route);
        }
        // Harvest
        else
        {
            Debug.Log("Searching for adjacent route");
            List<EnvironmentTile> adjacent_route = map.SolveAdjacent(CurrentPosition, tile);
            if (adjacent_route != null)
            {
                Debug.Log("Adjacent route found");
                yield return DoGoTo(adjacent_route);
                Debug.Log("Harvesting");
                yield return DoHarvest(tile);
            }
        }
    }

    public void UseTile(EnvironmentTile tile)
    {
        // Clear all coroutines before starting the new route so 
        // that clicks can interupt any current route animation
        if (!busy)
        {
            StopAllCoroutines();
            StartCoroutine(DoUseTile(tile));
        }
    }
}
