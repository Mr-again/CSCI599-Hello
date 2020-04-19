using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Drawing;
using System.IO;

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
    public Button[] likeButtons;
    public Image[] communityMapImages;
    public GameObject slotPanel;
    public GameObject slotPanelCard;
    public GameObject slotMapImage;

    public GameObject communityPanel;
    public GameObject mySlotPanel;
    public RectTransform mySlotPanelTransform;
    public GridLayoutGroup mySlotPanelGrid;

    public Button AddNewButton;
    private int page_size = 4;
    private int page = 1;
    int maker_id = 2;
    static int deleted_item_size = 0;
    LocalSlot ls;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        downloadMap(0);
        //Debug.Log("Page:" + page.ToString());
        pageUp.onClick.AddListener(() => { OnClickPageUp(); });
        pageDown.onClick.AddListener(() => { OnClickPageDown(); });
        communityButton.onClick.AddListener(() => { OnClickCommunity(); });
        slotButton.onClick.AddListener(() => { OnClickSlot(); });
        AddNewButton.onClick.AddListener(() => { OnClickAddNew(); });
        ls = new LocalSlot();
        deleted_item_size = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickAddNew()
    {
        gameController.gameplay_enetrance = 2;
        SceneManager.LoadScene("MapDesign");
    }

    public void OnClickSlotPanelToMapDesign(LevelData ld)
    {
        gameController.gameplay_enetrance = 2;
        gameController.target_map_json = ld.MapData;
        SceneManager.LoadScene("MapDesign");
    }
    public void OnClickCommunity()
    {
        downloadMap(0);
        communityPanel.SetActive(true);
        mySlotPanel.SetActive(false);
    }

    public void OnClickSlot()
    {
        communityPanel.SetActive(false);
        mySlotPanel.SetActive(true);
        Image[] tmp_img = slotPanel.GetComponentsInChildren<Image>();
        for(int i = 0; i < tmp_img.Length; i++)
        {
            if(tmp_img[i].name == "SlotPanelCard(Clone)")
            {
                foreach (Transform child in tmp_img[i].transform)
                {
                    Destroy(child.gameObject);
                }
                Destroy(tmp_img[i].gameObject);
            }
        }
        getCurrentUserMap();
        
    }

    public void OnClickCommunityPannels(LevelData ld)
    {
        gameController.gameplay_enetrance = 1;
        gameController.target_map_json = ld.MapData;
        SceneManager.LoadScene("GamePlay");
        
    }
    public void OnClickPageUp()
    {
        downloadMap(1);
        //Debug.Log("Page:" + page.ToString());
    }

    public void OnClickPageDown()
    {
        downloadMap(-1);
        //Debug.Log("Page:" + page.ToString());
    }

    public void OnClickRelease(LevelData ld, int index)
    {
        Button cur = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        
        GameObject curPanel = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        uploadMap(ld.MapData, ld.OneStarStep, ld.TwoStarStep, ld.ThreeStarStep, index, curPanel);
        Button btnPanel = curPanel.GetComponentsInChildren<Button>()[0];
        btnPanel.enabled = false;
        cur.GetComponentInChildren<Text>().text = "Released";
        cur.interactable = false;
        
    }

    public void OnClickDelete(LevelData ld, int index)
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        string title = cur.GetComponentsInChildren<Text>()[0].text;
        if (title == "Local Map")
        {
            deleteMap(-1, index);
        }
        else
        {
            deleteMap(int.Parse(title.Split(' ')[1]), index);
        }
        
        cur.SetActive(false);
    }
    
    public void OnClickLikeButton()
    {
        GameObject cur = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        int level_id = int.Parse(cur.GetComponentsInChildren<Text>()[0].text.Split(' ')[1]);
        thumbUpMap(level_id);
        int cur_thumb = int.Parse(cur.GetComponentsInChildren<Text>()[1].text);
        cur.GetComponentsInChildren<Text>()[1].text = (cur_thumb + 1).ToString();
    }

    public async void thumbUpMap(int level_id)
    {
        HttpClient client = new HttpClient();
        var responseString = await client.GetStringAsync(
            "http://35.238.86.31/level?type=3&update=1&try=0&pass=0&thumb=1&level_id=" + level_id.ToString());
    }

    // public static PreviewManager Instance;

    public async void downloadMap(int offset)
    {
        page += offset;
        HttpClient client = new HttpClient();
        var responseString = await client.GetStringAsync("http://35.238.86.31/level?type=2");
        LevelData[] ldArr = convertToJson(responseString, page_size, page);
        if (ldArr == null) return;
        int i = 0;
        while(i < ldArr.Length)
        {
            int index = i;
            communityPanels[i].GetComponentsInChildren<Text>()[0].text = "Map " + ldArr[i].levelId.ToString();
            communityPanels[i].GetComponentsInChildren<Text>()[1].text = ldArr[i].ThumbNum.ToString();
            communityPanelsButton[i].onClick.AddListener(delegate() { OnClickCommunityPannels(ldArr[index]); });
            likeButtons[i].onClick.AddListener(() => { OnClickLikeButton(); });
            communityPanels[i].gameObject.SetActive(true);
            // communityPanels[i].GetComponentInChildren<Image>().sprite

            // TODO: Give out a image of level: communityPanels[i].GetComponentsInChildren<Image>()[0];
            i++;
        }
        while (i < 4)
        {
            communityPanels[i].gameObject.SetActive(false);
            // communityPanels[i].GetComponentInChildren<Image>().sprite

            // TODO: Give out a image of level: communityPanels[i].GetComponentsInChildren<Image>()[0];
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
        int maxPage = (len - 1) / page_size + 1;
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


        // TODO: Get data from local playpref
        //LevelData ld = new LevelData("testMap", maker_id, 20, 40, 60);
        //ls.AddSlotMap(ld);
        //ld = new LevelData("testMap2", maker_id, 20, 40, 60);
        //ls.AddSlotMap(ld);
        LevelData[] ldArr = ls.GetAllSlotMap();
        int ldArrLen = ldArr.Length;
        Debug.Log(ldArrLen);
        for(int i = 0; i < ldArrLen; i++)
        {
            int index = i;
            GameObject new_slotPanelCard = Instantiate(slotPanelCard, transform.position, Quaternion.identity);

            if (ldArr[i].levelId == -1)
            {
                new_slotPanelCard.GetComponentInChildren<Text>().text = "Local Map";
                new_slotPanelCard.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate () { OnClickRelease(ldArr[index], index); });
                new_slotPanelCard.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate () { OnClickSlotPanelToMapDesign(ldArr[index]); });

            }
            else
            {
                new_slotPanelCard.GetComponentInChildren<Text>().text = "Map: " + ldArr[index].levelId.ToString();
                Button cur = new_slotPanelCard.GetComponentsInChildren<Button>()[1];
                cur.onClick.AddListener(delegate () { OnClickRelease(ldArr[index], index); });
                cur.GetComponentInChildren<Text>().text = "Released";
                cur.interactable = false;
                new_slotPanelCard.GetComponentsInChildren<Button>()[0].enabled = false;
            }
            
            new_slotPanelCard.GetComponentsInChildren<Button>()[2].onClick.AddListener(delegate () { OnClickDelete(ldArr[index], index); });
            new_slotPanelCard.transform.SetParent(mySlotPanelTransform);
            new_slotPanelCard.SetActive(true);

        }
    }

    public async void uploadMap(string mapData, int one_star_step, int two_star_step, int three_star_step, int index, GameObject curPanel)
    {
        HttpClient client = new HttpClient();
        var responseString = await client.GetStringAsync(
            "http://35.238.86.31/level?type=3&try_num=0&pass_num=0&thumb_num=0" +
            "&one_star_step=" + one_star_step.ToString() +
            "&two_star_step=" + two_star_step.ToString() +
            "&three_star_step=" + three_star_step.ToString() +
            "&id_of_maker=" + maker_id.ToString() + "&map_data=" + mapData);
        LevelData ld = JsonConvert.DeserializeObject<LevelData>(responseString);
        ls.UpdateSlotMap(index, ld);
        curPanel.GetComponentInChildren<Text>().text = "Map: " + ld.levelId.ToString();

    }

    //public int level_id;


    public async void deleteMap(int level_id, int index)
    {
        if(level_id != -1)
        {
            HttpClient client = new HttpClient();
            await client.GetStringAsync("http://35.238.86.31/level?type=3&delete=1&level_id=" + level_id.ToString());
        }
        ls.DeleteSlotMap(index - deleted_item_size);
        deleted_item_size += 1;
    }
}
