
using SCGPool;

using UnityEngine;

namespace Assets
{
    public class TestMonoComponent : MonoBehaviour,IPool
    {
        public int xx = 0;
        public void OnGet()
        {
            xx = 10;
            Debug.Log("OnGet.  start -- > "+GetHashCode() + " ----- " + xx);
        }

        public void OnRelease()
        {
            xx = 0; 
            Debug.Log("OnRelease.   -- > " + GetHashCode() + " ----- " + xx);
        }

        

       
    }
}