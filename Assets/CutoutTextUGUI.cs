using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class CutoutMaskTMP : TextMeshProUGUI
{
    private Material _cached;

    public override Material materialForRendering
    {
        get
        {
            var baseMat = base.materialForRendering;

            if (_cached == null || _cached.shader != baseMat.shader)
            {
                if (_cached != null) DestroyImmediate(_cached);
                _cached = new Material(baseMat);
            }
            else
            {
                _cached.CopyPropertiesFromMaterial(baseMat);
            }

            _cached.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return _cached;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_cached != null) DestroyImmediate(_cached);
        _cached = null;
    }
}