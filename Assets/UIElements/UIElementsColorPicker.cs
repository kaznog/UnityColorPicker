using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Mathf;

namespace UIElementsExtend
{
    public class UIElementsColorPickerDragger : MouseManipulator
    {
        private bool m_Focus;
        public delegate void OnMouseDownDelegate(MouseDownEvent evt);
        public OnMouseDownDelegate OnMouseDownCallback;
        public delegate void OnMouseUpDelegate(MouseUpEvent evt);
        public OnMouseUpDelegate OnMouseUpCallback;
        public delegate void OnMouseCaptureOutDelegate(MouseCaptureOutEvent evt);
        public OnMouseCaptureOutDelegate OnMouseCaptureOutCallback;
        public delegate void OnMouseMoveDelegate(MouseMoveEvent evt);
        public OnMouseMoveDelegate OnMouseMoveCallback;

        public UIElementsColorPickerDragger()
        {
            activators.Add(new ManipulatorActivationFilter { button = UnityEngine.UIElements.MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            /// フォーカス状態を初期化
            m_Focus = false;

            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
            target.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            target.UnregisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            OnMouseDownCallback = null;
            OnMouseUpCallback = null;
            OnMouseCaptureOutCallback = null;
            OnMouseMoveCallback = null;
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            /// 設定した有効条件である左クリックか？
            if (CanStartManipulation(evt))
            {
                m_Focus = true;
                target.CaptureMouse();
                if (OnMouseDownCallback != null)
                {
                    OnMouseDownCallback(evt);
                }
            }
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (CanStopManipulation(evt))
            {
                target.ReleaseMouse();
                m_Focus = false;
                if (OnMouseUpCallback != null)
                {
                    OnMouseUpCallback(evt);
                }
            }
        }

        private void OnMouseCaptureOut(MouseCaptureOutEvent evt)
        {
            m_Focus = false;
            if (OnMouseCaptureOutCallback != null)
            {
                OnMouseCaptureOutCallback(evt);
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (m_Focus)
            {
                if (OnMouseMoveCallback != null)
                {
                    OnMouseMoveCallback(evt);
                }
            }
        }
    }

    public class UIElementsColorPicker : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<UIElementsColorPicker> { }

        private const float recip2Pi = 0.159154943f;
        private const string colorPickerShaderName = "UI/ColorPicker";
        private static readonly int _HSV = Shader.PropertyToID(nameof(_HSV));
        private static readonly int _AspectRatio = Shader.PropertyToID(nameof(_AspectRatio));
        private static readonly int _HueCircleInner = Shader.PropertyToID(nameof(_HueCircleInner));
        private static readonly int _SVSquareSize = Shader.PropertyToID(nameof(_SVSquareSize));
        private Shader m_colorPickerShader;
        private Material m_generatedMaterial;

        public float h, s, v;
        public Color color
        {
            get { return Color.HSVToRGB(h, s, v); }
            set
            {
                Color.RGBToHSV(value, out h, out s, out v);
                ApplyColor();
            }
        }

        public UnityAction<Color> OnColorChanged;
        private void ApplyColor()
        {
            if (m_generatedMaterial == null)
            {
                m_generatedMaterial = new Material(m_colorPickerShader);
                m_generatedMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            m_generatedMaterial.SetVector(_HSV, new Vector3(h, s, v));

            OnColorChanged?.Invoke(color);
        }

        private UIElementsColorPickerDragger m_UIElementsColorPickerDragger;
        private RenderTexture m_RenderTexture;
        private bool m_DetachFromPanel;
        private Image m_Image;
        private float contentRectWidth, contentRectHeight;
        private enum PointerDownLocation { HueCircle, SVSquare, OutSide }
        private PointerDownLocation pointerDownLocation = PointerDownLocation.OutSide;
        public bool m_Editable = true;
        public bool m_GeometryInitialized = false;

        public UIElementsColorPicker()
        {
            m_colorPickerShader = Shader.Find(colorPickerShaderName);
            m_generatedMaterial = new Material(m_colorPickerShader);
            m_generatedMaterial.hideFlags = HideFlags.HideAndDontSave;

            contentRectWidth = 0;
            contentRectHeight = 0;

            m_Image = new Image();
            SetMargin(m_Image, 0);
            SetPadding(m_Image, 0);
            m_Image.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            m_Image.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            Add(m_Image);

            m_UIElementsColorPickerDragger = new UIElementsColorPickerDragger();
            m_UIElementsColorPickerDragger.OnMouseDownCallback = OnMouseDown;
            m_UIElementsColorPickerDragger.OnMouseUpCallback = OnMouseUp;
            m_UIElementsColorPickerDragger.OnMouseCaptureOutCallback = OnMouseCaptureOut;
            m_UIElementsColorPickerDragger.OnMouseMoveCallback = OnMouseMove;
            this.AddManipulator(m_UIElementsColorPickerDragger);

            m_DetachFromPanel = false;
            m_GeometryInitialized = false;
            RegisterCallback<DetachFromPanelEvent>(DetachFromPanelEventCallback);
            RegisterCallback<GeometryChangedEvent>(evt =>
            {
                m_GeometryInitialized = true;
            });

            ApplyColor();
            if (Application.isPlaying)
            {
                CoroutineHandler.StartStaticCoroutine(this.Updator());
            }
        }

        private void DetachFromPanelEventCallback(DetachFromPanelEvent evt)
        {
            if (m_GeometryInitialized == false) return;
            m_Image.image = null;
            if (m_RenderTexture != null)
            {
                UnityEngine.Object.DestroyImmediate(m_RenderTexture);
                m_RenderTexture = null;
            }
            if (m_generatedMaterial != null)
            {
                UnityEngine.Object.DestroyImmediate(m_generatedMaterial);
                m_generatedMaterial = null;
            }
            m_DetachFromPanel = true;
        }

        private IEnumerator Updator()
        {
            while (m_DetachFromPanel == false)
            {
                if (!float.IsNaN(contentRect.width) && !float.IsNaN(contentRect.height) && contentRect.width > 0 && contentRect.height > 0)
                {
                    if (m_generatedMaterial == null)
                    {
                        m_generatedMaterial = new Material(m_colorPickerShader);
                        m_generatedMaterial.hideFlags = HideFlags.HideAndDontSave;
                    }
                    if (contentRectWidth != contentRect.width || contentRectHeight != contentRect.height)
                    {
                        contentRectWidth = contentRect.width;
                        contentRectHeight = contentRect.height;
                        m_generatedMaterial.SetFloat(_AspectRatio, contentRect.width / contentRect.height);
                    }
                    if (m_RenderTexture != null)
                    {
                        UnityEngine.Object.DestroyImmediate(m_RenderTexture);
                        m_RenderTexture = null;
                    }
                    if (m_RenderTexture == null)
                    {
                        Debug.Log(string.Format("Color Picker Target Texture Create contentRect.width:{0} contentRect.height:{1} GetSquaredRect:{2}", contentRect.width, contentRect.height, GetSquaredRect()));
                        m_RenderTexture = new RenderTexture((int)contentRect.width, (int)contentRect.height, 32);
                    }
                    Graphics.DrawTexture(new Rect(0, 0, contentRect.width, contentRect.height), m_RenderTexture, m_generatedMaterial);
                    RenderTexture prevActiveTex = RenderTexture.active;
                    RenderTexture.active = m_RenderTexture;
                    Graphics.Blit(m_RenderTexture, m_generatedMaterial);
                    m_Image.image = m_RenderTexture;
                    RenderTexture.active = prevActiveTex;
                }
                yield return null;
            }
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (m_Editable == false) return;
            var pos = GetRelativePosition(evt.localMousePosition);

            float r = pos.magnitude;

            if (r < 0.5f && r > m_generatedMaterial.GetFloat(_HueCircleInner))
            {
                pointerDownLocation = PointerDownLocation.HueCircle;
                h = (Atan2(-pos.y, pos.x) * recip2Pi + 1) % 1;
                ApplyColor();
            }
            else
            {
                var size = m_generatedMaterial.GetFloat(_SVSquareSize);

                // s -> x, v -> y
                if (pos.x >= -size && pos.x <= size && pos.y >= -size && pos.y <= size)
                {
                    pointerDownLocation = PointerDownLocation.SVSquare;
                    s = InverseLerp(-size, size, pos.x);
                    v = InverseLerp(-size, size, -pos.y);
                    ApplyColor();
                }
            }
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (m_Editable == false) return;
            pointerDownLocation = PointerDownLocation.OutSide;
        }

        private void OnMouseCaptureOut(MouseCaptureOutEvent evt)
        {
            if (m_Editable == false) return;
            pointerDownLocation = PointerDownLocation.OutSide;
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (m_Editable == false) return;
            var pos = GetRelativePosition(evt.localMousePosition);

            if (pointerDownLocation == PointerDownLocation.HueCircle)
            {
                h = (Atan2(-pos.y, pos.x) * recip2Pi + 1) % 1;
                ApplyColor();
            }

            if (pointerDownLocation == PointerDownLocation.SVSquare)
            {
                var size = m_generatedMaterial.GetFloat(_SVSquareSize);
                s = InverseLerp(-size, size, pos.x);
                v = InverseLerp(-size, size, -pos.y);
                ApplyColor();
            }
        }

        /// <summary>
        /// returns position in range -0.5 .. 0.5 when it's inside color picker square area
        /// </summary>
        public Vector2 GetRelativePosition(Vector2 position)
        {
            var rect = GetSquaredRect();

            return new Vector2(InverseLerpUnclamped(rect.xMin, rect.xMax, position.x), InverseLerpUnclamped(rect.yMin, rect.yMax, position.y)) - Vector2.one * 0.5f;
        }

        public Rect GetSquaredRect()
        {
            Rect rect = contentRect;
            var smallestDimension = Min(rect.width, rect.height);
            return new Rect(rect.center - Vector2.one * smallestDimension * 0.5f, Vector2.one * smallestDimension);
        }

        public float InverseLerpUnclamped(float min, float max, float value)
        {
            return (value - min) / (max - min);
        }

        private static void SetMargin(VisualElement element, float px)
        {
            element.style.marginLeft = px;
            element.style.marginRight = px;
            element.style.marginTop = px;
            element.style.marginBottom = px;
        }

        private static void SetPadding(VisualElement element, float px)
        {
            element.style.paddingLeft = px;
            element.style.paddingRight = px;
            element.style.paddingTop = px;
            element.style.paddingBottom = px;
        }
    }
}