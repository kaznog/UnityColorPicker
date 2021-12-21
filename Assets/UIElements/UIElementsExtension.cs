using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public static class UIElementsExtension
{
    public static void SetPosition(this VisualElement self, Vector3 v)
    {
        if (self == null) return;
        var c = self.transform.position;
        c = v;
        self.transform.position = c;
    }

    public static void SetRotation(this VisualElement self, Vector3 v)
    {
        var c = self.transform.rotation;
        c = Quaternion.Euler(v);
        self.transform.rotation = c;
    }

    public static void SetScale(this VisualElement self, Vector3 v)
    {
        var c = self.transform.scale;
        c = v;
        self.transform.scale = c;
    }

    public static void SetWidth(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.width;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.width = c;
    }

    public static void SetMinWidth(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.minWidth;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.minWidth = c;
    }

    public static void SetHeight(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.height;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.height = c;
    }

    public static void SetMinHeight(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.minHeight;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.minHeight = c;
    }

    public static void SetPositionTop(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.top;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.top = c;
    }

    public static void SetPositionBottom(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.bottom;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.bottom = c;
    }

    public static void SetPositionLeft(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.left;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.left = c;
    }

    public static void SetPositionRight(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.right;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.right = c;
    }

    public static void SetMargineTop(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.marginTop;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.marginTop = c;
    }

    public static void SetMargineBottom(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.marginBottom;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.marginBottom = c;
    }

    public static void SetMargineLeft(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.marginLeft;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.marginLeft = c;
    }

    public static void SetMargineRight(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.marginRight;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.marginRight = c;
    }

    public static void SetPaddingTop(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.paddingTop;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.paddingTop = c;
    }

    public static void SetPaddingBottom(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.paddingBottom;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.paddingBottom = c;
    }

    public static void SetPaddingLeft(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.paddingLeft;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.paddingLeft = c;
    }

    public static void SetPaddingRight(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.paddingRight;
        var lu = c.value;
        lu.value = v;
        lu.unit = v2;
        c.value = lu;
        self.style.paddingRight = c;
    }

    public static void SetTextAlign(this VisualElement self, Align v)
    {
        var c = self.style.alignSelf;
        c.value = v;
        self.style.alignSelf = c;
    }

    public static void SetTextAlign(this VisualElement self, TextAnchor v)
    {
        var c = self.style.unityTextAlign;
        c.value = v;
        self.style.unityTextAlign = c;
    }

    public static void SetFontStyle(this VisualElement self, FontStyle v)
    {
        var c = self.style.unityFontStyleAndWeight;
        c.value = v;
        self.style.unityFontStyleAndWeight = c;
    }

    public static void SetFontSize(this VisualElement self, float v, LengthUnit v2)
    {
        var c = self.style.fontSize;
        var c2 = c.value;
        c2.value = v;
        c2.unit = v2;
        c.value = c2;
        self.style.fontSize = c;
    }

    public static float GetFontSize(this VisualElement self)
    {
        var c = self.style.fontSize;
        var c2 = c.value;
        return c2.value;
    }

    public static void SetTextWrap(this VisualElement self, WhiteSpace v)
    {
        var c = self.style.whiteSpace;
        c.value = v;
        self.style.whiteSpace = c;
    }

    public static void SetDisplay(this VisualElement self, DisplayStyle v)
    {
        var c = self.style.display;
        c.value = v;
        self.style.display = c;
    }

    public static void SetVisibility(this VisualElement self, Visibility v)
    {
        var c = self.style.visibility;
        c.value = v;
        self.style.visibility = c;
    }


    public static void SetOpacity(this VisualElement self, float v)
    {
        var c = self.style.opacity;
        c.value = v;
        self.style.opacity = c;
    }

    public static void SetOverflow(this VisualElement self, Overflow v)
    {
        var c = self.style.overflow;
        c.value = v;
        self.style.overflow = c;
    }

    public static void SetBorderTopWidth(this VisualElement self, float v)
    {
        var c = self.style.borderTopWidth;
        c.value = v;
        self.style.borderTopWidth = c;
    }

    public static void SetBorderBottomWidth(this VisualElement self, float v)
    {
        var c = self.style.borderBottomWidth;
        c.value = v;
        self.style.borderBottomWidth = c;
    }

    public static void SetBorderLeftWidth(this VisualElement self, float v)
    {
        var c = self.style.borderLeftWidth;
        c.value = v;
        self.style.borderLeftWidth = c;
    }

    public static void SetBorderRightWidth(this VisualElement self, float v)
    {
        var c = self.style.borderRightWidth;
        c.value = v;
        self.style.borderRightWidth = c;
    }
}
