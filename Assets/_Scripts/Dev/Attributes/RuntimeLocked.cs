using UnityEngine;
using System;

namespace Dev.Compiler {
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RuntimeLocked : PropertyAttribute { }
}