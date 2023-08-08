using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    public class Character : MonoBehaviour
    {
        //keo tha color data vao
        [SerializeField] ColorData colorData;
        //keo mesh renderer vao
        [SerializeField] Renderer meshRenderer;
        public ColorType color;

        //thay doi mau object
        public void ChangeColor(ColorType colorType)
        {
            color = colorType;
            meshRenderer.material = colorData.GetMat(colorType);
        }
    }

}