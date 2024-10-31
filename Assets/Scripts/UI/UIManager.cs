using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Controls the game UI.
    /// </summary>


    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private GameObject lvlCompleteScreen;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    
    [SerializeField] private GameObject player;
    private PlayerRespawn playerRespawn;
    private PlayerHealth health;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        lvlCompleteScreen.SetActive(false);
        playerRespawn = gameOverScreen.GetComponent<PlayerRespawn>();
        health = gameOverScreen.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Pauses and unpauses game
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    #region Game Over
    //Turns on the game over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    //Reset game status to last checkpoint
    public void ReloadCheckpoint()
    {
        gameOverScreen.SetActive(false);
        player.GetComponent<PlayerRespawn>().CheckRespawn();  
    }

    //Reload entire scene
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Load Main Menu Scene
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //If playing in editor, quits to editor. If in build, quits application
    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    #endregion

    #region Pause

    //Activates pause menu
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        Time.timeScale = System.Convert.ToInt32(!status);
        
    }

    //Lowers sound volume by 20 percent
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    //Lowers music volume by 20 percent
    public void MusicVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    #endregion

    //Activates the level complete screen
    #region Level Completed
    public void LevelCompleted()
    {
        lvlCompleteScreen.SetActive(true);
    }

    //Loads the next scene
    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    #endregion
}
