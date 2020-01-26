using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public GameObject stoneText;
    public GameObject woodText;
    public GameObject waveText;
    public GameObject healthText;
    public GameObject timeText;
    private Game gameScript;
    // Start is called before the first frame update
    void Start()
    {
        GameObject game = GameObject.Find("Game");
        gameScript = game.GetComponent<Game>();
    }

    void UpdateHUD()
    {
        // Stone
        TextMeshProUGUI stoneTextMesh = stoneText.GetComponent<TextMeshProUGUI>();
        stoneTextMesh.text = "Stone: " + gameScript.stone.ToString();
        // Wood
        TextMeshProUGUI woodTextMesh = woodText.GetComponent<TextMeshProUGUI>();
        woodTextMesh.text = "Wood: " + gameScript.wood.ToString();
        // Health
        TextMeshProUGUI healthTextMesh = healthText.GetComponent<TextMeshProUGUI>();
        healthTextMesh.text = "Health: " + gameScript.health.ToString();
        // Time
        TextMeshProUGUI timeTextMesh = timeText.GetComponent<TextMeshProUGUI>();
        timeTextMesh.text = "Time: " + gameScript.time.ToString();
        // Wave
        TextMeshProUGUI waveTextMesh = waveText.GetComponent<TextMeshProUGUI>();
        waveTextMesh.text = "Wave: " + gameScript.wave.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateHUD();
    }
}
