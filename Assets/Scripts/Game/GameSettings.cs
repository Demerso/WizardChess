public static class GameSettings
{

    public static PlayerType Player1 = PlayerType.Player;
    public static PlayerType Player2 = PlayerType.HardAI;

    public enum PlayerType
    {
        Player, EasyAI, HardAI
    }
}
