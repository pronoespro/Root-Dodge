using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public static Dictionary<string, int> publicVaribles;

    public static Dictionary<string, float> floatVariablesToSave;
    public static Dictionary<string, string> stringVariablesToSave;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        publicVaribles = new Dictionary<string, int>();

        floatVariablesToSave = new Dictionary<string, float>();
        stringVariablesToSave = new Dictionary<string, string>();
        LoadVariables();

    }

    public float AddOrGetVariable(string name, float var)
    {
        if (!floatVariablesToSave.ContainsKey(name))
        {
            floatVariablesToSave.Add(name, var);
            return var;
        }
        else
        {
            return floatVariablesToSave[name];
        }
    }

    public string AddOrGetVariable(string name, string var)
    {
        if (!stringVariablesToSave.ContainsKey(name))
        {
            stringVariablesToSave.Add(name, var);
            return var;
        }
        else
        {
            return stringVariablesToSave[name];
        }
    }

    public void SaveVariable(string name, float var)
    {
        if (floatVariablesToSave.ContainsKey(name))
        {
            floatVariablesToSave[name] = var;
            Debug.Log("Set variable " + name + " to " + var.ToString());
        }
        else
        {
            floatVariablesToSave.Add(name, var);
            Debug.Log("Created var " + name + " with value " + var.ToString());
        }
    }

    public void SaveVariable(string name, string var)
    {
        if (stringVariablesToSave.ContainsKey(name))
        {
            stringVariablesToSave[name] = var;
            Debug.Log("Set variable " + name + " to " + var);
        }
        else
        {
            stringVariablesToSave.Add(name, var);
            Debug.Log("Created var " + name + " with value " + var);
        }
    }

    public void SaveGame()
    {
        string fullKeys = "";

        int ammountRead = 0;
        foreach(string key in floatVariablesToSave.Keys)
        {
            if (ammountRead != 0)
            {
                fullKeys += "|";
            }
            ammountRead++;
            fullKeys += key;
            PlayerPrefs.SetFloat(key, floatVariablesToSave[key]);
        }

        PlayerPrefs.SetString("floatKeyNamesFull", fullKeys);

        fullKeys = "";
        ammountRead = 0;
        foreach(string key in stringVariablesToSave.Keys)
        {
            if (ammountRead != 0)
            {
                if (ammountRead != 0)
                {
                    fullKeys += "|";
                }
                ammountRead++;
                fullKeys += key;
                PlayerPrefs.SetString(key, stringVariablesToSave[key]);
            }
        }
        PlayerPrefs.SetString("stringKeyNamesFull", fullKeys);

    }

    public void LoadVariables()
    {
        if (PlayerPrefs.HasKey("floatKeyNamesFull"))
        {
            string fullKeys = PlayerPrefs.GetString("floatKeyNamesFull");

            if (fullKeys.Contains("|"))
            {
                string[] keys = fullKeys.Split('|');

                for (int i = 0; i < keys.Length; i++)
                {
                    if (PlayerPrefs.HasKey(keys[i]))
                    {
                        floatVariablesToSave.Add(keys[i], PlayerPrefs.GetFloat(keys[i]));
                        Debug.Log("Loaded " + keys[i]+" with value: "+floatVariablesToSave[keys[i]].ToString());
                    }
                }
            }
            else if (fullKeys != "")
            {
                if (PlayerPrefs.HasKey(fullKeys))
                {
                    floatVariablesToSave.Add(fullKeys, PlayerPrefs.GetFloat(fullKeys));
                        Debug.Log("Loaded " + fullKeys);
                        Debug.Log("Loaded " + fullKeys + " with value: "+floatVariablesToSave[fullKeys].ToString());
                }
            }

            fullKeys = PlayerPrefs.GetString("stringKeyNamesFull");

            if (fullKeys.Contains("|"))
            {
                string[] keys = fullKeys.Split('|');

                for (int i = 0; i < keys.Length; i++)
                {
                    if (PlayerPrefs.HasKey(keys[i]))
                    {
                        stringVariablesToSave.Add(keys[i], PlayerPrefs.GetString(keys[i]));
                        Debug.Log("Loaded " + keys[i] + " with value: " + stringVariablesToSave[keys[i]].ToString());
                    }
                }
            }
            else if (fullKeys != "")
            {
                if (PlayerPrefs.HasKey(fullKeys))
                {
                    stringVariablesToSave.Add(fullKeys, PlayerPrefs.GetString(fullKeys));
                    Debug.Log("Loaded " + fullKeys);
                    Debug.Log("Loaded " + fullKeys + " with value: " + stringVariablesToSave[fullKeys].ToString());
                }
            }

        }
    }

}