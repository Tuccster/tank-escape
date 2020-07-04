using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public class Ref<T>
    {
        private T backing;
        public T Value {get{return backing;}}
        public Ref(T reference)
        {
            backing = reference;
        }
    }
}
