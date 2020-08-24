using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Audiomata
{
    public class TaggerUI : EditorWindow
    {

        VisualElement root;

        VisualTreeAsset tagElement;

        List<string> allTags;

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
            audioDataDict = new Dictionary<string, AudioData>();
            allTags = new List<string>();

            // VisualElements objects can contain other VisualElement following a tree hierarchy.


            // Import UXML
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI/Tagger.uxml");
            VisualElement tagUI = visualTree.CloneTree();
            root.Add(tagUI);

            tagElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI/Tag.uxml");

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            StyleSheet mainFormSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StyleSheets/Main.uss");
            StyleSheet tagStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StyleSheets/Tag.uss");

            if (mainFormSheet)
            {
                root.styleSheets.Add(mainFormSheet);
            }
            else
            {
                Debug.LogError("Audiomata: Main Tagger form style sheet missing, Fixed path: Assets/StyleSheets/Main.uss ");
            }

            if (tagStyle)
            {
                root.styleSheets.Add(tagStyle);
            }
            else
            {
                Debug.LogError("Audiomata:Tag style sheet missing, Fixed path: Assets/StyleSheets/Tag.uss ");
            }
            CleanMetaData();
            FullRefresh();

            Button addTag = root.Query<Button>("addTagBtn");
            addTag.clickable.clicked += AddTagFromTxtBx;

            Button refreshButton = root.Query<Button>("refreshBtn");
            refreshButton.clickable.clicked += FullRefresh;

            Button cleanButton = root.Query<Button>("cleanBtn");
            cleanButton.clickable.clicked += () => CleanMetaData(true);
        }

        private void FullRefresh()
        {
            RefreshAudioList();
            RefreshTagList();
            QuickAddTagUIRefresh();
        }

        private void CleanMetaData(bool withRefresh = false)
        {
            MetaDataManager.CleanAudioData();
            if (withRefresh)
            {
                FullRefresh();
            }
        }

        private void AddTagFromTxtBx()
        {
            if (clipSelection == null)
            {
                EditorUtility.DisplayDialog("No Track Selected", "Please Select a Track to Tag to it", "Ok");
                return;
            }

            TextField addTagField = root.Query<TextField>("addTagTxtField");
            string tag = addTagField.text;

            if (ValidateTag(ref tag, out string reason))
            {
                AudioData audioData = audioDataDict[clipSelection];

                audioData.tags.Add(tag);
                AddTagToQuickAdd(tag, root.Query<ScrollView>("recentTags"));

                EditorUtility.SetDirty(audioData);

                RefreshTagList();

                //could use event propagation here
                addTagField.SetValueWithoutNotify("");
                addTagField.MarkDirtyRepaint();
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid Tag", reason, "OK");
            }
        }

        private void QuickAddTagUIRefresh()
        {
            ScrollView recentTagRegion = root.Query<ScrollView>("recentTags");

            for (int i = recentTagRegion.childCount - 1; i > -1; i--)
            {
                recentTagRegion.RemoveAt(i);
            }
            allTags.Clear();

            foreach (var kvp in audioDataDict)
            {
                List<string> nextTagSet = kvp.Value.tags;

                foreach (string tag in nextTagSet)
                {
                    AddTagToQuickAdd(tag, recentTagRegion);
                }
            }
        }

        private Button AddTagToQuickAdd(string tag, ScrollView recentTagRegion)
        {
            if (allTags.Contains(tag))
            {
                return null;
            }
            allTags.Add(tag);
            Button tagButton = new Button();
            tagButton.style.maxWidth = 280;
            tagButton.style.minWidth = 120;
            tagButton.style.height = 20;
            tagButton.text = tag;
            tagButton.clickable.clickedWithEventInfo += AddTagByRecents;
            recentTagRegion.Add(tagButton);

            return tagButton;
        }

        private void AddTagByRecents(EventBase btn)
        {
            if (clipSelection == null)
            {
                EditorUtility.DisplayDialog("No Track Selected", "Please Select a Track to Tag to it", "Ok");
                return;
            }

            string tag = ((Button)btn.target).text;
            List<string> targetTags = audioDataDict[clipSelection].tags;
            if (targetTags.Contains(tag))
            {
                EditorUtility.DisplayDialog("Tag Already Added", "This tag is already on the selected clip", "OK");
                return;
            }
            targetTags.Add(tag);
            RefreshTagList();
        }

        private void RefreshAudioList()
        {
            ScrollView clipScroll = root.Query<ScrollView>("audioClipScrollView");

            for (int i = clipScroll.childCount - 1; i > -1; i--)
            {
                clipScroll.RemoveAt(i);
            }

            audioDataDict.Clear();
            AudioData[] allAudio = MetaDataManager.GenerateAndLoadAllAudioData();

            for (int i = 0; i < allAudio.Length; i++)
            {
                //add clip button
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

            QuickAddTagUIRefresh();
        }

        private void RefreshTagList()
        {
            if (clipSelection == null)
            {
                return;
            }

            ScrollView tagScroll = root.Query<ScrollView>("tagScrollView");

            for (int i = tagScroll.childCount - 1; i > -1; i--)
            {
                tagScroll.RemoveAt(i);
            }

            AudioData audioData;

            if (!audioDataDict.TryGetValue(clipSelection, out audioData))
            {
                //this can occur during editor recompiling and building
                return;
            }

            List<string> targetTags = audioData.tags;

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

        /// <summary>
        /// Validates the tag and removes any excess whitespace
        /// </summary>
        /// <param name="tag">The tag to be validated</param>        
        /// <returns>True if the tag is valid for the system</returns>
        public static bool ValidateTag(ref string tag)
        {
            if (tag.Length < 1 || tag == "Tag Name")
            {
                return false;
            }

            tag = tag.Trim();
            Regex validCharsRegex = new Regex(@"[A-Z 0-9 \s \- _]", RegexOptions.IgnoreCase);

            int matchCount = validCharsRegex.Matches(tag).Count;

            if (matchCount != tag.Length)
            {
                return false;
            }
            //regex src: https://regexr.com/38p23
            Regex spaceTrimmer = new Regex(@"(?:\s)\s");
            //removes any excessive whitespace between words
            tag = spaceTrimmer.Replace(tag, " ");
            return true;
        }

        /// <summary>
        /// Validates the tag and removes any excess whitespace
        /// </summary>
        /// <param name="tag">The tag to be validated</param>
        /// <param name="reason">The reason why it was rejected (returns null if the tag is valid)</param>
        /// <returns>True if the tag is valid for the system</returns>
        public static bool ValidateTag(ref string tag, out string reason)
        {
            if (tag.Length < 1 || tag == "Tag Name")
            {
                reason = "Tag Empty or Placeholder";
                return false;
            }
            tag = tag.Trim();
            Regex validCharsRegex = new Regex(@"[A-Z 0-9 \s \- _]", RegexOptions.IgnoreCase);

            int matchCount = validCharsRegex.Matches(tag).Count;
            if (matchCount != tag.Length)
            {
                reason = "Tag contains invalid characters only letters,numbers,spaces,hyphens and underscores are allowed";
                return false;
            }
            tag = tag.Trim();
            //regex src: https://regexr.com/38p23
            Regex spaceTrimmer = new Regex(@"(?:\s)\s");
            //removes any excessive whitespace between words
            tag = spaceTrimmer.Replace(tag, " ");
            reason = null;
            return true;
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
                    if (previous != null)
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
}