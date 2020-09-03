using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Audiomata.ComponentMapping;


namespace Audiomata
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {
        SerializedProperty managerRefClips;
        List<AudioData> unAddedTracks;
        Vector2 sceneTrackLstScroll, unaddedTracksScroll;
        bool showAddableTracks = false, showCurrentRefs = false;
        GUIStyle scrollerStyle;
        private void OnEnable()
        {
            managerRefClips = serializedObject.FindProperty("relevantClips");
            unAddedTracks = new List<AudioData>();
            RefreshTracksToAddList();
            scrollerStyle = new GUIStyle();

            RectOffset ones = new RectOffset(1, 1, 1, 1);

            scrollerStyle.border = ones;
            scrollerStyle.padding = ones;
        }

        private void RefreshTracksToAddList()
        {
            AudioData[] all = MetaDataManager.LoadAllAudioData();
            unAddedTracks = new List<AudioData>();
            unAddedTracks.AddRange(MetaDataManager.LoadAllAudioData());
            unAddedTracks.Sort((a, b) =>  string.Compare(b.clip.name, a.clip.name));
            int refArrySize = managerRefClips.arraySize;

            for (int i = 0; i < managerRefClips.arraySize; i++)
            {
                Object nextObj = managerRefClips.GetArrayElementAtIndex(i).objectReferenceValue;
                AudioData castedObj = (AudioData)nextObj;
                unAddedTracks.Remove(castedObj);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //serializedObject.ApplyModifiedProperties();
            GUILayout.Space(15);

            if (showCurrentRefs)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current Scene Track List");

                if(GUILayout.Button("Hide Scene References"))
                {
                    showCurrentRefs = false;
                }

                EditorGUILayout.EndHorizontal();

               

                if (managerRefClips.arraySize> 0)
                {
                    sceneTrackLstScroll = EditorGUILayout.BeginScrollView(sceneTrackLstScroll, scrollerStyle, GUILayout.MaxHeight(150));
                    for (int i = 0; i < managerRefClips.arraySize; i++)
                    {
                        SerializedProperty audioProp = managerRefClips.GetArrayElementAtIndex(i);
                        Object next = audioProp.objectReferenceValue;
                        AudioData nextAD = (AudioData)next;
                        if (!nextAD)
                        {
                            managerRefClips.DeleteArrayElementAtIndex(i);
                            continue;
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(nextAD.clip.name);

                        if (GUILayout.Button("Remove Track"))
                        {
                            managerRefClips.DeleteArrayElementAtIndex(i);
                            RefreshTracksToAddList();

                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.Space(30);
                    EditorGUILayout.LabelField("(Empty)");
                    EditorGUILayout.Space(30);
                }
                
            }
            else
            {
                if(GUILayout.Button("Show Scene Track References"))
                {
                    showCurrentRefs = true;
                }
            }
            //  return;
            
            GUILayout.Space(40);
            if (showAddableTracks)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add Tracks Menu");
                if (GUILayout.Button("Hide Add Track Menu"))
                {
                    showAddableTracks = false;
                }
                EditorGUILayout.EndHorizontal();

                if (unAddedTracks.Count > 0)
                {
                    unaddedTracksScroll = EditorGUILayout.BeginScrollView(unaddedTracksScroll, scrollerStyle, GUILayout.MaxHeight(150));

                    for (int i = unAddedTracks.Count - 1; i > -1; i--)
                    {
                        AudioData next = unAddedTracks[i];
                        if (!next)
                        {
                            continue;
                        }
                        EditorGUILayout.BeginHorizontal();

                        if (GUILayout.Button(next.clip.name))
                        {
                            unAddedTracks.Remove(next);
                            managerRefClips.InsertArrayElementAtIndex(managerRefClips.arraySize);
                            managerRefClips.GetArrayElementAtIndex(managerRefClips.arraySize - 1).objectReferenceValue = (Object)next;
                            RefreshTracksToAddList();
                        }

                        EditorGUILayout.EndHorizontal();

                    }
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.Space(30);
                    EditorGUILayout.LabelField("(Empty)");
                    EditorGUILayout.Space(30);
                }
               
            }
            else if(GUILayout.Button("Show Add Track Menu"))
            {
                showAddableTracks = true;
            }
            GUILayout.Space(15);
            /*
            if (GUILayout.Button("Generate Command Pattern References"))
            {
                AudioMapGenerator.GenerateScripts();
            }
            */
            serializedObject.ApplyModifiedProperties();
        }

    }
}
