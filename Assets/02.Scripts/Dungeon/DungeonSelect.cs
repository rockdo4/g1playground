using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DungeonSelect : MonoBehaviour
{
    private DayOfWeek tomorrow;
    private TextMeshProUGUI tomorrowtime;
    StringBuilder remainingtime = new StringBuilder();

    // Start is called before the first frame update
    private void OnEnable()
    {
        tomorrow = (DayOfWeek)((int)DateTime.Now.DayOfWeek + 1);
        transform.Find(tomorrow.ToString()).Find("Lock").Find("Time").gameObject.SetActive(true);
        tomorrowtime = transform.Find(tomorrow.ToString()).Find("Lock").Find("Time").GetComponentInChildren<TextMeshProUGUI>();
        DungeionEnter();
    }

    private void Update()
    {
        if (DateTime.Now.Minute==0&&DateTime.Now.DayOfWeek == tomorrow)
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
        tomorrowtime.text=remainingtime.ToString();


    }

    private void DungeionEnter()
    {
        gameObject.SetActive(true);
        switch (DateTime.Now.DayOfWeek)
        {
            case DayOfWeek.Monday:
                transform.Find("Monday").Find("Lock").gameObject.SetActive(false);
                // dungeonTable = DataTableMgr.Load(dungeonTable, "Resources\\DataTables\\Mon");
                break;
            case DayOfWeek.Tuesday:
                transform.Find("Tuesday").Find("Lock").gameObject.SetActive(false);
                break;
            case DayOfWeek.Wednesday:
                transform.Find("Wednesday").Find("Lock").gameObject.SetActive(false); ;
                break;
            case DayOfWeek.Thursday:
                transform.Find("Thursday").Find("Lock").gameObject.SetActive(false); ;
                break;
            case DayOfWeek.Friday:
                transform.Find("Friday").Find("Lock").gameObject.SetActive(false); ;
                break;
            default:
                transform.Find("Monday").Find("Lock").gameObject.SetActive(false);
                transform.Find("Tuesday").Find("Lock").gameObject.SetActive(false);
                transform.Find("Thursday").Find("Lock").gameObject.SetActive(false);
                transform.Find("Friday").Find("Lock").gameObject.SetActive(false);
                transform.Find("Wednesday").Find("Lock").gameObject.SetActive(false);
                break;

        }
    }
}
