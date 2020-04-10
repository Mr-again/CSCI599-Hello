using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class PageControl : MonoBehaviour
{
    public Button pageUp;
    public Button pageDown;
    public Button communityButton;
    public Button slotButton;
    public Image[] panels;
    public Image[] likeImages;
    public Image[] mapImages;

    private int page_size = 4;
    private int page = 1;
    // Start is called before the first frame update
    void Start()
    {
        downloadMap(0);
        Debug.Log("Page:" + page.ToString());
        pageUp.onClick.AddListener(() => { OnClickPageUp(); });
        pageDown.onClick.AddListener(() => { OnClickPageDown(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickPageUp()
    {
        downloadMap(1);
        Debug.Log("Page:" + page.ToString());
    }

    public void OnClickPageDown()
    {
        downloadMap(-1);
        Debug.Log("Page:" + page.ToString());
    }

    public async void downloadMap(int offset)
    {
        page += offset;
        HttpClient client = new HttpClient();
        var responseString = await client.GetStringAsync(
            "http://35.238.86.31/level?type=2");
        //Debug.Log(responseString);
        LevelData[] ldArr = convertToJson(responseString, page_size, page);
        if (ldArr == null) return;
        int i = 0;
        while(i < ldArr.Length)
        {
            panels[i].GetComponentsInChildren<Text>()[0].text = "Map " + (i + 1).ToString();
            panels[i].GetComponentsInChildren<Text>()[1].text = ldArr[i].ThumbNum.ToString();
            //panels[i].GetComponentsInChildren<Image>()[0];
            i++;
        }
        while(i < page_size)
        {
            panels[i].GetComponentsInChildren<Text>()[0].text = "Map Title";
            panels[i].GetComponentsInChildren<Text>()[1].text = "00";
            i++;
        }

    }

    public LevelData[] convertToJson(string responseString, int page_size, int page)
    {
        LevelData[] ldArr = JsonConvert.DeserializeObject<LevelData[]>(responseString);
        int len = ldArr.Length;
        if (page_size == 0)
        {
            return new LevelData[0];
        }
        int maxPage = len / page_size + 1;
        if (page > maxPage) return null;
        if(page == maxPage)
        {
            GameObject.Find("PageUp").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("PageUp").GetComponent<Button>().interactable = true;
        }
        if(page == 1)
        {
            GameObject.Find("PageDown").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("PageDown").GetComponent<Button>().interactable = true;
        }
        int left = (page - 1) * page_size;
        int right = left + page_size - 1;
        if (right >= len) right = len - 1;
        LevelData[] retArr = new LevelData[right - left + 1];
        for (int i = left; i <= right; i++)
        {
            retArr[i - left] = ldArr[i];
        }
        return retArr;
    }

    //public string mapData;
    //public int maker_id;

    //public void OnClickRelease()
    //{
    //    uploadMap();
    //}

    //public async void uploadMap()
    //{
    //    HttpClient client = new HttpClient();
    //    var values = new Dictionary<string, string>();
    //    var content = new FormUrlEncodedContent(values);
    //    await client.PostAsync(
    //        "http://35.238.86.31/level?try_num=0&pass_num=0&thumb_num=0&id_of_maker="
    //        + maker_id.ToString() + "&map_data=" + mapData, content);
    //}

    //public int level_id;

    //public void OnClickDelete()
    //{
    //    deleteMapAsync();
    //}

    //public async void deleteMapAsync()
    //{
    //    HttpClient client = new HttpClient();
    //    var values = new Dictionary<string, string>();
    //    var content = new FormUrlEncodedContent(values);
    //    await client.PostAsync(
    //        "http://35.238.86.31/level?delete=1&level_id=" + level_id.ToString(), content);
    //}
}
