

using System.Collections.Generic;
using UnityEngine;
namespace Audiomata
{
    /// <summary>
    /// Simple event system, for use of Audiomata systems such as AFX and Queries, get the reference insisde of the class from which it is called
    /// </summary>
    public class EventManager
    {
        Dictionary<string, Event> activeEventDict;

        public EventManager()
        {
            activeEventDict = new Dictionary<string, Event>();
        }

        public void AddEventReference(string id, Event e, bool overwrite = false)
        {
            if (overwrite)
            {
                activeEventDict[id] = e;
                return;
            }

            if(activeEventDict.TryGetValue(id, out Event dictE))
            {
                dictE += e;
                return;
            }
            activeEventDict[id] = e;
        }

        public bool RemoveReference(string id)
        {
            return activeEventDict.Remove(id);
        }

        public bool Unsubscribe(string id, Event toRemove)
        {
            if(activeEventDict.TryGetValue(id, out Event allListeners))
            {
                allListeners -= toRemove;
                return true;
            }
            return false;
        }

        public bool FireEvent(string id, object sender, Object target = null)
        {
            if(activeEventDict.TryGetValue(id,out Event targetEvnt))
            {
                targetEvnt(sender, target);
                return true;
            }
            return false;
        }

    }

    public delegate void Event(object sender, Object target = null);
}