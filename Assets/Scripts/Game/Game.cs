using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField] private Board board;
    [SerializeField] private Transform cameraHolder;

    public static Team _currTeam = Team.Light;
    public GameSettings.PlayerType currP = GameSettings.Player1;
    private void Start()
    {
        board.SetTurn(Team.Light);

    }

    public void EndTurn()
    {
        switch (_currTeam)
        {
            case Team.Dark:
                _currTeam = Team.Light;
                currP = GameSettings.Player1;
                break;
            case Team.Light:
                _currTeam = Team.Dark;
                currP = GameSettings.Player2;
                break;
            default:
                break;
        }

        board.SetTurn(_currTeam);
        var rot = Quaternion.AngleAxis(_currTeam == Team.Light ? 0 : 180, Vector3.up);
        StartCoroutine(RotateCam(rot));

    }

    private IEnumerator RotateCam(Quaternion rot)
    {
        var timeToStart = Time.time;
        while (Quaternion.Angle(cameraHolder.rotation, rot) > 0.05f)
        {
            cameraHolder.rotation = Quaternion.Slerp(cameraHolder.rotation, rot, 0.1f);
            yield return null;
        }
        yield return null;
    }


    public enum Team
    {
        Light, Dark
    }

}
