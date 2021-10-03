using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private static Engine instance;
    public static Engine Instance{
        get{
            if(instance == null){
                instance = FindObjectOfType<Engine>();
            }
            return instance;
        }
    }
    public Player player;
    public Checkpoint currentCheckpoint, majorCheckpoint;
    public Health health;
    public int currentPlayerHealth = 3;
    public bool particlesEnabled, deathEnabled;
    private void Start() {
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Particles"))){
            PlayerPrefs.SetString("Particles", "Enabled");
        }
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Death"))){
            PlayerPrefs.SetString("Death", "Enabled");
        }
        particlesEnabled = PlayerPrefs.GetString("Particles") == "Enabled" ? true : false;
        deathEnabled = PlayerPrefs.GetString("Death") == "Enabled" ? true : false;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            Death();
        }
    }
    public void Restart(){
        player.transform.position = currentCheckpoint.playerSpawnPos.position;
        CameraFollow.Instance.canDie = true;
        DamagePlayer();
    }
    public void Death(){
        currentPlayerHealth = 3;
        player.transform.position = majorCheckpoint.playerSpawnPos.position;
        CameraFollow.Instance.transform.position = majorCheckpoint.cameraPos.transform.position;
        var checkpoints = FindObjectsOfType<Checkpoint>();
        foreach(var c in checkpoints){
            c.triggered = false;
        }
        health.Reset();
    }
    public void DamagePlayer(){
        if(!deathEnabled)return;
        if(currentPlayerHealth > 0)health.DoDamage(currentPlayerHealth - 1);
        currentPlayerHealth--;
        if(currentPlayerHealth <= 0){
            Death();
        }
    }
}
