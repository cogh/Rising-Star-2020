using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Character Character;
    [SerializeField] private GameObject EnemyType;
    [SerializeField] private Canvas Menu;
    [SerializeField] private Canvas Hud;
    [SerializeField] private Transform CharacterStart;

    public List<GameObject> enemy_list;
    public int time { get; set; }
    public int wave { get; set; }
    public int health { get; set; }
    public int wood { get; set; }
    public int stone { get; set; }

    private RaycastHit[] mRaycastHits;
    private Character mCharacter;
    private Environment mMap;
    private EnvironmentTile mEnemyBaseTile;
    private EnvironmentTile mPlayerBaseTile;
    private readonly int NumberOfRaycastHits = 1;
    

    void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
        mMap = GetComponentInChildren<Environment>();
        mCharacter = Instantiate(Character, transform);
        mCharacter.map = mMap;
        mEnemyBaseTile = mMap.Start;
        ShowMenu(true);
        wood = 0;
        stone = 0;
        time = 60;
        wave = 1;
        health = 10;
    }

    void SpawnEnemy()
    {
        // Using specific prefab
        GameObject enemy = Instantiate(EnemyType, transform);
        enemy.GetComponent<Enemy>().map = mMap;
        enemy.GetComponent<Enemy>().CurrentPosition = mMap.EnemySpawn;
        enemy.GetComponent<Enemy>().UseTile(mMap.PlayerSpawn);
        enemy_list.Add(enemy);
    }

    private void Update()
    {
        // Check to see if the player has clicked a tile and if they have, try to find a path to that 
        // tile. If we find a path then the character will move along it to the clicked tile. 
        // Move
        if (Input.GetMouseButtonDown(0))
        {
            Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
            if (hits > 0)
            {
                EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
                if (tile != null)
                {
                    mCharacter.UseTile(tile);
                }
            }
        }
        // Build
        if (Input.GetMouseButtonDown(1))
        {
            Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);
            if (hits > 0)
            {
                EnvironmentTile tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();
                if (tile != null)
                {
                    mCharacter.UseTile(tile);
                }
            }
        }
        // Spawn enemy
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnEnemy();
        }
    }

    public void ShowMenu(bool show)
    {
        if (Menu != null && Hud != null)
        {
            Menu.enabled = show;
            Hud.enabled = !show;

            if( show )
            {
                mCharacter.transform.position = CharacterStart.position;
                mCharacter.transform.rotation = CharacterStart.rotation;
                mMap.CleanUpWorld();
            }
            else
            {
                mCharacter.transform.position = mMap.Start.Position;
                mCharacter.transform.rotation = Quaternion.identity;
                mCharacter.CurrentPosition = mMap.Start;
            }
        }
    }

    public void Generate()
    {
        mMap.GenerateWorld();
    }

    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
