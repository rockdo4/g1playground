using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonSelect : PopupUI
{
    private DayOfWeek tomorrow;
    private TextMeshProUGUI tomorrowtime;
    StringBuilder remainingtime = new StringBuilder();
    [SerializeField]
    private List<GameObject> days;
   
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
        {
            tomorrow = (DayOfWeek)((int)DateTime.Now.DayOfWeek + 1);

            for (int i = 0; i < days.Count; i++)
            {
                if (days[i].name == tomorrow.ToString())
                {
                    days[i].transform.Find("Lock").Find("Time").gameObject.SetActive(true);
                    tomorrowtime = days[i].transform.Find("Lock").Find("Time").GetComponentInChildren<TextMeshProUGUI>();
                }
                else
                {
                    days[i].transform.Find("Lock").Find("Time").gameObject.SetActive(false);
                }
            }

        }

        DungeionEnter();
    }

    private void Update()
    {
        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            return;
        if (DateTime.Now.Minute == 0 && DateTime.Now.DayOfWeek == tomorrow)
        {
            transform.Find(((DayOfWeek)((int)DateTime.Now.DayOfWeek - 1)).ToString()).Find("Lock").gameObject.SetActive(true);
            transform.Find(tomorrow.ToString()).Find("Lock").gameObject.SetActive(false);
            tomorrow = (DayOfWeek)((int)DateTime.Now.DayOfWeek + 1);
            transform.Find(tomorrow.ToString()).Find("Lock").Find("Time").gameObject.SetActive(true);
            tomorrowtime = transform.Find(tomorrow.ToString()).Find("Lock").Find("Time").GetComponentInChildren<TextMeshProUGUI>();
        }
        remainingtime.Clear();
        remainingtime.Append(DateTime.MaxValue.Subtract(DateTime.Now).Hours.ToString());
        remainingtime.Append(" : ");
        remainingtime.Append(DateTime.MaxValue.Subtract(DateTime.Now).Minutes.ToString());
        remainingtime.Append(" : ");
        remainingtime.Append(DateTime.MaxValue.Subtract(DateTime.Now).Seconds.ToString());
        if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
        {
            tomorrowtime.text = remainingtime.ToString();
        }
    }

    private void DungeionEnter()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < days.Count; i++)
        {
            if (DateTime.Now.DayOfWeek.ToString() == days[i].name)
            {
                days[i].transform.Find("Lock").gameObject.SetActive(false);
                break;
            }
            else if (i == days.Count - 1)
            {
                days[0].transform.Find("Lock").gameObject.SetActive(false);
                days[1].transform.Find("Lock").gameObject.SetActive(false);
                days[2].transform.Find("Lock").gameObject.SetActive(false);
                days[3].transform.Find("Lock").gameObject.SetActive(false);
                days[4].transform.Find("Lock").gameObject.SetActive(false);
                break;
            }
        }


    }
}
