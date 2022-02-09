# HomeSteaders
 Programming showcase for Micaiah Stevens

HomeSteaders
By Haven Studios
Programmed by Micaiah Stevens
support@havenstudios.net

This programming project which is released on GitHub is a showcase project for Micaiah Stevens.  This is released as is and can be manipulated changed or full access to the source code, with the expressed understanding this concept might be taken further by Micaiah, and all code concepts and ideas are the sole owner of Micaiah.   While using Tutorials and concepts, normally codes everything himself and reengineers it anyways to his preferred standards.

While the code show cases implementing Craig Perko’s code from his YouTube tutorial.  It happened to come out at a similar time.  (This was entitled LandEvents) This starting code was taken further in exploring all the other ways you can use Events.  And is self-Contained.

//https://www.youtube.com/watch?v=7OMrWvXNedw
// Source: Craig Perko

The game was a general concept of using Unity’s UI to facilitate a resource trading game.  Based in large part of the environmental impact of clearing forests for little gain, instead of harvesting them for unlimited resources.

This is a second implementation of a game released on Itch.IO as a game entitled Land-Idle.   A Unity UI implementation of the game called ‘Territory Idle’ as released on Steam.

Land Idle by MikHaven (itch.io)
https://mikhaven.itch.io/land-idle

This game ‘HomeSteaders’ and its code will be on Github, and hopefully Playable on Itch.io as a way to let managers and recruiters see and play with Micaiah Stevens’ code and styles.

I am a C# programmer by trade, and not an artist, but have worked with ALL aspects of Unity and have made a LARGE amount of content and games, including a TON of clones and original work.

This code uses something I call GO’s; GameObjects that facilitate the UI/Unity Events to manipulate a Json class for data.

The Image shown here, and in the files, is the overall game concept.  You are given a set amount of land on a larger board.  With 4 neighbors.   You can see their connecting nodes.   The 5 colors represent the five players contending over the section of land assigned to the main ‘game’ player/user.   The one with the most tiles in the middle.
 

This involves ‘natural’ resources that are generated on the game start.  The user than can clear them for ‘Resources’ that are used to build ‘Buildings’.

Each week the user gets Taxes, uses Resources to feed people who migrate or leave their island based on the previous week resources.

The neighbors are a stub, and only one line of tiles was used in the game as of this writing.  The people are infinite and come from thing air, but larger concepts could be used to build out the project.

When it was revised in design, I found you need a second layer to show yours and theirs.  Which adds in conflict and now 4 corner tiles to block access to the 3 ‘conflict/contested’ tiles between the other users.  (This was pared down to showcase the conflict, and not the large size of the game planned.

Corner tiles are concept tiles that add no value or use but are just strategic points to funnel in conflict.

This uses a Unity Assembly Definition (Haven_HomeSteaders) to contain all the code in a Project.  Which has the namespace of Haven_HomeStead.  This was inked to a Haven Engine DLL but was removed to release online.  The only thing that it used currently was Transforms.Clear method extensions with some others added in for context.

While Micaiah now uses Packages on local code libraries, along with the DLL this was eliminated to make the code cleaner and less portable, but all inclusive.   I have not come up with a way to use Source Control to include external libraries, but instead back up the Source Library files every so often to make sure I have backups.

In general, I tend to backup my code to zip, and store them on an external storage site, such as OneDrive.  This gives me easy backup and redo points, without massive source control ruining my structure and file placements.
