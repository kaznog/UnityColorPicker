using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorPickerTest : MonoBehaviour
{
    public enum ColorValues
    {
        R,
        G,
        B,
        A
    }
    private VisualElement m_rootElement;
    private VisualElement m_container;
    private VisualElement m_ColorPreviewContainer;
    private Image m_ColorPreviewImage;
    private VisualElement m_ColorPickerContainer;
    private VisualElement m_RGBA_SliderContainer;
    private VisualElement m_R_SliderContainer;
    private VisualElement m_G_SliderContainer;
    private VisualElement m_B_SliderContainer;
    private VisualElement m_A_SliderContainer;
    private Slider m_R_Slider;
    private Slider m_G_Slider;
    private Slider m_B_Slider;
    private Slider m_A_Slider;
    private float m_currentColorR = 0f;
    private float m_currentColorG = 0f;
    private float m_currentColorB = 0f;
    private float m_currentColorA = 0f;
    private Texture2D m_R_SliderTrackerTexture = null;
    private Texture2D m_G_SliderTrackerTexture = null;
    private Texture2D m_B_SliderTrackerTexture = null;
    private Texture2D m_A_SliderTrackerTexture = null;
    private VisualElement m_R_TrackerImage = null;
    private VisualElement m_G_TrackerImage = null;
    private VisualElement m_B_TrackerImage = null;
    private VisualElement m_A_TrackerImage = null;
    private Texture2D m_texture2DSliderBackground = null;
    private UIElementsExtend.UIElementsColorPicker m_UIElementsColorPicker;
    private float contentWidth;
    private float contentHeight;

    // Start is called before the first frame update
    public void Start()
    {
        var document = GetComponent<UIDocument>();
        if (document != null)
        {
            m_rootElement = document.rootVisualElement;
            m_container = m_rootElement.Q<VisualElement>("Container");
            m_ColorPreviewContainer = m_container.Q<VisualElement>("ColorPreviewContainer");
            m_ColorPickerContainer = m_container.Q<VisualElement>("ColorPickerContainer");
            m_RGBA_SliderContainer = m_container.Q<VisualElement>("RGBA_SliderContainer");
            m_UIElementsColorPicker = m_ColorPickerContainer.Q<UIElementsExtend.UIElementsColorPicker>("ColorPicker");
            m_UIElementsColorPicker.m_Editable = true;

            m_ColorPreviewImage = m_ColorPreviewContainer.Q<Image>("ColorPreviewImage");

            m_R_SliderContainer = m_RGBA_SliderContainer.Q<VisualElement>("R_SliderContainer");
            m_G_SliderContainer = m_RGBA_SliderContainer.Q<VisualElement>("G_SliderContainer");
            m_B_SliderContainer = m_RGBA_SliderContainer.Q<VisualElement>("B_SliderContainer");
            m_A_SliderContainer = m_RGBA_SliderContainer.Q<VisualElement>("A_SliderContainer");
            m_R_Slider = m_R_SliderContainer.Q<Slider>("R_Slider");
            m_G_Slider = m_G_SliderContainer.Q<Slider>("G_Slider");
            m_B_Slider = m_B_SliderContainer.Q<Slider>("B_Slider");
            m_A_Slider = m_A_SliderContainer.Q<Slider>("A_Slider");
            m_R_Slider.RegisterCallback<GeometryChangedEvent, Slider>(initializeSlider, m_R_Slider);
            m_G_Slider.RegisterCallback<GeometryChangedEvent, Slider>(initializeSlider, m_G_Slider);
            m_B_Slider.RegisterCallback<GeometryChangedEvent, Slider>(initializeSlider, m_B_Slider);
            m_A_Slider.RegisterCallback<GeometryChangedEvent, Slider>(initializeSlider, m_A_Slider);
            m_R_Slider.RegisterCallback<ChangeEvent<float>>(ChangeEventRSliderCallback);
            m_G_Slider.RegisterCallback<ChangeEvent<float>>(ChangeEventGSliderCallback);
            m_B_Slider.RegisterCallback<ChangeEvent<float>>(ChangeEventBSliderCallback);
            m_A_Slider.RegisterCallback<ChangeEvent<float>>(ChangeEventASliderCallback);

            m_UIElementsColorPicker.OnColorChanged = OnColorChanged;

            RectTransform rectTrans = GetComponent<RectTransform>();
            Rect rect = CalcurateScreenSpaceRect(rectTrans);
            Debug.Log(string.Format("Container rect.x:{0} y:{1} width:{2} height:{3}", rect.x, rect.y, rect.width, rect.height));
            m_container.SetPositionLeft(rect.x, LengthUnit.Pixel);
            m_container.SetPositionTop(rect.y, LengthUnit.Pixel);
            m_container.SetWidth(rect.width, LengthUnit.Pixel);
            m_container.SetMinWidth(rect.width, LengthUnit.Pixel);
            m_container.SetHeight(rect.height, LengthUnit.Pixel);
            m_container.SetMinHeight(rect.height, LengthUnit.Pixel);
            m_ColorPickerContainer.SetWidth(rect.width, LengthUnit.Pixel);
            m_ColorPickerContainer.SetMinWidth(rect.width, LengthUnit.Pixel);
            m_ColorPickerContainer.SetHeight(rect.width, LengthUnit.Pixel);
            m_ColorPickerContainer.SetMinHeight(rect.width, LengthUnit.Pixel);
            m_UIElementsColorPicker.SetWidth(rect.width, LengthUnit.Pixel);
            m_UIElementsColorPicker.SetMinWidth(rect.width, LengthUnit.Pixel);
            m_UIElementsColorPicker.SetHeight(rect.width, LengthUnit.Pixel);
            m_UIElementsColorPicker.SetMinHeight(rect.width, LengthUnit.Pixel);
            m_RGBA_SliderContainer.SetWidth(rect.width, LengthUnit.Pixel);
            m_RGBA_SliderContainer.SetMinWidth(rect.width, LengthUnit.Pixel);
            contentWidth = rect.width;
            contentHeight = rect.height;

            StartCoroutine(DoWaitinitialize(() =>
            {
                SetColor(new Color(0, 0, 0, 255));
            }));

        }
    }

    // Update is called once per frame
    public void Update()
    {
        //RectTransform rectTrans = GetComponent<RectTransform>();
        //Rect rect = CalcurateScreenSpaceRect(rectTrans);
        //if (rect.width != contentWidth || rect.height != contentHeight)
        //{
        //    Debug.Log(string.Format("Container rect.x:{0} y:{1} width:{2} height:{3}", rect.x, rect.y, rect.width, rect.height));
        //    m_container.SetPositionLeft(rect.x, LengthUnit.Pixel);
        //    m_container.SetPositionTop(rect.y, LengthUnit.Pixel);
        //    m_container.SetWidth(rect.width, LengthUnit.Pixel);
        //    m_container.SetMinWidth(rect.width, LengthUnit.Pixel);
        //    m_container.SetHeight(rect.height, LengthUnit.Pixel);
        //    m_container.SetMinHeight(rect.height, LengthUnit.Pixel);
        //    m_ColorPickerContainer.SetWidth(rect.width, LengthUnit.Pixel);
        //    m_ColorPickerContainer.SetMinWidth(rect.width, LengthUnit.Pixel);
        //    m_ColorPickerContainer.SetHeight(rect.width, LengthUnit.Pixel);
        //    m_ColorPickerContainer.SetMinHeight(rect.width, LengthUnit.Pixel);
        //    m_UIElementsColorPicker.SetWidth(rect.width, LengthUnit.Pixel);
        //    m_UIElementsColorPicker.SetMinWidth(rect.width, LengthUnit.Pixel);
        //    m_UIElementsColorPicker.SetHeight(rect.width, LengthUnit.Pixel);
        //    m_UIElementsColorPicker.SetMinHeight(rect.width, LengthUnit.Pixel);
        //    m_RGBA_SliderContainer.SetWidth(rect.width, LengthUnit.Pixel);
        //    m_RGBA_SliderContainer.SetMinWidth(rect.width, LengthUnit.Pixel);
        //    contentWidth = rect.width;
        //    contentHeight = rect.height;
        //}
    }

    public void OnDestroy()
    {
        if (m_R_Slider != null)
        {
            m_R_Slider.UnregisterCallback<GeometryChangedEvent, Slider>(initializeSlider);
            m_G_Slider.UnregisterCallback<GeometryChangedEvent, Slider>(initializeSlider);
            m_B_Slider.UnregisterCallback<GeometryChangedEvent, Slider>(initializeSlider);
            m_A_Slider.UnregisterCallback<GeometryChangedEvent, Slider>(initializeSlider);
            m_R_Slider.UnregisterCallback<ChangeEvent<float>>(ChangeEventRSliderCallback);
            m_G_Slider.UnregisterCallback<ChangeEvent<float>>(ChangeEventGSliderCallback);
            m_B_Slider.UnregisterCallback<ChangeEvent<float>>(ChangeEventBSliderCallback);
            m_A_Slider.UnregisterCallback<ChangeEvent<float>>(ChangeEventASliderCallback);
        }
    }

    private void SetColor(Color color)
    {
        m_currentColorR = color.r;
        m_currentColorG = color.g;
        m_currentColorB = color.b;
        m_currentColorA = color.a;

        m_R_Slider.value = Mathf.RoundToInt(m_currentColorR * 255);
        m_G_Slider.value = Mathf.RoundToInt(m_currentColorG * 255);
        m_B_Slider.value = Mathf.RoundToInt(m_currentColorB * 255);
        m_A_Slider.value = Mathf.RoundToInt(m_currentColorA * 255);

        m_UIElementsColorPicker.color = color;
    }

    public void OnColorChanged(Color c)
    {
        if (m_currentColorR == Mathf.RoundToInt(c.r * 255)
        &&  m_currentColorG == Mathf.RoundToInt(c.g * 255)
        &&  m_currentColorB == Mathf.RoundToInt(c.b * 255))
        {
            return;
        }

        m_R_Slider.value = Mathf.RoundToInt(c.r * 255);
        m_G_Slider.value = Mathf.RoundToInt(c.g * 255);
        m_B_Slider.value = Mathf.RoundToInt(c.b * 255);
    }

    private IEnumerator DoWaitinitialize(System.Action act)
    {
        bool wait = true;
        while (wait)
        {
            yield return null;
            if (m_R_TrackerImage != null && m_G_TrackerImage != null && m_B_TrackerImage != null && m_A_TrackerImage != null)
            {
                wait = false;
            }
        }
        act();
    }

    private void initializeSlider(GeometryChangedEvent evt, Slider slider)
    {
        var tracker = slider.Q<VisualElement>("unity-tracker");
        if (tracker != null)
        {
            VisualElement trackerimage = null;
            string trackerimageName = "";
            if (slider == m_R_Slider)
            {
                trackerimageName = "unity-tracker-bg-red";
            }
            else if (slider == m_G_Slider)
            {
                trackerimageName = "unity-tracker-bg-green";
            }
            else if (slider == m_B_Slider)
            {
                trackerimageName = "unity-tracker-bg-blue";
            }
            else if (slider == m_A_Slider)
            {
                trackerimageName = "unity-tracker-bg-alpha";
            }
            trackerimage = tracker.Q<VisualElement>(trackerimageName);
            if (trackerimage != null) return;
            tracker.style.backgroundImage = m_texture2DSliderBackground;
            tracker.SetHeight(16, LengthUnit.Pixel);
            tracker.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
            tracker.SetMargineTop(0, LengthUnit.Pixel);
            tracker.SetMargineBottom(0, LengthUnit.Pixel);
            trackerimage = new VisualElement();
            trackerimage.name = trackerimageName;
            if (slider == m_R_Slider)
            {
                m_R_SliderTrackerTexture = generateSliderTexture(ColorValues.R);
                trackerimage.style.backgroundImage = m_R_SliderTrackerTexture;
                m_R_TrackerImage = trackerimage;
            }
            else if (slider == m_G_Slider)
            {
                m_G_SliderTrackerTexture = generateSliderTexture(ColorValues.G);
                trackerimage.style.backgroundImage = m_G_SliderTrackerTexture;
                m_G_TrackerImage = trackerimage;
            }
            else if (slider == m_B_Slider)
            {
                m_B_SliderTrackerTexture = generateSliderTexture(ColorValues.B);
                trackerimage.style.backgroundImage = m_B_SliderTrackerTexture;
                m_B_TrackerImage = trackerimage;
            }
            else if (slider == m_A_Slider)
            {
                m_A_SliderTrackerTexture = generateSliderTexture(ColorValues.A);
                trackerimage.style.backgroundImage = m_A_SliderTrackerTexture;
                m_A_TrackerImage = trackerimage;
            }
            trackerimage.SetHeight(16, LengthUnit.Pixel);
            trackerimage.style.unityBackgroundScaleMode = ScaleMode.StretchToFill;
            trackerimage.SetMargineTop(0, LengthUnit.Pixel);
            trackerimage.SetMargineBottom(0, LengthUnit.Pixel);
            tracker.Add(trackerimage);
            var dragger_border = slider.Q<VisualElement>("unity-dragger-border");
            dragger_border.SetHeight(28, LengthUnit.Pixel);
            var dragger = slider.Q<VisualElement>("unity-dragger");
            dragger.SetMargineTop(-4, LengthUnit.Pixel);
            dragger.SetHeight(24, LengthUnit.Pixel);
            var textfield = slider.Q<TextField>("unity-text-field");
            //textfield.SetMargineTop(3, LengthUnit.Pixel);
        }
    }

    private Texture2D generateSliderTexture(ColorValues type)
    {
        Color32 baseColor = m_UIElementsColorPicker.color;
        Texture2D texture;
        Color32[] colors;
        int size = 256;
        texture = new Texture2D(size, 1);
        colors = new Color32[size];
        switch (type)
        {
            case ColorValues.R:
                for (int i = 0; i < size; i++)
                {
                    colors[i] = new Color32((byte)i, baseColor.g, baseColor.b, 255);
                }
                break;
            case ColorValues.G:
                for (int i = 0; i < size; i++)
                {
                    colors[i] = new Color32(baseColor.r, (byte)i, baseColor.b, 255);
                }
                break;
            case ColorValues.B:
                for (int i = 0; i < size; i++)
                {
                    colors[i] = new Color32(baseColor.r, baseColor.g, (byte)i, 255);
                }
                break;
            case ColorValues.A:
                for (int i = 0; i < size; i++)
                {
                    colors[i] = new Color32(baseColor.r, baseColor.g, baseColor.b, (byte)i);
                }
                break;
        }
        texture.SetPixels32(colors);
        texture.Apply();
        return texture;
    }

    private void RefreshRedSliderBackgroundImage()
    {
        Destroy(m_R_SliderTrackerTexture);
        m_R_SliderTrackerTexture = generateSliderTexture(ColorValues.R);
        m_R_TrackerImage.style.backgroundImage = m_R_SliderTrackerTexture;
        m_R_TrackerImage.MarkDirtyRepaint();
    }

    private void RefreshGreenSliderBackgroundImage()
    {
        Destroy(m_G_SliderTrackerTexture);
        m_G_SliderTrackerTexture = generateSliderTexture(ColorValues.G);
        m_G_TrackerImage.style.backgroundImage = m_G_SliderTrackerTexture;
        m_G_TrackerImage.MarkDirtyRepaint();
    }

    private void RefreshBlueSliderBackgroundImage()
    {
        Destroy(m_B_SliderTrackerTexture);
        m_B_SliderTrackerTexture = generateSliderTexture(ColorValues.B);
        m_B_TrackerImage.style.backgroundImage = m_B_SliderTrackerTexture;
        m_B_TrackerImage.MarkDirtyRepaint();
    }

    private void RefreshAlphaSliderBackgroundImage()
    {
        Destroy(m_A_SliderTrackerTexture);
        m_A_SliderTrackerTexture = generateSliderTexture(ColorValues.A);
        m_A_TrackerImage.style.backgroundImage = m_A_SliderTrackerTexture;
        m_A_TrackerImage.MarkDirtyRepaint();
    }

    private void ChangeEventRSliderCallback(ChangeEvent<float> evt)
    {
        float newColorR = evt.newValue / 255;
        if (m_currentColorR == newColorR) return;
        m_currentColorR = newColorR;
        Color color = ColorUtils.FromRGBA((byte)evt.newValue, (byte)m_G_Slider.value, (byte)m_B_Slider.value, (byte)m_A_Slider.value);
        RefreshRedSliderBackgroundImage();
        m_UIElementsColorPicker.color = color;
        RefreshAlphaSliderBackgroundImage();
        StyleColor styleColor = m_ColorPreviewImage.style.backgroundColor;
        Color pc = styleColor.value;
        pc.r = color.r;
        styleColor.value = pc;
        m_ColorPreviewImage.style.backgroundColor = styleColor;
    }

    private void ChangeEventGSliderCallback(ChangeEvent<float> evt)
    {
        float newColorG = evt.newValue / 255;
        if (m_currentColorG == newColorG) return;
        m_currentColorG = newColorG;
        Color color = ColorUtils.FromRGBA((byte)m_R_Slider.value, (byte)evt.newValue, (byte)m_B_Slider.value, (byte)m_A_Slider.value);
        RefreshGreenSliderBackgroundImage();
        m_UIElementsColorPicker.color = color;
        RefreshAlphaSliderBackgroundImage();
        StyleColor styleColor = m_ColorPreviewImage.style.backgroundColor;
        Color pc = styleColor.value;
        pc.g = color.g;
        styleColor.value = pc;
        m_ColorPreviewImage.style.backgroundColor = styleColor;
    }

    private void ChangeEventBSliderCallback(ChangeEvent<float> evt)
    {
        float newColorB = evt.newValue / 255;
        if (m_currentColorB == newColorB) return;
        m_currentColorB = newColorB;
        Color color = ColorUtils.FromRGBA((byte)m_R_Slider.value, (byte)m_G_Slider.value, (byte)evt.newValue, (byte)m_A_Slider.value);
        RefreshBlueSliderBackgroundImage();
        m_UIElementsColorPicker.color = color;
        RefreshAlphaSliderBackgroundImage();
        StyleColor styleColor = m_ColorPreviewImage.style.backgroundColor;
        Color pc = styleColor.value;
        pc.b = color.b;
        styleColor.value = pc;
        m_ColorPreviewImage.style.backgroundColor = styleColor;
    }

    private void ChangeEventASliderCallback(ChangeEvent<float> evt)
    {
        float newColorA = evt.newValue / 255;
        if (m_currentColorA == newColorA) return;
        m_currentColorA = newColorA;

        Color color = ColorUtils.FromRGBA((byte)m_R_Slider.value, (byte)m_G_Slider.value, (byte)m_B_Slider.value, (byte)evt.newValue);
        m_UIElementsColorPicker.color = color;
        RefreshAlphaSliderBackgroundImage();
        StyleColor styleColor = m_ColorPreviewImage.style.backgroundColor;
        Color pc = styleColor.value;
        pc.a = color.a;
        styleColor.value = pc;
        m_ColorPreviewImage.style.backgroundColor = styleColor;
    }

    private Rect CalcurateScreenSpaceRect(RectTransform rectTransform)
    {
        var canvas = rectTransform.GetComponentInParent<Canvas>();
        var camera = canvas.worldCamera;
        var corners = new Vector3[4];

        rectTransform.GetWorldCorners(corners);
        var screenCorner1 = RectTransformUtility.WorldToScreenPoint(camera, corners[1]);
        var screenCorner3 = RectTransformUtility.WorldToScreenPoint(camera, corners[3]);

        var ScreenRect = new Rect();
        ScreenRect.x = screenCorner1.x;
        ScreenRect.width = screenCorner3.x - ScreenRect.x;
        ScreenRect.y = Screen.height - screenCorner1.y;
        ScreenRect.height = screenCorner1.y - screenCorner3.y;
        return ScreenRect;
    }
}
