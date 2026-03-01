using UnityEditor;
using UnityEngine;

namespace Mechanics.Config
{
    [CustomPropertyDrawer(typeof(IntProperty))]
    public class IntPropertyDrawer : PropertyDrawer
    {
        private const float LineHeight = 18f;
        private const float Spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty modeProp = property.FindPropertyRelative("_mode");
            SerializedProperty constantProp = property.FindPropertyRelative("_constantValue");
            SerializedProperty minProp = property.FindPropertyRelative("_minValue");
            SerializedProperty maxProp = property.FindPropertyRelative("_maxValue");

            if (modeProp == null || constantProp == null || minProp == null || maxProp == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.EndProperty();
                return;
            }

            Rect lineRect = new Rect(position.x, position.y, position.width, LineHeight);

            Rect labelRect = new Rect(lineRect.x, lineRect.y, EditorGUIUtility.labelWidth, lineRect.height);
            Rect modeRect = new Rect(lineRect.x + EditorGUIUtility.labelWidth, lineRect.y, lineRect.width - EditorGUIUtility.labelWidth, lineRect.height);

            EditorGUI.PrefixLabel(labelRect, label);
            modeProp.enumValueIndex = (int)(IntPropertyMode)EditorGUI.EnumPopup(modeRect, (IntPropertyMode)modeProp.enumValueIndex);

            IntPropertyMode mode = (IntPropertyMode)modeProp.enumValueIndex;
            float y = lineRect.y + LineHeight + Spacing;

            if (mode == IntPropertyMode.Constant)
            {
                Rect valueRect = new Rect(position.x + 14f, y, position.width - 14f, LineHeight);
                EditorGUI.PropertyField(valueRect, constantProp, new GUIContent("Value"));
            }
            else
            {
                float halfWidth = (position.width - 14f - Spacing) * 0.5f;
                Rect minRect = new Rect(position.x + 14f, y, halfWidth, LineHeight);
                Rect maxRect = new Rect(position.x + 14f + halfWidth + Spacing, y, halfWidth, LineHeight);
                EditorGUI.PropertyField(minRect, minProp, new GUIContent("Min"));
                EditorGUI.PropertyField(maxRect, maxProp, new GUIContent("Max"));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty modeProp = property.FindPropertyRelative("_mode");
            if (modeProp == null) return LineHeight;
            return LineHeight + Spacing + LineHeight;
        }
    }
}
