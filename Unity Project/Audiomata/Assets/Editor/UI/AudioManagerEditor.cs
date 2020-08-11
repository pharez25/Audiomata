using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Audiomata
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {
        SerializedProperty managerRefClips;
        List<AudioData> unAddedTracks;
        Vector2 sceneTrackLstScroll, unaddedTracksScroll;
        bool showAddableTracks = false;
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
            AudioData[] all = AssetImporter.LoadAllAudioData();
            unAddedTracks = new List<AudioData>();
            unAddedTracks.AddRange(AssetImporter.LoadAllAudioData());
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

          //  return;
            
            EditorGUILayout.LabelField("Current Scene Track List");
            sceneTrackLstScroll = EditorGUILayout.BeginScrollView(sceneTrackLstScroll,scrollerStyle, GUILayout.MaxHeight(150));

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

                if (GUILayout.Button("X"))
                {
                    managerRefClips.DeleteArrayElementAtIndex(i);
                    RefreshTracksToAddList();
                    
                    
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            GUILayout.Space(40);
            if (showAddableTracks)
            {
                EditorGUILayout.LabelField("Add Tracks Menu");
                unaddedTracksScroll = EditorGUILayout.BeginScrollView(unaddedTracksScroll,scrollerStyle, GUILayout.MaxHeight(150));

                for (int i = unAddedTracks.Count-1; i >-1 ; i--)
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

                if (GUILayout.Button("Hide Add Track Menu"))
                {
                    showAddableTracks = false;
                }
            }
            else if(GUILayout.Button("Show Add Track Menu"))
            {
                showAddableTracks = true;
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
