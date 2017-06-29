using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Liangddyy.Util.SingleTonManage
{
    /// <summary>
    ///     单例管理类
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private static GameObject _rootObj;

        private static readonly List<Action> _singletonReleaseList = new List<Action>();

        public void Awake()
        {
            _rootObj = gameObject;
            DontDestroyOnLoad(_rootObj);

            StartCoroutine(InitSingletons());
        }

        /// <summary>
        ///     在这里进行所有单例的初始化
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitSingletons()
        {
            yield return null;
        }

        /// <summary>
        ///     在这里进行所有单例的销毁
        /// </summary>
        public void OnApplicationQuit()
        {
            for (var i = _singletonReleaseList.Count - 1; i >= 0; i--)
                _singletonReleaseList[i]();
        }


        private static void AddSingleton<T>() where T : Singleton<T>
        {
            if (_rootObj.GetComponent<T>() == null)
            {
                var t = _rootObj.AddComponent<T>();
                if (t == null)
                {
                    Debug.LogError("未找到组件：" + typeof(T));
                }
                t.SetInstance(t);
                t.Init();
                //委托 添加到list(用于释放单例）
                _singletonReleaseList.Add(delegate { t.Release(); });
            }
        }

        public static T GetSingleton<T>() where T : Singleton<T>
        {
            var t = _rootObj.GetComponent<T>();

            if (t == null)
                AddSingleton<T>();

            return t;
        }
    }
}