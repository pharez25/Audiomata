using System;
[Serializable]
public struct TagData
{
    public TaggedTrack[] taggedTracks;
}


[Serializable]
public struct Track
{
    public string name;
    public string path;
    public string guid;
    public int armIndex;
}

[Serializable]
public struct TaggedTrack
{
    public Track track;
    public string[] tags;
}

