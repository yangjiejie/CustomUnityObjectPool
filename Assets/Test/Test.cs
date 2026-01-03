

using Assets;
using SCGPool;
using UnityEngine;

public class Test : MonoBehaviour
{
    [ShowInInspector("测试对象池")]
    void  TestPool1()
    {
        var go = PoolManager.Get("Assets/Art/prefab/TestPool.prefab");
        go.name = "测试对象池";

        
    }

    [ShowInInspector("测试释放对象池")]
    void TestPool2()
    {

        var go = GameObject.Find("测试对象池");
        PoolManager.Release(go); 


    }
    [ShowInInspector("测试组件对象池")]
    void TestComponentPool()
    {
        var go = PoolManager.Get<TestMonoComponent>("Assets/Art/prefab/TestComponent.prefab");
        go.name = "测试组件对象池";
    }
    [ShowInInspector("测试释放组件对象池")]
    void TestComponentPool2()
    {
        var go = GameObject.Find("测试组件对象池");
        var com = go.GetComponent<TestMonoComponent>();
        PoolManager.Release(com);
    }


}
