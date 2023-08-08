using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scriptable 
{
    public enum ColorType
    {
        None = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
        Orange = 4,
    }

    [CreateAssetMenu(menuName = "ColorData")]
    public class ColorData : ScriptableObject
    {
        //theo tha material theo dung thu tu ColorType
        [SerializeField] Material[] materials;

        //lay material theo mau tuong ung
        public Material GetMat(ColorType colorType)
        {
            return materials[(int)colorType];
        }
    }
}