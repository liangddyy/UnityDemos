using UnityEngine;
using System.Collections;

namespace Liangddyy.Util.SingleTonManage
{
    /// <summary>
    /// 单例 基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        public static T GetInstance()
        {
            return _instance;
        }

        public void SetInstance(T t)
        {
            if (_instance == null)
            {
                _instance = t;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            return;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Release()
        {
            return;
        }
    }
}