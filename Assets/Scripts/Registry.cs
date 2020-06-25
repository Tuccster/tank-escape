using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Registry
{
    private static Dictionary<string, object> _registryDictionary = new Dictionary<string, object>();

    public static void AddRegister<T>(string key, T value) where T : class
    {
        key.ToLower();
        if (_registryDictionary.ContainsKey(key))
        {
            UnityEngine.Debug.LogWarning($"Attempting to add register with key '{key}' even though one already exists");
            return;
        }
        _registryDictionary.Add(key, value);
    }

    public static T GetRegister<T>(string key) where T : class
    {
        if (!_registryDictionary.ContainsKey(key))
        {
            UnityEngine.Debug.LogWarning($"Attempted to get register with key '{key}' but one does not exist.");
            return null;
        }
        return _registryDictionary[key] as T;
    }

    public static int GetAmountOfType<T>()
    {
        int typeMatches = 0;
        foreach(KeyValuePair<string, object> register in _registryDictionary)
            if (register.Value.GetType() == typeof(T))
                typeMatches++;
        return typeMatches;
    }

    public static int GetRegistryAmount()
    {
        return _registryDictionary.Count;
    }

    public static string[] GetAllRegisterIDs()
    {
        List<string> registerIDs = new List<string>();
        foreach(KeyValuePair<string, object> register in _registryDictionary)
            registerIDs.Add(register.Key);
        return registerIDs.ToArray();
    }
}
