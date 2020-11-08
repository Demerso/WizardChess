using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField] private Board board;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private InGameUI ui;

    private AI _hardAI;
    private AI _easyAI;

    public bool UIIsOpen => ui.open;
    public Team currTeam = Team.Light;
    private void Start()
    {
        _hardAI = new Hard(this, board);
        _easyAI = new Rando(this, board);
        NextTurn();
    }

    private void NextTurn()
    {
        switch (currTeam == Team.Light ? GameSettings.Player1 : GameSettings.Player2)
        {
            case GameSettings.PlayerType.Player:
                board.SetTurn(currTeam);
                break;
            case GameSettings.PlayerType.EasyAI:
                StartCoroutine(_easyAI.SelectMove(move => _easyAI.Move(move)));
                break;
            case GameSettings.PlayerType.HardAI:
                StartCoroutine(_hardAI.SelectMove(move => _hardAI.Move(move)));
                break;
            default:
                break;
        }
    }

    public void EndTurn()
    {
        switch (currTeam)
        {
            case Team.Dark:
                currTeam = Team.Light;
                break;
            case Team.Light:
                currTeam = Team.Dark;
                break;
            default:
                break;
        }

        var rot = Quaternion.AngleAxis(currTeam == Team.Light ? 0 : 180, Vector3.up);
        StartCoroutine(RotateCam(rot));

    }

    private IEnumerator RotateCam(Quaternion rot)
    {
        while (Quaternion.Angle(cameraHolder.rotation, rot) > 0.05f)
        {
            cameraHolder.rotation = Quaternion.Slerp(cameraHolder.rotation, rot, 0.1f);
            yield return null;
        }
        NextTurn();
    }


    public void KingDied(Team team)
    {
        switch (team)
        {
            case Team.Light:
                ui.OpenWinPanel("Player 2");
                break;
            case Team.Dark:
                ui.OpenWinPanel("Player 1");
                break;
            default:
                break;
        }
    }

    public enum Team
    {
        Light, Dark
    }

}
