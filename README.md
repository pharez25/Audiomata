# Audiomata
A Unity developer tool that automates much of the Audio system in unity to allow various combinations of effects, filters and tracks in a procedural like manner, to be published on the Asset store and completely open source

# The system should allow:
- Events that can change or play tracks
- Events that apply audio filters such as high pass or band pass e.g. These must be applied to any designated channel
- A system that can combine samples into tracks using controlled randomisation whereby some combinations can be made disallowed
- A robust UI that allows these systems to be managed from one place.
- Events that change individual audio sources 
- audio should be able to sync up with a beat or be played asynchronously
- The ability to revert back to the raw audio signals
- The ability to use Unity Audio Snapshots in order to apply effects to channels
- The ability to tag audio tracks and use tags with Boolean operators to call upon tracks with the event system e.g. I want a track that is tagged "Dramatic" AND "Uplifting" OR "Energetic"


# Stretch goals:
- A system that allows players to import thier own audio into the game

The system should be created using an Scrum methodology with Test driven development in order to ensure that the code is curated well enough to go onto the store.

# Current Risks to this project include:
- Not being able to deploy on the Asset store as the system developed could be too buggy and all projects submitted to the Asset store go through a scrutiny process.
- The project might not add enough to the Unity Audio system to be considered a viable asset by developers.
- Optimisation could be a problem if there are too many audio signals being managed.
- The project could be too complex to be user friendly as the Unity Audio system is very comprehensive
- Unity's Editor UI systems is very rudimentary therefore, the code for the UI system may be hard to manage.
- Randomly putting together samples to make tracks might not work at all if the music isn't in the same key or time signature,
users may mistakenly think this to be possible. Even with these two things being the same melodic tracks may not work together well


# Audio Credits For Sample Project
Below are the credits for the audio that has been used in sample project courtesy of freesound.org and all the nice people on reddit who really saved my bacon on this one!
Where the audio is not credited, it is my audio. The credits below are sources only and therfore the authors of these samples do not endorse this project in any way. Where the names are in "quotes" it indicates a username.
- "Bassy Explosion" - https://freesound.org/people/Dasgoat/sounds/361592/ by "Dogat"
- "Rock 808 beat" - https://freesound.org/people/PlanetroniK/sounds/425556/ by "PlanetroniK"
- Footstep sounds with "Nox_Sound fs" were sampled from one track "Footsteps_Walk" by "Nox_Sound" (wow who'd a thought) - https://freesound.org/people/Nox_Sound/sounds/490951/
- "Waves Up Close 2" - https://freesound.org/people/amholma/sounds/376804/ by "amholma"
- "geese chattering" - https://freesound.org/people/straget/sounds/424093/ by "straget"
-"birds-01" - https://freesound.org/people/Anthousai/sounds/398736/ by "Anthousai"




# This software (only) upon publication (from when it is publically listed) on Github is under the CC0 1 Universal license, this means that copyright has been wavered and the software is free to use in virtually any ethical manner both commercial and non commercial without attribution. For more information [click here](https://creativecommons.org/publicdomain/zero/1.0/?ref=chooser-v1).

The sound in this project is for demoing as it has been put together very quickly by a software engineer, not an audio technician. **Some of the sounds and all other non-software assets are third party and as such, may fall under different licenses I advise that the links are used with due dilligence (check the licenses on the respective sites dummy) before adding them into your own project and you do not do so without permission of respective authors**. This sound assets authored by me are also under CC0 1, they are cleary indicated with the "Contributing artists" metedata on the files. Also, they are pretty bad so I can't actually see a reason to use them but eh, you do you.

System Instructions (and Quirks):
Essentially you:
-Give audio clips tags and search for them using C#-like syntax 
-Make events that can modify any audio component
- Use Unity's Snapshots as well if you feel like it
- Reverse any effect on any system. Note that it will not be able to revert backwards so a clip being modified by a channel effect cannot be unmodified on the clip, you must unmodify the channel
- Query tags in game (really slow) or pre query the, out side (As fast as can be).
- 

The workflow is normally going to be like this:
Add as many tracks as you like to assets.
Then go on to fix 

Note that the operators go left to right. Queries can be any length, but they will take longer, each one is like going through the whole list once. You can add custom operations and incorrect queries will crash (using dictionaries and thought people might wanna catch it themselves). A query has the following rule:
-any number of prefix operations (e.g. NOT).
-one postfix operation must be inbetween 2 tags. 
- brackets are good.
- nested brackets are good as well.
- the tag must exist in the current instance to work, even with NOT.

Tags have the following rules:
- Cannot have spaces on either side nor double spaces in the middle (these are removed automatically).
- Only numbers, letters, underscores and spaces are allowed.
- can have any valid character from 1 length.
- allowing spaces will likely be removed in future (they truly are awful to deal with genreally)

Operations (Yup you can use Linq to add your own and it was not very nice to program but I made it easy for you : ) ):
- the operations under the hood use lists of guids (basically ID strings for each track), 
- Prefix only get parsed the rhs as they are meant for single tag set operations the same applies to brackets as they don't implicityl have another side
- Postfix is for 2 tags and cannot be used in any other way but in between them
- A group will do everything between it's start and it's end character, then it will perform it's own operation after wards and return.
- Operation Prescedence goes groups>prefix>postfix and from right to left of the string. Right to left shouldn't affect results, but then again a lot of shouldn'ts can never be accounted for.
 you get parsed in by default the lhs (except for prefix) set of tags, the right hand side set of tags and the instance to the qm that called the operation
- where an operation cannot be done or yields no results returning an empty list of strings is the most stable solution. **Null may cause a crash** .
- Finally You may completely overwrite the operation dictionary with your own one or, parse values to be added
