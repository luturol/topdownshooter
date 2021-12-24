using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private int wins;
    [SerializeField] private int deaths;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddWin() => wins += 1;

    public void AddDeath() => deaths += 1;

    public int GetWin() => wins;

    public int GetDeaths() => deaths;

    public void SendToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
