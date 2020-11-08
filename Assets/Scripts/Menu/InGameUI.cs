using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;

    public bool open;

    private void Start()
    {
        open = false;
        winPanel.SetActive(false);
    }

    public void OpenWinPanel(string winingTeam)
    {
        open = true;
        winPanel.GetComponentInChildren<TMP_Text>().text = winingTeam + " has won!";
        winPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
