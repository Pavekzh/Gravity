using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Services
{
    public class ValuesPanelTemplate : BasicTools.Singleton<ValuesPanelTemplate>
    {
        [Header("Base")]        
        [SerializeField] RectTransform emptyPrefab;
        [SerializeField] TMP_InputField planetNameFieldPrefab;
        [SerializeField] RectTransform shortViewPrefab;

        [Header("Module properties")]        
        [SerializeField] TMP_Text moduleLabelPrefab;
        [SerializeField] TMP_InputField inputFieldPrefab;
        [SerializeField] TMP_Text propertyLabelPrefab;
        [SerializeField] TMP_Text lineLabelPrefab;
        [SerializeField] float startMargin = 10;
        [SerializeField] float propertiesMargin = 10;
        [SerializeField] float lineMargin = 5;
        [SerializeField] float moduleMargin = 20;

        public TMP_InputField PlanetNameFieldPrefab { get => planetNameFieldPrefab; set => planetNameFieldPrefab = value; }
        public TMP_Text PropertyLabelPrefab { get => propertyLabelPrefab; set => propertyLabelPrefab = value; }
        public TMP_Text LineLabelPrefab { get => lineLabelPrefab; set => lineLabelPrefab = value; }
        public float StartMargin { get => startMargin; set => startMargin = value; }
        public float PropertiesMargin { get => propertiesMargin; set => propertiesMargin = value; }
        public float LineMargin { get => lineMargin; set => lineMargin = value; }
        public float ModuleMargin { get => moduleMargin; set => moduleMargin = value; }
        public RectTransform EmptyPrefab { get => emptyPrefab; set => emptyPrefab = value; }
        public TMP_InputField InputFieldPrefab { get => inputFieldPrefab; set => inputFieldPrefab = value; }
        public TMP_Text ModuleLabelPrefab { get => moduleLabelPrefab; set => moduleLabelPrefab = value; }
        public RectTransform ShortViewPrefab { get => shortViewPrefab; set => shortViewPrefab = value; }
    }
}