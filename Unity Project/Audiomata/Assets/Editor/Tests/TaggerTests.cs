using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEngine;
//
[TestFixture]
public class TaggerTests
{
    string[] trackNames = { "Bassy Explosion", "CC Rock Beat" };

    [Test]
    public void TestFindAllAssets()
    {
        TrackTagger tagger = new TrackTagger();

        string[] receivedTrackNames = tagger.GetAudioAssets();
        for (int i = 0; i < trackNames.Length; i++)
        {
            Assert.That(receivedTrackNames, Has.Exactly(1).EqualTo(trackNames[i]));
        }
    }

    [Test]
    public void ActiveTracksTest()
    {
        TrackTagger tagger = new TrackTagger();
        string[] receivedTracks = tagger.GetActiveTrackNames();
        for (int i = 0; i < trackNames.Length; i++)
        {
            Assert.That(receivedTracks, Has.Exactly(1).EqualTo(trackNames[i]));
        }
    }

    
    [TestCase("boom", 0)]
    [TestCase("drums", 1)]
    public void AddTrackTest(string tag = "test", int trackNameIdx = 0)
    {
        TrackTagger tagger = new TrackTagger();

        tagger.TagTrack(tag, trackNames[trackNameIdx]);

        Assert.That(tagger.GetTrack(tag, out var track));
        Assert.That(track, Is.EqualTo(trackNames[trackNameIdx]));
    }
    
    [TestCase("epic dramatic classical")]
    public void AddTagTest(string tagsIn)
    {
        TrackTagger tagger = new TrackTagger();
    string[] tags = tagsIn.Split(' ');

        for (int i = 0; i < tags.Length; i++)
        {
            tagger.AddTag(tags[i]);
        }

        string[] resultTags = tagger.GetAllTags();

        Assert.That(resultTags.Length == tags.Length);

        for (int i = 0; i < resultTags.Length; i++)
        {
            Assert.That(resultTags, Has.Exactly(1).EqualTo(tags[i]));
        }
    }

    [TestCase("epic dramatic classical")]
    public void RemoveTag(string tagsIn)
    {
        TrackTagger tagger = new TrackTagger();

        string[] tags = tagsIn.Split(' ');

        for (int i = 0; i < tags.Length; i++)
        {
            tagger.AddTag(tags[i]);
            tagger.RemoveTag(tags[i]);
        }
        tagger.RemoveTag("Fake Tag");

        Assert.That(tagger.GetAllTags().Length == 0);
    }
    
    [TestCase("test.json")]
    public void SaveLoadTest(string fileName)
    {
        TrackTagger tagger = new TrackTagger();

        string path = Application.dataPath + "\\" + fileName;
        string[] tags = { "great", "emotional", "loud", "classical" };

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        tagger.TagTrack(tags[0], trackNames[0]);
        tagger.TagTrack(tags[1], trackNames[0]);
        tagger.TagTrack(tags[2], trackNames[0]);
        tagger.TagTrack(tags[2], trackNames[1]);
        tagger.TagTrack(tags[3], trackNames[1]);
        tagger.TagTrack(tags[1], trackNames[1]);

        tagger.SaveAll(path);

        Assert.That(File.Exists(path));

        tagger.Load(path);

        string[] resultTags = tagger.GetAllTags();
        string[] resultTracks = tagger.GetActiveTrackNames();

        for (int i = 0; i < resultTags.Length; i++)
        {
            Assert.That(resultTags, Has.Exactly(1).EqualTo(tags[i]));
        }

        for (int i = 0; i < resultTracks.Length; i++)
        {
            Assert.That(resultTracks, Has.Exactly(1).EqualTo(trackNames[i]));
        }
    }

    
}
