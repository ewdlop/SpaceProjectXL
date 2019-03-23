using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour {

    Delegate myDelegate;

    void Start()
    {
        B b = new B();
        B1 b1 = new B1();
        List<A> a = new List<A>
        {
            b,
            b1
        };
        foreach (A bs in a)
        {
            Type myType = bs.GetType();
            MethodInfo[] methods = myType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                Delegate delegates = Delegate.CreateDelegate(typeof(Func<int>), bs, method);
                myDelegate = Delegate.Combine(myDelegate, delegates);
               
            }
        }
    }
}
public class A
{
}

public class B:A
{

    public int Test()
    {
        Debug.Log(2);
        return 0;
    }
}
public class B1 : A
{
    public int Test()
    {
        Debug.Log(4);
        return 0;
    }
}