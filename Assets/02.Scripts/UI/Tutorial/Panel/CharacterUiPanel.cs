using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUiPanel : PanelUi
{
    private CharacterStatUI characterStatUI;

    private void Awake()
    {
        characterStatUI = GetComponentInChildren<CharacterStatUI>(true);
    }
}
