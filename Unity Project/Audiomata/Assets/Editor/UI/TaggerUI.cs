using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TaggerUI : EditorWindow
{
    TrackTagger tagger;
    VisualElement root;

    VisualTreeAsset tagElement;

    int clipSelection = -1;
    Track[] clips;
    string[] clipTags;

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
        string path =
        EditorUtility.SaveFilePanel("Save TrackTags", "Assets", "track tags", "json");

        if (!string.IsNullOrEmpty(path))
        {
            tagger.SaveAll(path);
        }
    }

    private void Load()
    {
        string path = EditorUtility.OpenFilePanel("Load Track Tag JSON", "Assets", "json");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        tagger.Load(path);
        RefreshAudioList();
        RefreshTagList();
    }

    private void AddTag()
    {
        if (clipSelection < 0 || clipSelection >= clips.Length)
        {
            return;
        }
        
        TextField addTagField = root.Query<TextField>("addTagTxtField");
        string text = addTagField.text;

        if (text.Length < 1 && text != "Tag Name")
        {
            return;
        }

        tagger.TagTrack(text, clips[clipSelection].guid);
        RefreshTagList();
    }

    private void RefreshAudioList()
    {
        ScrollView clipScroll = root.Query<ScrollView>("audioClipScrollView");
        
        for (int i = clipScroll.childCount-1; i > -1; i--)
        {
            clipScroll.RemoveAt(i);
        }


        clips = tagger.GetAudioAssets();

        for (int i = 0; i < clips.Length; i++)
        {
            Button clipSelector = new Button();
            clipSelector.AddToClassList((i != clipSelection) ? "clipBtnUnselected" : "clipBtnSelected");
            clipSelector.name = "clipBtn" + i;
            clipSelector.clickable.clickedWithEventInfo += SetSelectedAudioCLip;

            clipSelector.text = clips[i].name;
            clipSelector.tooltip = clips[i].path;
            clipScroll.Add(clipSelector);
        }
    }

    private void RefreshTagList()
    {
        if(clipSelection<0 || clipSelection>= clips.Length)
        {
            return;
        }

        ScrollView tagScroll = root.Query<ScrollView>("tagScrollView");

        for (int i = tagScroll.childCount - 1; i > -1; i--)
        {
            tagScroll.RemoveAt(i);
        }

        clipTags = tagger.GetTags(clips[clipSelection].guid);

        if (clipTags == null)
        {
            return;
        }

        for (int i = 0; i < clipTags.Length; i++)
        {
            TemplateContainer nextTagElement = tagElement.CloneTree();
            Label tagLabel = nextTagElement.Query<Label>("nameLabel");
            Button xBtn = nextTagElement.Query<Button>("removeBtn");
            xBtn.clickable.clickedWithEventInfo += RemoveTag;

            tagLabel.text = clipTags[i];
            tagScroll.Add(nextTagElement);
        }

    }

    private void RemoveTag(EventBase obj)
    {
        Button button = (Button)obj.target;
        Label label = button.parent.Query<Label>("nameLabel");
        tagger.UntagTrack(clips[clipSelection].guid, label.text);
        RefreshTagList();
    }

    private void SetSelectedAudioCLip(EventBase obj)
    {
        Button target = (Button)obj.target;

        int newSelection = int.Parse(target.name.Remove(0, 7));

        if (newSelection == clipSelection)
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
        RefreshTagList();
    }
}