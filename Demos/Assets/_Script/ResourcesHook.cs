using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace Babybus
{
    public class Resources
    {
        public static Object[] FindObjectsOfTypeAll(Type type)
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll(type);
        }

        public static T[] FindObjectsOfTypeAll<T>() where T : Object
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<T>();
        }

        public static Object GetBuiltinResource(Type type, string path)
        {
            return UnityEngine.Resources.GetBuiltinResource(type, path);
        }

        public static T GetBuiltinResource<T>(string path) where T : Object
        {
            return UnityEngine.Resources.GetBuiltinResource<T>(path);
        }

        public static Object Load(string path)
        {
            return UnityEngine.Resources.Load(path);
        }

        public static Object Load(string path, Type systemTypeInstance)
        {
            return UnityEngine.Resources.Load(path, systemTypeInstance);
        }

        public static T Load<T>(string path) where T : Object
        {
            return Load(path, typeof(T)) as T;
        }

        public static Object[] LoadAll(string path)
        {
            return UnityEngine.Resources.LoadAll(path);
        }

        public static Object[] LoadAll(string path, Type systemTypeInstance)
        {
            return UnityEngine.Resources.LoadAll(path, systemTypeInstance);
        }

        public static T[] LoadAll<T>(string path) where T : Object
        {
            return UnityEngine.Resources.LoadAll<T>(path);
        }

        public static ResourceRequest LoadAsync(string path)
        {
            return UnityEngine.Resources.LoadAsync(path);
        }


        public static ResourceRequest LoadAsync(string path, Type type)
        {
            return UnityEngine.Resources.LoadAsync(path, type);
        }

        public static ResourceRequest LoadAsync<T>(string path) where T : Object
        {
            return UnityEngine.Resources.LoadAsync(path);
        }

        public static void UnloadAsset(Object assetToUnload)
        {
            UnityEngine.Resources.UnloadAsset(assetToUnload);
        }

        public static AsyncOperation UnloadUnusedAssets()
        {
            return UnityEngine.Resources.UnloadUnusedAssets();
        }
    }
}