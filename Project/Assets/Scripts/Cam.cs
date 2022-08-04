using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Cam : MonoBehaviour
{
    [SerializeField] Image popupMenu;
    public GameObject target;
    float scrollPos;

    void Start()
    {

    }

    public void ClickedOn(GameObject obj)
    {
        if (!target) { target = obj; popupMenu.gameObject.SetActive(true); }
        else if(obj == target)
        {
            target = null;
            popupMenu.gameObject.SetActive(false);
        }
        else
        {
            target = obj;
            popupMenu.gameObject.SetActive(true);
        }
    }

    void LateUpdate()
    {
        var wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput < 0 && scrollPos < 47)
        {
            scrollPos += 2;
        }
        else if(wheelInput > 0 && scrollPos > -47)
        {
            scrollPos -= 2;
        }

        if (target)
        {
            UpdateInfo(target.GetComponent<Animal>());
            transform.position = Vector3.Lerp(transform.position, target.transform.position - new Vector3(0, 0, 10), Time.deltaTime * 2);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(scrollPos, 0, -10), Time.deltaTime * 2);
        }
    }

    void UpdateInfo(Animal a)
    {
        popupMenu.transform.Find("HungerBar").GetComponent<Image>().fillAmount = a.food / 100;
        popupMenu.transform.Find("SpeedHeader").Find("SpeedText").GetComponent<TMP_Text>().text = $"{(a.speed * 10).ToString("0.00")}";
    }
}
