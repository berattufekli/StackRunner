using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider slider;

    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject prefab7;
    public GameObject finish;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelText2;
    public GameObject MenuPanel;
    public GameObject GamePanel;
    public GameObject FinishPanel;
    public GameObject Bonus;
    public TextMeshProUGUI levelStatus;

    private List<GameObject> prefabs = new List<GameObject>();
    private List<GameObject> createdObjects = new List<GameObject>();
    


    private float newPrefabPosZ = 10f;
    private float newPrefabPosX = -0.5f;

    private float totalWay = 5;
    private float currentWay = 0;
    private bool setLevel = false;
    private int wayToBeCreated = 10;
    private bool levelIsBegin = false;
    private int bonusPos;
    private void Start()
    {

        if((PlayerPrefs.GetInt("Level") / 5) % 2 == 0)
        {
            wayToBeCreated += PlayerPrefs.GetInt("Level") / 5;
            PlayerPrefs.SetInt("wayToBeCreated", wayToBeCreated);
        }
        else
        {
            wayToBeCreated = PlayerPrefs.GetInt("wayToBeCreated");

        }

        bonusPos = wayToBeCreated / 2 - 1;

        levelText.SetText(PlayerPrefs.GetInt("Level").ToString());
        levelText2.SetText(PlayerPrefs.GetInt("Level").ToString());
        prefabs.Add(prefab3);
        prefabs.Add(prefab4);
        prefabs.Add(prefab5);
        prefabs.Add(prefab6);
        prefabs.Add(prefab7);

        for(int i = 0; i<=wayToBeCreated; i++)
        {
            int random = Random.Range(0, 5);
            GameObject go = prefabs[random];
            createdObjects.Add(go);
            totalWay += go.transform.localScale.z;


            //adding with rotation 90
            if (i % 2 == 0)
            {
                if (i == wayToBeCreated)
                {
                    Instantiate(finish, new Vector3(newPrefabPosX - 0.5f, 0, newPrefabPosZ + 0.5f), Quaternion.identity);
                }
                else
                {
                    newPrefabPosX += -go.transform.localScale.z / 2f;
                    Instantiate(go, new Vector3(newPrefabPosX, 0, newPrefabPosZ - 0.5f), Quaternion.Euler(new Vector3(0, 90, 0)));
                    newPrefabPosX += -go.transform.localScale.z / 2f;
                }
            }

            //adding with rotation 0
            else
            {
                //adding bonus
                if (i == bonusPos)
                {
                    Debug.LogWarning("asasf");
                    Instantiate(Bonus, new Vector3(newPrefabPosX - 0.5f, 0.55f, newPrefabPosZ - 1f + go.transform.localScale.z / 2), Quaternion.identity);
                }
                newPrefabPosZ += go.transform.localScale.z / 2;
                Instantiate(go, new Vector3(newPrefabPosX - 0.5f, 0, newPrefabPosZ - 1f), Quaternion.identity);
                newPrefabPosZ += go.transform.localScale.z / 2;
            }
        }
    }

    private void Update()
    {
        if(transform.localPosition.y < -3)
        {
            StartCoroutine(TryAgain());
        }

        if(Input.touchCount > 0 && !levelIsBegin)
        {
            StartCoroutine(StartGame());
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(other.gameObject, 2);
        }

        if (other.CompareTag("Slice"))
        {
            other.gameObject.AddComponent<Rigidbody>();
            Destroy(other.gameObject, 2);
            currentWay += 1;

            slider.value = currentWay / totalWay;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            slider.value = 1;
            GetComponent<PlayerMovement>().enabled = false;
            if (!setLevel)
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                setLevel = true;
            }
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator StartGame()
    {
        MenuPanel.GetComponent<Animator>().SetTrigger("FadeIn");
        levelIsBegin = true;
        MenuPanel.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerMovement>().enabled = true;
        GamePanel.SetActive(true);
        GamePanel.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    IEnumerator NextLevel()
    {
        GamePanel.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        GamePanel.SetActive(false);
        FinishPanel.SetActive(true);
        FinishPanel.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator TryAgain()
    {
        GamePanel.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        GamePanel.SetActive(false);
        levelStatus.SetText("Level Failed");
        FinishPanel.SetActive(true);
        FinishPanel.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }
}
