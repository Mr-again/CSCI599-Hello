using UnityEngine;

namespace Json
{
    public class JsonObject
    {
        public string[] wall_position;
        public string[] stone_position;
        public string[] ice_position;
        public string[] mud_position;
        public string[] pit_position;
        public int[] player_position;
        public string[] box_position;
        public string[] target_position;

        public JsonObject(
            string[] wall_position,
            string[] stone_position,
            string[] ice_position,
            string[] mud_position,
            string[] pit_position,
            int[] player_position,
            string[] box_position,
            string[] target_position)
        {
            this.wall_position = wall_position;
            this.stone_position = stone_position;
            this.ice_position = ice_position;
            this.mud_position = mud_position;
            this.pit_position = pit_position;
            this.player_position = player_position;
            this.box_position = box_position;
            this.target_position = target_position;
        }

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }

        public static JsonObject CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JsonObject>(jsonString);
        }
    }
}


