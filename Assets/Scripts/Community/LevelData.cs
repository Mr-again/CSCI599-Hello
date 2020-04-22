using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LevelData
{
    public int levelId;
    public int TryNum;
    public int PassNum;
    public int ThumbNum;
    public string IdOfMaker;
    public string MapData;
    public int OneStarStep;
    public int TwoStarStep;
    public int ThreeStarStep;
    public string LevelName;
    public LevelData(string mapData, string maker_id, int one_star_step, int two_star_step, int three_star_step, string level_name)
    {
        this.MapData = mapData;
        this.IdOfMaker = maker_id;
        this.levelId = -1;
        this.TryNum = 0;
        this.PassNum = 0;
        this.ThumbNum = 0;
        this.OneStarStep = one_star_step;
        this.TwoStarStep = two_star_step;
        this.ThreeStarStep = three_star_step;
        this.LevelName = level_name;
    }
}

public class LocalSlot
{
    public LevelData[] ldArr;

    public LocalSlot()
    {
        if (PlayerPrefs.HasKey("slot"))
        {
            string slotStr = PlayerPrefs.GetString("slot");
            ldArr = JsonConvert.DeserializeObject<LevelData[]>(slotStr);
        }
        else
        {
            ldArr = JsonConvert.DeserializeObject<LevelData[]>("[]");
        }
    }

    public int AddSlotMap(LevelData ld)
    {
        List<LevelData> ldList = new List<LevelData>(ldArr);
        ldList.Add(ld);
        ldArr = ldList.ToArray();
        PlayerPrefs.SetString("slot", JsonConvert.SerializeObject(ldArr));
        return ldArr.Length - 1;
    }

    public LevelData[] GetAllSlotMap()
    {
        return ldArr;
    }

    public void UpdateSlotMap(int index, LevelData ld)
    {
        List<LevelData> ldList = new List<LevelData>(ldArr);
        ldList[index] = ld;
        ldArr = ldList.ToArray();
        PlayerPrefs.SetString("slot", JsonConvert.SerializeObject(ldArr));
    }

    public void DeleteSlotMap(int index)
    {
        List<LevelData> ldList = new List<LevelData>(ldArr);
        ldList.RemoveAt(index);
        ldArr = ldList.ToArray();
        PlayerPrefs.SetString("slot", JsonConvert.SerializeObject(ldArr));
    }
}

