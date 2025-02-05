﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float SingleNodeMoveTime = 0.5f;
    private bool busy = false;

    public EnvironmentTile CurrentPosition { get; set; }
    public Environment map;

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

    private IEnumerator DoBuild(EnvironmentTile tile)
    {
        for (int i = 0; i < 60; i++)
        {
            yield return null;
        }
        ManagePlacement script = tile.GetComponent<ManagePlacement>();
        script.Build();
        Debug.Log("Built");
        tile.IsAccessible = true;
        busy = false;
        yield return null;
    }

    private IEnumerator DoUpgrade(EnvironmentTile tile)
    {
        for (int i = 0; i < 60; i++)
        {
            yield return null;
        }
        ManagePlacement script = tile.GetComponent<ManagePlacement>();
        script.Upgrade();
        Debug.Log("Built");
        tile.IsAccessible = true;
        busy = false;
        yield return null;
    }

    private IEnumerator DoUseTile(EnvironmentTile tile)
    {
        // Go to
        List<EnvironmentTile> route = map.Solve(CurrentPosition, tile);
        if (route != null && route.Count > 2)
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

    private IEnumerator DoBuildTile(EnvironmentTile tile)
    {
        // Go to
        List<EnvironmentTile> route = map.Solve(CurrentPosition, tile);
        if (route != null && route.Count > 2)
        {
            Debug.Log("Searching for adjacent route");
            List<EnvironmentTile> adjacent_route = map.SolveAdjacent(CurrentPosition, tile);
            if (adjacent_route != null)
            {
                Debug.Log("Adjacent route found");
                yield return DoGoTo(adjacent_route);
                Debug.Log("Building");
                yield return DoBuild(tile);
            }
        }
        // Build
        else
        {
            Debug.Log("Searching for adjacent route");
            List<EnvironmentTile> adjacent_route = map.SolveAdjacent(CurrentPosition, tile);
            if (adjacent_route != null)
            {
                Debug.Log("Adjacent route found");
                yield return DoGoTo(adjacent_route);
                Debug.Log("Upgrading");
                yield return DoUpgrade(tile);
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

    public void BuildTile(EnvironmentTile tile)
    {
        // Clear all coroutines before starting the new route so 
        // that clicks can interupt any current route animation
        if (!busy)
        {
            StopAllCoroutines();
            StartCoroutine(DoBuildTile(tile));
        }
    }
}
