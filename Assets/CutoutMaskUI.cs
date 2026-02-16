using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMaskUI : Image
{
    public override Material materialForRendering {
        get
        {
            Material revMask = new Material(base.materialForRendering);
            revMask.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return revMask;
        }
    }
}
