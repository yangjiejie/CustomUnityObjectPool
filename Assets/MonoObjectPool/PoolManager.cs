using System;
using System.Collections.Generic;

using Core;
using UnityEngine;

//desc 我需要一个对象池实现 仅仅只用manager调用Get即可 需要覆盖一下3类使用场景 

//第一类对象次 普通c#对象 cs           需要实现IPool接口 
//第二类对象池 比如mono对象gameObject  不需要实现IPool接口 
//第三类对象池 自定义的组件 component  需要实现IPool接口  
//上诉三类对象池比较常用 故此封装一他们的使用 
//特别的 对于第二类和第三类 也存在动态加载的情况，因此也提供了url接口 即Get会附带一个url加载链接

//另外需要提供对象次的统一清理接口，防止有人在某些场景未手动回收，我们需要在转场的时候清理 ReigsterRelease

namespace SCGPool
{
    public class PoolManager
    {
        private const int CSNormalObj = 1;
        private const int MonoGameObject = 2;
        private const int MonoComponent = 3;

        private static Dictionary<int, HashSet<Action>> poolMap = null;

        private static GameObject PoolRoot;
        public static void Init()
        {
            if(PoolRoot == null)
            {
                var go = new GameObject("PoolRoot");
                GameObject.DontDestroyOnLoad(go);
            }
        }
       
        /// <summary>
        /// 加载预设资源并且用于对象池 
        /// </summary>
        /// <param name="prefabRes"></param>
        /// <returns></returns>
        public static UnityEngine.GameObject Get(string prefabRes) 
        {
            
            var itemPrefab = LoadResApi.LoadRes<UnityEngine.GameObject>(prefabRes);
            return Get(itemPrefab);
        }
        /// <summary>
        ///  获取一个已经存在的gameObject并且以此为基础的对象 常见于滚动视图中的item
        ///  可能需要n个item的情况
        /// </summary>
        /// <param name="prefabTemplte"></param>
        /// <returns></returns>
        public static UnityEngine.GameObject Get(UnityEngine.GameObject prefabTemplte)
        {
            var result = Pool<UnityEngine.GameObject>.Get(() =>
            {
                var ins = GameObject.Instantiate(prefabTemplte);
                return ins;
            });
            ReigsterRelease<UnityEngine.GameObject>(MonoGameObject);
            return result;
        }

        private static void ReigsterRelease<T>(int type)
        {
            poolMap = poolMap ?? new Dictionary<int, HashSet<Action>>();
            if (!poolMap.ContainsKey(type))
            {
                poolMap.Add(type, new HashSet<Action>());
                poolMap[type].Add(Pool<T>.ReleaseAll);
            }
            else if (!poolMap[type].Contains(Pool<T>.ReleaseAll))
            {
                poolMap[type].Add(Pool<T>.ReleaseAll);
            }
        }
        public static T Get<T>() where T : IPool, new() // 普通c#对象
        {
            var result = Pool<T>.Get(() => new T());
            ReigsterRelease<T>(CSNormalObj);
            return result;
        }
        /// <summary>
        /// 获取脚本对象的对象池 (复用)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="bindGo"></param>
        /// <returns></returns>
        public static T Get<T>(GameObject bindGo) where T : Component, IPool, new() // 普通脚本对象
        {   
            var result = Pool<T>.Get(() =>
            {
                var go = GameObject.Instantiate<GameObject>(bindGo);
                var com = go.GetComponent<T>();
                if (com == null)
                {
                    com = go.AddComponent<T>();
                }
                return com;
            });
            ReigsterRelease<T>(MonoComponent);
            return result;
        }

        public static T Get<T>( string prefabRes) where T : Component, IPool, new() // 普通脚本对象
        {
            var result =  Pool<T>.Get(() =>
            {

                var go = LoadResApi.LoadRes<GameObject>(prefabRes);
                var goIns = GameObject.Instantiate(go);
                var com = goIns.GetComponent<T>();
                if (com == null)
                {
                    com = goIns.AddComponent<T>();
                }
                return com;
            });
            ReigsterRelease<T>(MonoComponent);  
            return result;
        }

        public static void ReleaseAll()
        {
            if (poolMap == null || poolMap.Count == 0) return;
            foreach(var item in poolMap)
            {
                foreach(var va in item.Value)
                {
                    va.Invoke();
                }
                item.Value.Clear();
            }
            poolMap?.Clear();
        }

        public static void Release<T>(T t)
        {
            Pool<T>.Release(t); 
        }

    }
}
