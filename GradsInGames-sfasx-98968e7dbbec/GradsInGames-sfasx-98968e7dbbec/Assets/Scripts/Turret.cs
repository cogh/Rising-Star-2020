using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    GameObject target;
    public GameObject projectile;
    float max_range;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        max_range = 30.0f;
    }

    GameObject FindNearestTarget()
    {
        List<GameObject> enemy_list = GameObject.Find("Game").GetComponent<Game>().enemy_list;
        if (enemy_list.Count > 0)
        {
            GameObject nearest_enemy = enemy_list[0];
            float shortest_distance = Vector3.Distance(nearest_enemy.transform.position, transform.position);
            foreach (GameObject enemy in enemy_list)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) < shortest_distance) {
                    nearest_enemy = enemy;
                    shortest_distance = Vector3.Distance(nearest_enemy.transform.position, transform.position);
                }
            }
            if (shortest_distance < max_range)
            {
                return nearest_enemy;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = FindNearestTarget();
        if (target != null)
        {
            Transform target_transform = target.transform;
            Quaternion targetRotation = Quaternion.LookRotation(target_transform.position - transform.position);
            transform.rotation = targetRotation;
            if (Time.frameCount % 60 == 0)
            {
                GameObject new_shot = Instantiate(projectile, transform.position, transform.rotation);
            }
        }
    }
}
