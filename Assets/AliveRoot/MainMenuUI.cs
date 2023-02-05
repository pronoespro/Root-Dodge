using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public void LoadFirstLevel()
    {
        NavigationManager.singleton.NavigateToScene("SampleRoots");
    }
}
