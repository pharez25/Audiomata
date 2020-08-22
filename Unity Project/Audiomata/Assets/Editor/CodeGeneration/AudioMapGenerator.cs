using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Audio;
using UnityEngine; 

namespace Audiomata.ComponentMapping
{
    /// <summary>
    /// This is a way of mapping all classes into a command pattern
    /// </summary>
    public static class PropertyMapper
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
                return Application.dataPath + @"Audiomata\GeneratedCode\";
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
            AudioClassData[] classes = GenerateDefaultClassInfo();
            
            foreach(AudioClassData classData in classes)
            {

            }
        }

        public static AudioClassData Generate(Type type)
        {
            
            PropertyInfo[] publicProps = type.GetProperties().
                 Where(prop => prop.GetGetMethod() != null && prop.GetSetMethod() != null).ToArray();

            MethodInfo[] publicMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => char.IsUpper(m.Name[0])).ToArray();

            return new AudioClassData()
            {
                propType = type,
                publicProps = publicProps,
                publicMethods = publicMethods
            };
           
        }

        public struct AudioClassData
        {
            public Type propType;
            public PropertyInfo[] publicProps;
            public MethodInfo[] publicMethods;
        }

    }
}



