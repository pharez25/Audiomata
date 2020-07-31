using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TaggerUI : EditorWindow
{
  
    VisualElement root;

    VisualTreeAsset tagElement;

    string clipSelection = null;
    Dictionary<string, AudioData> audioDataDict;

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
        

        // VisualElements objects can contain other VisualElement following a tree hierarchy.


        // Import UXML
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI/Tagger.uxml");
        VisualElement tagUI = visualTree.CloneTree();
        root.Add(tagUI);

        tagElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI/Tag.uxml");

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/UI/Main.uss");

        RefreshAudioList();
        Button addTag = root.Query<Button>("addTagBtn");
        addTag.clickable.clicked += AddTag;

        Button saveBtn = root.Query<Button>("saveBtn");
        saveBtn.clickable.clicked += Save;

        Button loadBtn = root.Query<Button>("loadBtn");
        loadBtn.clickable.clicked += Load;
    }

    private void Save()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void Load()
    {
        RefreshAudioList();
        RefreshTagList();
    }

    private void AddTag()
    {
        if (clipSelection == null)
        {
            EditorUtility.DisplayDialog("No Track Selected", "Please Select a Track to Tag to it", "Ok");
            return;
        }
        
        TextField addTagField = root.Query<TextField>("addTagTxtField");
        string text = addTagField.text;

        if (text.Length < 1 || text == "Tag Name")
        {
            return;
        }

        AudioData audioData = audioDataDict[clipSelection];

        audioData.tags.Add(text);

        EditorUtility.SetDirty(audioData);

        RefreshTagList();
    }

    private void RefreshAudioList()
    {
        ScrollView clipScroll = root.Query<ScrollView>("audioClipScrollView");
        
        for (int i = clipScroll.childCount-1; i > -1; i--)
        {
            clipScroll.RemoveAt(i);
        }
        audioDataDict = new Dictionary<string, AudioData>();
        AudioData[] allAudio = AssetImporter.GenerateAndLoadAllAudioData();

        for (int i = 0; i < allAudio.Length; i++)
        {
            Button clipSelector = new Button();
            AudioData nextTrack = allAudio[i];
            clipSelector.AddToClassList((nextTrack.guid != clipSelection) ? "clipBtnUnselected" : "clipBtnSelected");
            clipSelector.name = "clipBtn" + nextTrack.guid;
            clipSelector.clickable.clickedWithEventInfo += SetSelectedAudioCLip;

           
            audioDataDict.Add(nextTrack.guid, nextTrack);
            clipSelector.text = nextTrack.clip.name;
            clipSelector.tooltip = AssetDatabase.GUIDToAssetPath(nextTrack.guid);
            clipScroll.Add(clipSelector);
        }
    }

    private void RefreshTagList()
    {
        if(clipSelection == null)
        {
            return;
        }

        ScrollView tagScroll = root.Query<ScrollView>("tagScrollView");

        for (int i = tagScroll.childCount - 1; i > -1; i--)
        {
            tagScroll.RemoveAt(i);
        }

        List<string> targetTags = audioDataDict[clipSelection].tags;


        if (targetTags.Count == 0)
        {
            return;
        }

        for (int i = 0; i < targetTags.Count; i++)
        {
            TemplateContainer nextTagElement = tagElement.CloneTree();
            Label tagLabel = nextTagElement.Query<Label>("nameLabel");
            Button xBtn = nextTagElement.Query<Button>("removeBtn");
            xBtn.clickable.clickedWithEventInfo += RemoveTag;

            tagLabel.text = targetTags[i];
            tagScroll.Add(nextTagElement);
        }

    }

    private void RemoveTag(EventBase obj)
    {
        Button button = (Button)obj.target;
        Label label = button.parent.Query<Label>("nameLabel");
        AudioData targetTrack = audioDataDict[clipSelection];
        targetTrack.tags.Remove(label.text);
        EditorUtility.SetDirty(targetTrack);
        RefreshTagList();
    }

    private void SetSelectedAudioCLip(EventBase obj)
    {
        Button target = (Button)obj.target;

        string newSelection = target.name.Substring(7);

        if (newSelection == clipSelection)
        {
            target.RemoveFromClassList("clipBtnSelected");
            target.AddToClassList("clipBtnUnselected");
            clipSelection = null;
        }
        else
        {
            if (clipSelection != null)
            {
                Button previous = root.Query<Button>("clipBtn" + clipSelection);
                if(previous != null)
                {
                    previous.RemoveFromClassList("clipBtnSelected");
                    previous.AddToClassList("clipBtnUnselected");
                }
            }

            target.AddToClassList("clipBtnSelected");
            clipSelection = newSelection;
        }
        RefreshTagList();
    }
}