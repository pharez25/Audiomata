using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TaggerUI : EditorWindow
{
    TrackTagger tagger;
    VisualElement root;
    int clipSelection = -1;
    string[] clipNames;

    [MenuItem("Window/Audiomata/Tagger")]
    public static void ShowExample()
    {
        TaggerUI wnd = GetWindow<TaggerUI>();
        wnd.titleContent = new GUIContent("Tagger");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
         root = rootVisualElement;
        tagger = new TrackTagger();

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
       

        // Import UXML
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Tagger.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Main.uss");

        RefreshAudioList();
        
    }

    private void RefreshAudioList()
    {
        ScrollView clipScroll = root.Query<ScrollView>("audioClipScrollView");

        clipNames = tagger.GetAudioAssets();

        for (int i = 0; i < clipNames.Length; i++)
        {
            Button clipSelector = new Button();
            clipSelector.AddToClassList((i != clipSelection) ? "clipBtnUnselected" : "clipBtnSelected");
            clipSelector.name = "clipBtn" + i;
            clipSelector.clickable.clickedWithEventInfo += SetSelectedAudioCLip;

            clipSelector.text = clipNames[i];
            clipScroll.Add(clipSelector);
        }
    }

    private void SetSelectedAudioCLip(EventBase obj)
    {
        Button target = (Button)obj.target;

        int newSelection = int.Parse(target.name.Remove(0, 7));
       
        if(newSelection == clipSelection)
        {
            target.RemoveFromClassList("clipBtnSelected");
            target.AddToClassList("clipBtnUnselected");
            clipSelection = -1;
        }
        else
        {
            Button previous = root.Query<Button>("clipBtn" + clipSelection);

            if (previous != null)
            {
                previous.RemoveFromClassList("clipBtnSelected");
                previous.AddToClassList("clipBtnUnselected");
            }

            target.AddToClassList("clipBtnSelected");
            clipSelection = newSelection;
        }
    }


}