[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public int[] points;

    public PlayerData(int level, int[] points)
    {
        currentLevel = level;

        this.points = points;
    }
}
