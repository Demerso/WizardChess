using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown player1Dropdown;
    [SerializeField] private TMP_Dropdown player2Dropdown;


    public void StartGame()
    {
        GameSettings.Player1 = GetPlayerType(player1Dropdown.value);
        GameSettings.Player2 = GetPlayerType(player2Dropdown.value);
        SceneManager.LoadScene("GameScene");
    }

    private static GameSettings.PlayerType GetPlayerType(int value)
    {
        switch (value)
        {
            case 0:
                return GameSettings.PlayerType.Player;
            case 1:
                return GameSettings.PlayerType.EasyAI;
            case 2:
                return GameSettings.PlayerType.HardAI;
        }

        return GameSettings.PlayerType.Player;
    }

}
