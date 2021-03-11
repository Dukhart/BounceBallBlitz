using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    GameObject player;
    [SerializeField] GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }
    // bind event listeners
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += ProccessPlayerDeath;
    }
    // unbind event listeners
    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= ProccessPlayerDeath;
    }
    void CallGameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
        gameOverPanel.SetActive(true);
    }
    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void ProccessPlayerDeath(int playerNum)
    {
        CallGameOver();
    }
}
