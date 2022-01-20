using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] List<Text> textFields;
    GameObject UI;
    bool waitForGeneration;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("UICanvas");
    }

    // Update is called once per frame
    void Update()
    {
        // Activates / Deactivates the UI
        if (Input.GetKeyDown(KeyCode.H)){

            if (UI.activeSelf)
            {
                UI.SetActive(false);
            }
            else
            {
                UI.SetActive(true);
            }

        }
    }

    public void IncreaseTilesPerRegion()
    {
        string str = textFields[0].text.Trim(new char[] { '[', ']' });
        int new_number = ConvertToInt(str.Split('x')[0]) + 1;
        if(new_number > 9) { new_number = 5; }
        textFields[0].text = "[ " + new_number + "x" + new_number+ " ]";


    }

    public void DecreaseTilesPerRegion()
    {
        string str = textFields[0].text.Trim(new char[] { '[', ']' });
        int new_number = ConvertToInt(str.Split('x')[0]) - 1;
        if (new_number < 5) { new_number = 9; }
        textFields[0].text = "[ " + new_number + "x" + new_number + " ]";
    }

    public void IncreaseLayers()
    {
        string str = textFields[1].text.Trim(new char[] { '[', ']' });
        int new_number = ConvertToInt(str) + 1;
        if (new_number > 7) { new_number = 2; }
        textFields[1].text = "[" + new_number + "]";
    }

    public void DecreaseLayers()
    {
        string str = textFields[1].text.Trim(new char[] { '[', ']' });
        int new_number = ConvertToInt(str) - 1;
        if (new_number < 2) { new_number = 7; }
        textFields[1].text = "[" + new_number + "]";
    }

    public void IncreaseStructureSpawnChance()
    {
        string str = textFields[2].text.Trim(new char[] { '[', ']' });
        int new_number = ConvertToInt(str) + 5;
        if (new_number > 100) { new_number = 0; }
        textFields[2].text = "[ " + new_number + " ]";
    }

    public void DecreaseStructureSpawnChance()
    {
        string str = textFields[2].text.Trim(new char[] { '[', ']' });
        int new_number = ConvertToInt(str) - 5;
        if (new_number < 0) { new_number = 100; }
        textFields[2].text = "[ " + new_number + " ]";
    }

    public void Regenerate()
    {
        List<string> numbers = new List<string>()
        {
            textFields[0].text.Trim(new char[] { '[', ']' }),
            textFields[1].text.Trim(new char[] { '[', ']' }),
            textFields[2].text.Trim(new char[] { '[', ']' })
        };

        int num1 = ConvertToInt(numbers[0].Split('x')[0]);
        int num2 = ConvertToInt(numbers[1]);
        float num3 = ConvertToInt(numbers[2]) / 100f;

        if (!waitForGeneration)
        {
            waitForGeneration = true;
            StartCoroutine(Wait(num1, num2, num3));
        }

    }

    IEnumerator Wait(int num1, int num2, float num3)
    {
        GameObject.Destroy(GameObject.Find("Instance"));
        yield return new WaitForSeconds(.5f);
        GameObject newInstance = new GameObject("Instance");
        GameObject structures = new GameObject("Structures");
        structures.transform.parent = newInstance.transform;


        yield return new WaitForSeconds(.5f);
        this.GetComponent<MapGenerator>().Generate(num1, num2, num3);
        waitForGeneration = false;
    }

    int ConvertToInt(string str)
    {

        int num = -1;
        try
        {
            num = int.Parse(str);
        }
        catch { Debug.Log("Could Not Parse"); }

        return num;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
