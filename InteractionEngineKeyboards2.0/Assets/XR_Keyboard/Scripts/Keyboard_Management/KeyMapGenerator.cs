﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyMapGenerator : MonoBehaviour
{
    public GameObject KeyboardGameObject;
    public GameObject keyPrefab;
    public GameObject shadowPrefab;
    public KeyMap keyboardMap;
    public UIKeyboardResizer keyboardResizer;
    public bool overWritePrefab = false;

    private List<Transform> keyboardRows = new List<Transform>();
    private List<Transform> shadowRows = new List<Transform>();


    // Start is called before the first frame update
    void Awake()
    {
        PopulateRows();
        // If the keyboard is empty of keys them generate a new one
        if (keyboardRows[0].GetComponentsInChildren<TextInputButton>().Length == 0)
        {
            RegenerateKeyboard();
        }
    }

    private void PopulateRows()
    {
        foreach (UIKeyboardResizer.KeyboardLayoutObjects keyboardLayoutObject in keyboardResizer.keyboardLayoutObjects)
        {
            keyboardLayoutObject.KeysParent.GetComponentsInChildren<HorizontalLayoutGroup>().ToList().ForEach(
                keyboardRow =>
                {
                    keyboardRows.Add(keyboardRow.transform);
                }
            );

            keyboardLayoutObject.ShadowsParent.GetComponentsInChildren<HorizontalLayoutGroup>().ToList().ForEach(
               shadowRow =>
               {
                   shadowRows.Add(shadowRow.transform);
               }
           );
        }
    }

    public void RegenerateKeyboard()
    {
        PopulateRows();

        if (keyboardMap == null)
        {
            keyboardMap = GetComponent<KeyMap>();
            if (keyboardMap == null)
            {
                keyboardMap = gameObject.AddComponent<DefaultKeyMap>();
            }
        }

        if (!keyPrefab.transform.GetComponentInChildren<TextInputButton>())
        {
            throw new System.Exception("Ensure prefab contains an object with the TextInputButton component");
        }

        ClearKeys();
        ClearShadows();

        GenerateKeyboard();
    }

    private void GenerateKeyboard()
    {
        var keyMap = keyboardMap.GetKeyMap();
        for (int i = 0; i < keyboardRows.Count; i++)
        {
            foreach (var key in keyMap[i].row)
            {
                GameObject shadow = Instantiate(shadowPrefab, shadowRows[i]);
                GameObject newKey = Instantiate(keyPrefab, keyboardRows[i]);
                TextInputButton button = newKey.GetComponentInChildren<TextInputButton>();
                button.NeutralKey = key.neutralKey;
                button.Symbols1Key = key.symbols1Key;
                button.Symbols2Key = key.symbols2Key;
                button.UpdateActiveKey(button.NeutralKey, Keyboard.KeyboardMode.NEUTRAL);
                newKey.name = button.NeutralKey.ToString();
            }
        }
        keyboardResizer.ResizeKeyboard();
    }

    private void ClearKeys()
    {
        foreach (Transform row in keyboardRows)
        {
            for (int i = row.childCount - 1; i >= 0; i--)
            {
                if (row.GetChild(i).GetComponentInChildren<TextInputButton>())
                {
                    DestroyImmediate(row.GetChild(i).gameObject);
                }
            }
        }
    }

    private void ClearShadows()
    {
        foreach (Transform row in shadowRows)
        {
            for (int i = row.childCount - 1; i >= 0; i--)
            {
                if (row.GetChild(i).GetComponentInChildren<TextInputButton>())
                {
                    DestroyImmediate(row.GetChild(i).gameObject);
                }
            }
        }
    }


    public void SetNewKeyPrefab(GameObject newPrefab)
    {
        keyPrefab = newPrefab;

        if (Application.isPlaying)
        {
            RegenerateKeyboard();
        }
    }

    public void SetNewKeyMap(KeyMap newMap)
    {
        keyboardMap = newMap;

        if (Application.isPlaying)
        {
            RegenerateKeyboard();
        }
    }
}
