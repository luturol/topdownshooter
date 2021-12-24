using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI DeathText;

    private GameController gameController;

    private void Start()
    {
        gameController = GameController.Instance;

        winText.text = "Wins: " + gameController.GetWin();
        DeathText.text = "Deaths: " + gameController.GetDeaths();
    }

    public void OnPlayButtonClick()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
