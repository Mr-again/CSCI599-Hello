using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PageControl : MonoBehaviour
{
    GameController gameController;
    public Button pageUp;
    public Button pageDown;
    public Button communityButton;
    public Button slotButton;
    public Image[] communityPanels;
    public Button[] communityPanelsButton;
    public Image[] likeImages;
    public Image[] communityMapImages;
    public GameObject slotPanel;
    public GameObject slotPanelCard;
    public GameObject slotMapImage;

    public GameObject communityPanel;
    public GameObject mySlotPanel;
    public RectTransform mySlotPanelTransform;
    public GridLayoutGroup mySlotPanelGrid;

    private int page_size = 4;
    private int page = 1;
    int maker_id = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        downloadMap(0);
        Debug.Log("Page:" + page.ToString());
        pageUp.onClick.AddListener(() => { OnClickPageUp(); });
        pageDown.onClick.AddListener(() => { OnClickPageDown(); });
        communityButton.onClick.AddListener(() => { OnClickCommunity(); });
        slotButton.onClick.AddListener(() => { OnClickSlot(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCommunity()
    {
        communityPanel.SetActive(true);
        mySlotPanel.SetActive(false);
    }

    public void OnClickSlot()
    {
        communityPanel.SetActive(false);
        mySlotPanel.SetActive(true);
        getCurrentUserMap();
    }

    public void OnClickCommunityPannels(LevelData ld)
    {
        Debug.Log(gameController);
        gameController.gameplay_enetrance = 1;
        gameController.target_map_json = ld.MapData;
        SceneManager.LoadScene("GamePlay");
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

    public void OnClickRelease(LevelData ld)
    {
        Button cur = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        cur.GetComponentInChildren<Text>().text = "Released";
        cur.interactable = false;
        //uploadMap(mapData);
        uploadMap(ld.MapData);
    }

    public void OnClickDelete(LevelData ld)
    {
        deleteMap(ld.levelId);
        GameObject cur = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        cur.SetActive(false);
    }

    public async void downloadMap(int offset)
    {
        page += offset;
        HttpClient client = new HttpClient();
        var responseString = await client.GetStringAsync(
            "http://35.238.86.31/level?type=2");
        LevelData[] ldArr = convertToJson(responseString, page_size, page);
        if (ldArr == null) return;
        int i = 0;
        while(i < ldArr.Length)
        {
            int index = i;
            communityPanels[i].GetComponentsInChildren<Text>()[0].text = "Map " + ldArr[i].levelId.ToString();
            communityPanels[i].GetComponentsInChildren<Text>()[1].text = ldArr[i].ThumbNum.ToString();
            communityPanelsButton[i].onClick.AddListener(delegate() { OnClickCommunityPannels(ldArr[index]); });
            // TODO: Give out a image of level: communityPanels[i].GetComponentsInChildren<Image>()[0];
            i++;
        }
        while(i < page_size)
        {
            communityPanels[i].GetComponentsInChildren<Text>()[0].text = "Empty";
            communityPanels[i].GetComponentsInChildren<Text>()[1].text = "0";
            communityPanelsButton[i].interactable = false;
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

    public void getCurrentUserMap()
    {
        //HttpClient client = new HttpClient();
        //var responseString = await client.GetStringAsync(
        //    "http://35.238.86.31/level?type=0&maker_id=" + maker_id.ToString());
        //LevelData[] ldArr = JsonConvert.DeserializeObject<LevelData[]>(responseString);


        // TODO: Get data from local files
        LevelData[] ldArr = new LevelData[0];
        int ldArrLen = ldArr.Length;
        Debug.Log(ldArrLen);
        for(int i = 0; i < ldArrLen; i++)
        {
            int index = i;
            GameObject new_slotPanelCard = Instantiate(slotPanelCard, transform.position, Quaternion.identity);
            new_slotPanelCard.GetComponentInChildren<Text>().text = "Map: " + ldArr[index].levelId.ToString();
            new_slotPanelCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate() { OnClickRelease(ldArr[index]); });
            new_slotPanelCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate () { OnClickDelete(ldArr[index]); });
            new_slotPanelCard.transform.SetParent(mySlotPanelTransform);
            new_slotPanelCard.SetActive(true);

        }
    }

    public async void uploadMap(string mapData)
    {
        HttpClient client = new HttpClient();
        var values = new Dictionary<string, string>();
        var content = new FormUrlEncodedContent(values);
        await client.PostAsync(
            "http://35.238.86.31/level?try_num=0&pass_num=0&thumb_num=0&id_of_maker="
            + maker_id.ToString() + "&map_data=" + mapData, content);
    }

    //public int level_id;


    public async void deleteMap(int level_id)
    {
        HttpClient client = new HttpClient();
        var values = new Dictionary<string, string>();
        var content = new FormUrlEncodedContent(values);
        await client.PostAsync(
            "http://35.238.86.31/level?delete=1&level_id=" + level_id.ToString(), content);
    }
}
