using NUnit.Framework;
using System.IO;
using UnityEngine;

//
[TestFixture]
public class TaggerTests
{
    string[] trackNames = { "Bassy Explosion", "CC Rock Beat" };

    [Test]
    public void FindAllAssetsTest()
    {
        TrackTagger tagger = new TrackTagger();

        Track[] receivedTrackNames = tagger.GetAudioAssets();

        for (int i = 0; i < trackNames.Length; i++)
        {
            bool wasFound = false;
            for (int j = 0; j < receivedTrackNames.Length; j++)
            {
                if(receivedTrackNames[j].name == trackNames[i])
                {
                    wasFound = true;
                    break;
                }
            }
            Assert.That(wasFound);
        }

    }

    
    [TestCase("boom")]
    [TestCase("drums")]
    public void TagTrackTest(string tag = "test")
    {
        TrackTagger tagger = new TrackTagger();
        Track[] tracks = tagger.GetAudioAssets();

        int idxToTag = Random.Range(0, tracks.Length - 1);

        tagger.TagTrack(tracks[idxToTag].guid, tag);

        Assert.That(tagger.GetRandomTrack(tag, out var track));
        Assert.That(track.Equals(tracks[idxToTag]));

        tagger.UntagTrack(tracks[idxToTag].guid, tag);

        Assert.That(tagger.GetRandomTrack(tag, out var trackName) == false);
    }
   
    [TestCase("test.json")]
    public void SaveLoadTest(string pathFromApp,int randTagCount =5)
    {
        TrackTagger tagger = new TrackTagger();
        Track[] tracks = tagger.GetAudioAssets();

        string path = Application.dataPath + "\\" + pathFromApp;
        string[] tags = { "great", "emotional", "loud", "classical" };

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        int[,] trackTagIdxs = new int[2, randTagCount];

        for (int i = 0; i < randTagCount; i++)
        {
            int nextTagIdx = Random.Range(0, tags.Length - 1);
            int nexTrackIdx= Random.Range(0, tracks.Length- 1);
            tagger.TagTrack(tracks[nexTrackIdx].guid, tags[nextTagIdx]);
            trackTagIdxs[0, i] = nexTrackIdx;
            trackTagIdxs[1, i] = nextTagIdx;
        }

        tagger.SaveAll(path);

        Assert.That(File.Exists(path));

        tagger.Load(path);

        for (int i = 0; i < randTagCount; i++)
        {
            Track[] nextTracks = tagger.GetTracksTaggedAs(tags[trackTagIdxs[1, i]]);
            CollectionAssert.Contains(nextTracks, tracks[trackTagIdxs[0, i]]);
        }
    }

    [TestCase("happy","sad")]
    public void GetTagsTest(string tag1, string tag2)
    {
        TrackTagger tagger = new TrackTagger();
        Track[] tracks = tagger.GetAudioAssets();
        int idx = Random.Range(0, tracks.Length - 1);
        Track selTrack = tracks[idx];

        tagger.TagTrack(selTrack.guid, tag1);
        tagger.TagTrack(selTrack.guid, tag2);

        string[] outTags = tagger.GetTags(selTrack.guid);

        Assert.That(outTags.Length == 2);
        Assert.That(outTags[0] == tag1 || outTags[0] == tag2);
        Assert.That(outTags[1] == tag1 || outTags[1] == tag2);
    }
}
