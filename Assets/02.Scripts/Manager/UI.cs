using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayoutState
{
    None,
    Title,
    Tutorial,
    Village,
    Main,
    Weekly,
}
public class UI : MonoBehaviour
{
    public Layout title;
    public Layout tutorial;
    public Layout village;
    public Layout main;
    public Layout weekly;

    private LayoutState State;

    private void Awake()
    {
        title = GetComponent<Layout>();
        title.ActiveTrue();
        tutorial = GetComponent<Layout>();
        tutorial.ActiveFalse();
        village = GetComponent<Layout>();
        village.ActiveFalse();
        main = GetComponent<Layout>();
        main.ActiveFalse();
        weekly = GetComponent<Layout>();
        weekly.ActiveFalse();
        State = LayoutState.Title;
    }

    public void SetState(LayoutState state)
    {
        switch (State)
        {
            case LayoutState.Title:
                title.ActiveFalse();
                break;
            case LayoutState.Tutorial:
                tutorial.ActiveFalse();
                break;
            case LayoutState.Village:
                village.ActiveFalse();
                break;
            case LayoutState.Main:
                main.ActiveFalse();
                break;
            case LayoutState.Weekly:
                weekly.ActiveFalse();
                break;
        }

        State = state;

        switch (state)
        {
            case LayoutState.Title:
                title.ActiveTrue();
                break;
            case LayoutState.Tutorial:
                tutorial.ActiveTrue();
                break;
            case LayoutState.Village:
                village.ActiveTrue();
                break;
            case LayoutState.Main:
                main.ActiveTrue();
                break;
            case LayoutState.Weekly:
                weekly.ActiveTrue();
                break;
        }
    }
}
