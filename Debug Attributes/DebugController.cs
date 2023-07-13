using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    [Serializable]
    public class DebugField
    {
        public MemberInfo MemberInfo;
        public DebugAttribute Attribute;
        public TextMeshProUGUI TextMeshPro;
    }

    private Canvas m_Canvas;
    private GameObject m_Panel;

    public TMP_FontAsset font;
    public float fontSize = 36;

    private List<DebugField> m_DebugFields;

    private void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        m_Panel = transform.GetChild(0).gameObject;
        m_DebugFields = new List<DebugField>();

        VerticalLayoutGroup layoutGroup = m_Panel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;

        AddDebugFields();
    }

    private void AddDebugFields()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (MemberInfo member in members)
                {
                    DebugAttribute attribute = member.GetCustomAttribute<DebugAttribute>();

                    if (attribute != null)
                    {
                        DebugField debugField = new DebugField();
                        debugField.MemberInfo = member;
                        debugField.Attribute = attribute;

                        GameObject textMesh = new GameObject("Debug Text Mesh");
                        RectTransform rectTransform = textMesh.AddComponent<RectTransform>();
                        textMesh.transform.parent = m_Panel.transform;

                        rectTransform.anchorMin = new Vector2(0f, 1f);
                        rectTransform.anchorMax = new Vector2(1f, 1f);
                        rectTransform.pivot = new Vector2(0.5f, 1f);
                        rectTransform.offsetMin = Vector2.zero;
                        rectTransform.offsetMax = Vector2.zero;

                        TextMeshProUGUI textMeshPro = textMesh.AddComponent<TextMeshProUGUI>();

                        if (font != null)
                        {
                            textMeshPro.font = font;
                        }

                        textMeshPro.fontSize = fontSize;
                        textMeshPro.text = member.Name;

                        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, fontSize);

                        debugField.TextMeshPro = textMeshPro;
                        m_DebugFields.Add(debugField);
                    }
                }
            }
        }
    }

    private void Update()
    {
        foreach (var debugField in m_DebugFields)
        {
            if (debugField.MemberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)debugField.MemberInfo;

                // Iterate through the objects in the scene to find instances with the attribute
                foreach (var obj in FindObjectsOfType(fieldInfo.DeclaringType))
                {
                    object value = fieldInfo.GetValue(obj);
                    if (value != null)
                    {
                        string valueText = value.ToString();
                        debugField.TextMeshPro.text = debugField.Attribute.DebugName + " - " + valueText;
                        break; // Assuming you want to update the first instance found
                    }
                }
            }
        }
    }
}
