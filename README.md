# Examen Repo
# ACCESS DENIED
[![GitHub release (latest Sable)](https://img.shields.io/github/v/release/T3rabyte/Examen)](https://github.com/T3rabyte/Examen/releases/tag/stable) 
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/T3rabyte/Examen?include_prereleases)](https://github.com/T3rabyte/Examen/releases/latest)
[![GitHub issue (Open issues)](https://img.shields.io/github/issues/T3rabyte/Examen)](https://github.com/T3rabyte/Examen/issues)
<br /> [![GitHub issue (Open issues)](https://img.shields.io/github/last-commit/T3rabyte/Examen)](https://github.com/T3rabyte/Examen/tree/develop)
[![GitHub download (downloads)](https://img.shields.io/github/downloads/T3rabyte/Examen/total)](https://github.com/T3rabyte/Examen/releases/latest)
[![GitHub download (downloads)](https://img.shields.io/github/commit-activity/w/T3rabyte/Examen)](https://github.com/T3rabyte/Examen/releases/latest)

## ACCESS DENIED
Voor de gemeente Amsterdam moest een game gerealiseerd worden die hun medewerkers leert bewust te worden over online privacy waarbij onderwerpen als datalekken en persoonsgegevens terug komen. Na het spelen van de game moeten de spelers meer  weten over deze onderwerpen en hoe ze hier mee om kunnen gaan. Ook moest de game makkelijk bereikbaar zijn voor hun medewerkers.

# Geproduceerde Game Onderdelen

Student, Teun:

 - [Lobby systeem](https://github.com/T3rabyte/Proef-Examen/tree/origin/minigame%232_memory/proef%20proeve/Assets/src/Singing%20Frogs "SingingForgs minigame")
 - [Multiplayer intergratie](https://github.com/T3rabyte/Proef-Examen/tree/origin/minigame%232_memory/proef%20proeve/Assets/src/Singing%20Frogs "SingingForgs minigame")
 - [Input systeem](https://github.com/T3rabyte/Proef-Examen/tree/origin/minigame%232_memory/proef%20proeve/Assets/src/memory "SingingForgs minigame")

Student, Miguel:

 - [Voorbeeld](https://google.com/)

# Lobby systeem by Teun

Om er voor te zorgen dat mensen samen kunnen spelen gebruiken we unity's Lobby package en infrastuctuur. De gebruiker wordt anoniem ingelogd wanneer ze op de samen spelen knop drukken. Waarna ze een overzight krijgen van alle open lobby's die ze zouden kunnen joinen. Ook kunnen ze vanuit daar zelf een lobby aanmaken en prive lobby's joinen aan de hand van een code die de beherder van een lobby krijgt. Wanneer ze een lobby aanmaken of joinen kunnen ze een rol kiezen. Ook is de lobby leider in staat mensen te verwijderen uit de lobby. Zodra iedereen in de lobby een rol heeft gekozen kan de lobby leider de game starten en wordt iedereen naar het game level verplaatst.

![alt text](https://cdn.discordapp.com/attachments/1089835395098869822/1110179645845950474/ezgif.com-video-to-gif.gif "Lobby create gif")
![alt text](https://cdn.discordapp.com/attachments/1089835395098869822/1110180506525519912/ezgif.com-video-to-gif_1.gif "Random name gif")
![alt text](https://cdn.discordapp.com/attachments/1089835395098869822/1110188298724442194/ezgif.com-video-to-gif_2.gif "Join Lobby gif")

```mermaid
graph TD;
    start((Game starts up + cinematic gets shown)) --> mainMenu(user presses multiplayer button);
    mainMenu --> roomList(User gets shown panel with lobbys to join);
    roomList --> |User presses back button| mainMenu;
    roomList --> |User presses dice button| name(Sets random player name);
    name --> roomList;
    roomList --> |User presses join by code button| codeJoin{Is the code input filled?};
    codeJoin --> |No| nothingResponse(Nothing happens);
    codeJoin --> |Yes| joinLobby(Player joins lobby of specified lobby);
    roomList --> |User presses join button of a lobby in the lobby list| joinLobby;
    roomList --> |User presses lobby create button| createRoom(user gets a menu to configurate the lobby);
    createRoom --> |User presses back button| roomList;
    createRoom --> |User presses create confitm button| privateRoom{Is private toggled on?};
    privateRoom --> |No| createOpenRoom(creates open lobby and joins the user as host);
    privateRoom --> |Yes| createPrivateRoom(creates private lobby and shows code to share withn others, also joins the user as host);
```

```mermaid
graph TD;
    start((Player joins lobby)) --> |Player presses leave button| mainMenu(user leaves the lobby and goes back to main menu);
    start --> |Player presse one of the buttons to change there role| updateRole(updates the role of the player over the lobby to the prefered role);
    start --> |Lobby host kicks player| removePlayer(removes the player from the lobby and updates that to the side of the kicked player);
    start --> |Player presses start game button| startGame{have all the players selected a role thats not neutral?};
    startGame --> |Yes| gameStarts(game loads);
    startGame --> |No| start;
```

# Multiplayer intergratie by Teun

Voor het oorspronkelijke concept was het plan dat sommige elementen gesynced werden tussen de 2 spelers. waaronder bijvoorbeeld de kantoor medewerkers positie en rotatie zodat ze hacker hem kon zien bewegen wanneer de hacker zijn camera hacked. Of wanneer de hacker een minigame volbracht er een panalty kwam bij de kantoor medewerker in de voor van een popup of een freeze die de kantoor medewerker hindert te winnen. Hiervoor is gekozen voor Unity's Replay.

![alt text](https://cdn.discordapp.com/attachments/417058981526110240/1086290575105466378/SingingFrogs.gif "SingingFrogs gif")

```mermaid
graph TD;
    start((Start)) --> fillDic(Fill dictonary with frogs sprites);
    fillDic --> genOrder(Generate order);
    genOrder --> startMini(Frogs sing order);
    startMini --> userInput[/User repeats order/];
    userInput --> correctOrder{User repeated correct order?};
    correctOrder -->|no| startMini;
    correctOrder -->|yes| complete{Has the user correctly done this 5 times?};
    complete -->|no| genOrder;
    complete -->|yes| finished(Player gets end screen and th eoption to play again);
    finished -->|player wants to play again| empty(reset game);
    empty --> genOrder;
    finished -->|player chooses to return to main screen| end_d((end));
```
## Getting Started
To get started with the Cybersecurity Awareness Game simply click on the "downloads" button above.

## How to Play
ACCESS DENIED is a Unity game that can be played on any modern PC. To play the game, simply navigate to the location you downloaded the game and double click the downloaded ACCESS_DENIED.exe file and follow the on-screen instructions.

The game consists of an office where you play as a worker of the municipality of Amsterdam completing different tasks, each of which presents a different cybersecurity threat. The player must navigate through each Task within the allotted time, avoiding threats and answering questions about cybersecurity best practices. The game is designed to be both educational and fun, with engaging graphics and challenging gameplay.

## Questions/Suggestions/Bug Reports
Please read the [Issues List](https://github.com/T3rabyte/Examen/issues) before suggesting a feature. If you have a question, need troubleshooting help, or want to brainstorm a new feature, please start a [Discussion](https://github.com/T3rabyte/Examen/discussions). If you'd like to suggest a feature or report a reproducible bug, please open an [Issue](https://github.com/T3rabyte/Examen/issues/new) on this repository. View the [contributing page](https://github.com/T3rabyte/Examen/blob/main/CONTRIBUTING.md) for more information.

## Contact
If you have any questions or feedback about the ACCESS DENIED, please contact us at miguelafonso939@gmail.com.
