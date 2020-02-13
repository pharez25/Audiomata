using NUnit.Framework;
using System.Collections;

public class TaggerTests
{
     string[] trackNames = { "Bassy Explosion", "CC Rock Beat" };

    [Test]
    public void TestFindAllAssets()
    {
        Tagger tagger = new Tagger();

        string[] receivedTrackNames = tagger.GetAllAudio();
        for (int i = 0; i < trackNames.Length; i++)
        {
            Assert.That(receivedTrackNames, Has.Exactly(1).EqualTo(trackNames[i]));
        }
    }
}
