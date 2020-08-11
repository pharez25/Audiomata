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