using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core
{
    public  class LoadResApi
    {
        public static T LoadRes<T>(string resUrl) where T : UnityEngine.Object
        {
            if(!resUrl.StartsWith("Assets"))
            {
                UnityEngine.Debug.LogError("资源必须是Assets开头");
            }
            
            ///平台加载接口 用宏定义区分 这里我先不写 具体项目都不一样 
            ///这里只测试同步加载接口 
#if UNITY_EDITOR
            try
            {
                var result = AssetDatabase.LoadAssetAtPath<T>(resUrl);
                return result;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
#else
            //Load AssetBundle and Then Load Sub res 
#endif

            return default(T);
        }
    }
}
