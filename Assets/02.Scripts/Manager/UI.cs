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
    public TitleLayout title;
    public Layout tutorial;
    public Layout village;
    public Layout main;
    public Layout weekly;
    //public SceneLoader loading;

    private LayoutState State;

    private void Awake()
    {
        title = GetComponentInChildren<TitleLayout>(true);
        //title.ActiveTrue();
        //loading.GetComponentInChildren<SceneLoader>(true);
        //tutorial = GetComponentInChildren<Layout>();
        //village = GetComponentInChildren<Layout>();
        //main = GetComponentInChildren<Layout>();
        //weekly = GetComponentInChildren<Layout>();
        //State = LayoutState.Title;
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
