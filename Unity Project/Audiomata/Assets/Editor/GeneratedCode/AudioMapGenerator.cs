using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Audio;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Audiomata.ComponentMapping
{
    /// <summary>
    /// This is a way of mapping all classes into a command pattern
    /// </summary>
    [ExecuteInEditMode]
    public static class AudioMapGenerator
    {

        private static readonly Type[] allAudioTypes = new Type[]
        {
           typeof(AudioClip), typeof(AudioSource),typeof(AudioListener),typeof(AudioChorusFilter),typeof(AudioDistortionFilter),
           typeof(AudioEchoFilter), typeof(AudioHighPassFilter), typeof(AudioLowPassFilter),typeof(AudioMixer),typeof(AudioMixerGroup),
          typeof( AudioMixerSnapshot),typeof(AudioReverbFilter),typeof(AudioReverbZone)
        };

        public static string GeneratedCodePath
        {
            get
            {
                return Application.dataPath + @"\Audiomata\GeneratedCode\";
            }
        }

        public static AudioClassData[] GenerateDefaultClassInfo()
        {
            AudioClassData[] outData = new AudioClassData[allAudioTypes.Length];

            for (int i = 0; i < allAudioTypes.Length; i++)
            {
                outData[i] = Generate(allAudioTypes[i]);
            }
            return outData;
        }

        public static void GenerateScripts()
        {
            AudioClassData[] targetComponents = GenerateDefaultClassInfo();

            DirectoryInfo directoryInfo = Directory.CreateDirectory(GeneratedCodePath);

            Debug.Log(directoryInfo.FullName);

            for (int i = 0; i < targetComponents.Length; i++)
            {
                AudioClassData nextComponent = targetComponents[i];
                if (nextComponent.publicProps.Length < 1)
                {
                    Debug.Log("Skipping " + nextComponent.propType.Name + " Because it has no detectable public properties");
                }

                Assets.Editor.GeneratedCode.AudioCommandPatGen template = new Assets.Editor.GeneratedCode.AudioCommandPatGen() { TargetClass = nextComponent };
                string csFile = template.TransformText();
                string fullPath = GeneratedCodePath + template.FileName;
                File.WriteAllText(fullPath, csFile);

            }
            AssetDatabase.Refresh();
        }

        public static AudioClassData Generate(Type type)
        {
            
            PropertyInfo[] publicProps = type.GetProperties().
                 Where(prop => prop.GetGetMethod() != null && prop.GetSetMethod() != null &&
                 !(prop.Name == "name" || prop.Name == "hideFlags" || prop.Name == "tag" )&&
                 prop.GetCustomAttributes<ObsoleteAttribute>().Count()<1).ToArray();

            MethodInfo[] publicMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            List<MethodInfo> filteredMethods = new List<MethodInfo>();

            for (int i = 0; i < publicMethods.Length; i++) 
            {
                MethodInfo nextMethod = publicMethods[i];

                if (char.IsLower(nextMethod.Name[0]))
                {
                    continue;
                }

                if (nextMethod.GetCustomAttributes(false).OfType<ObsoleteAttribute>().Count() > 0)
                {
                    continue;
                }

                filteredMethods.Add(nextMethod);
            }

            for (int i = 0; i < publicProps.Length; i++)
            {
                PropertyInfo nextProperty = publicProps[i];
              
            }

            
            return new AudioClassData()
            {
                propType = type,
                publicProps = publicProps,
                publicMethods = filteredMethods.ToArray()
            };
           
        }
    }

    public struct AudioClassData
    {
        public Type propType;
        public PropertyInfo[] publicProps;
        public MethodInfo[] publicMethods;
    }
}



