using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// 此类包含用于ReadOnly特性的自定义绘制器
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Unity编辑器中绘制GUI的方法
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="property">属性</param>
        /// <param name="label">标签</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 保存之前的GUI启用状态
            var previousGUIState = GUI.enabled;
            // 禁用属性的编辑功能
            GUI.enabled = false;
            // 绘制属性
            EditorGUI.PropertyField(position, property, label);
            // 恢复之前的GUI启用状态
            GUI.enabled = previousGUIState;
        }
    }
}
