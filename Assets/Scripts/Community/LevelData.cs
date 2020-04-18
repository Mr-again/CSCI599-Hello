public class LevelData
{
    public int levelId;
    public int TryNum;
    public int PassNum;
    public int ThumbNum;
    public int IdOfMaker;
    public string MapData;
    public int OneStarStep;
    public int TwoStarStep;
    public int ThreeStarStep;
    public LevelData(string mapData, int maker_id)
    {
        this.MapData = mapData;
        this.IdOfMaker = maker_id;
    }
}

