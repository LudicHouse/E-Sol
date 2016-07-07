using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Save
{
    public System.DateTime saveDate;

    public int randomSeed;
    public float height;

    public float smogTimer;
    public float pestTimer;
    public float canLevel;
    public float hydrationLevel;

    public string selectedAccessory;
    public List<string> unlockedAccessories = new List<string>();

    public Dictionary<int, string> selectedAnimals = new Dictionary<int, string>();
    public List<string> unlockedAnimals = new List<string>();

    /// <summary>
    /// Load a save from file.
    /// </summary>
    /// <param name="path">The path to load from.</param>
    /// <returns>The loaded save.</returns>
    public static Save load(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        Save loadedSave = (Save)bf.Deserialize(file);
        file.Close();

        return loadedSave;
    }

    /// <summary>
    /// Write the current save to file.
    /// </summary>
    /// <param name="path">The path to write to.</param>
    public void save(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, this);
        file.Close();
    }
}
