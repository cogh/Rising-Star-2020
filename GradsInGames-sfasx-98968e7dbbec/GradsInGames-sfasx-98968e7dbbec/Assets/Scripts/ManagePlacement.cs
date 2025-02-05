﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePlacement : MonoBehaviour
{
    public GameObject[] placement_list;
    private GameObject current_placement;
    private Game gameScript;
    public bool newPlacementOnStart;
    // Start is called before the first frame update
    void Start()
    {
        if (newPlacementOnStart)
        {
            NewPlacement();
        }
        GameObject game = GameObject.Find("Game");
        gameScript = game.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NewPlacement();
        }
    }

    public void NewPlacement()
    {
        GameObject new_placement = placement_list[Random.Range(1, placement_list.Length)];
        Vector3 new_position = transform.position + new Vector3(5, 3, 5);
        Destroy(current_placement);
        current_placement = Instantiate(new_placement, new_position, transform.rotation);
    }

    public void Build()
    {
        if (gameScript.stone > 0 && gameScript.wood > 0)
        {
            GameObject new_placement = placement_list[0];
            Vector3 new_position = transform.position + new Vector3(5, 3, 5);
            Destroy(current_placement);
            current_placement = Instantiate(new_placement, new_position, transform.rotation);
            gameScript.wood--;
            gameScript.stone--;
        }
    }

    public void Harvest()
    {
        switch (current_placement.name)
        {
            case "Tree(Clone)":
                gameScript.wood++;
                break;
            case "Rock(Clone)":
                gameScript.stone++;
                break;
        }
        Destroy(current_placement);
    }

    public void Upgrade()
    {
        //Destroy(current_placement);
    }
}
