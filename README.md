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

 - [Lobby systeem](https://github.com/T3rabyte/Examen/tree/1.0/Examen%20Gemeente%20Amsterdam/Assets/Scripts/Menu "Lobby systeem")
 - [Multiplayer intergratie](https://github.com/T3rabyte/Examen/tree/1.0/Examen%20Gemeente%20Amsterdam/Assets/Scripts/Managers/Networking "SingingForgs minigame")
 - [Input systeem](https://github.com/T3rabyte/Examen/tree/1.0/Examen%20Gemeente%20Amsterdam/Assets/Scripts/Controllers "Input systeem")
 

Student, Miguel:

 - [UI implementation](https://github.com/T3rabyte/Examen/tree/US%239_SocialMedia)
 - [social media system](https://github.com/T3rabyte/Examen/tree/US%239_SocialMedia)
 - [quiz system](https://github.com/T3rabyte/Examen/tree/US%2320_mails)
 - [JSON integration](https://github.com/T3rabyte/Examen/tree/US%2320_mails)
 - [Mail system](https://github.com/T3rabyte/Examen/tree/US%2320_mails)
 - [calling for tips mechanic](https://github.com/T3rabyte/Examen/tree/US%236_Tips)
 - [Antivirus installer](https://github.com/T3rabyte/Examen/tree/US%238_AntiVirus)
 - [hacks](https://github.com/T3rabyte/Examen/tree/US%231_PopupAttack)

# Lobby systeem

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
    codeJoin --> |Yes| joinLobby(Player joins chosen lobby);
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
    start --> |Player presses one of the buttons to change there role| updateRole(updates the role of the player over the lobby to the prefered role);
    start --> |Lobby host kicks player| removePlayer(removes the player from the lobby and updates that to the side of the kicked player);
    start --> |Player presses start game button| startGame{have all the players selected a role thats not neutral?};
    startGame --> |Yes| gameStarts(game loads);
    startGame --> |No| start;
```
```mermaid
---
title: Lobby systeem class diagram
---
classDiagram
    MainMenuUI <|-- LobbyManager
    LobbyManager <|-- MainMenuUI
    MainMenuUI <|-- RoomManager
    LobbyManager <|-- RoomManager
    RoomManager <|-- LobbyManager
    class MainMenuUI{
        -GameObject lobbyPanel
        -GameObject lobbyViewContent
        -GameObject lobbyListObj
        -List<GameObject> panelList
        -List<GameObject> lobbyListItems
        +List<GameObject> roomListItems
        -Lobbymanager lobbymanager
        -InstantiateLobbyList()
        +DestroyItemsOnLists(List<List<GameObjects>> lists)
        +ShowPanel(string panelName)
        +ShowError(string message)
        +QuitGame()
    }
    class LobbyManager{
        +Lobby joinedLobby
        -MainMenuUI mainMenuUI
        -RoomManager roomManager
        -RelayManager relayManager
        -UnityAction leaveFromLobbyAction
        -TMP_InputField playerNameInput
        -TMP_InputField quickJoinLobbyName
        -TMP_InputField lobbyCreateName
        -Toggle isLobbyPrivate
        -Button roomBackBtn
        -Button roomStartBtn
        -List<string> randomPlayerNames
        -InitializeUnityService()
        -SetRandomPlayerName()
        +CreateLobby()
        -NewPlayer(string playerRole) Player
        +JoinLobbyById(string lobbyId)
        +SetLobbyleaveButton()
        +JoinLobbyByCode()
        -LobbyHeartbeat()
        -PollForLobbyUpdates()
        +ResetRoom()
        +GetLobbyList() List<Lobby>
        +LeaveFromLobby(string lobbyId)
        +RemoveFromLobby(string lobbyId, GameObject player) 
    }
    class RoomManager{
        -GameObject roomPlayerObj
        -GameObject roomCodeObj
        +List<GameObject> roomContentList
        -MainMenuUI mainMenuUI
        -LobbyManager lobbyManager
        -RelayManager relaymanager
        -List<Player> joinedPlayerList
        +InstantiateRoomItems(Lobby lobby)
        +UpdatePlayerRole(string newRole) 
        +UpdateRolesInLobby(Lobby joinedLobby) 
        +StartGame()
    }
    class RoleManager{
        +string role
        -OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    }
```
# Multiplayer intergratie

Voor het oorspronkelijke concept was het plan dat sommige elementen gesynced werden tussen de 2 spelers. waaronder bijvoorbeeld de kantoor medewerkers positie en rotatie zodat ze hacker hem kon zien bewegen wanneer de hacker zijn camera hacked. Of wanneer de hacker een minigame volbracht er een panalty kwam bij de kantoor medewerker in de voor van een popup of een freeze die de kantoor medewerker hindert te winnen. Hiervoor is gekozen voor Unity's Relay.

![alt text](https://cdn.discordapp.com/attachments/417058981526110240/1110909144820629636/ezgif.com-video-to-gif.gif "Multiplayer gif")

# Input systeem

Om het tijdens de game makkelijk te maken voor de spelers om te zien wat er op de monitoren gebeurt is er een script gemaakt dat de camera van de speler op een locatie zet die dichter bij het scherm staat. Later werdt dit script omgebouwd zodat er ook een audio object aangeroepen kan worden.

![alt text](https://cdn.discordapp.com/attachments/417058981526110240/1110914969324769300/ezgif.com-video-to-gif_1.gif "Input Systeem gif")

```mermaid
graph TD;
    start((Player presses mouse button)) --> buttonType{What mouse button did the user press?}
    buttonType --> |Left mouse button| check{Does the object the player aims at have a collider and one of the input tags};
    check --> |No| nothing(Nothing happens);
    check --> |Yes| type{What is the tag of the object?};
    type --> |Camera| camera(Sets the camera position to the position of the camera position child of the object);
    type --> |Audio| audio(Retrieves a random audio file from the audio clip list of the object);
    audio --> playAudio(plays the chosen audio from the object chosen);
    buttonType --> |Right mouse button| checkInObject{Is the camera in a objects camera position?};
    checkInObject --> |Yes| returnCam(Returns the player camera to the player model);
    checkInObject --> |no| nothingCam(Nothing happens);
```
# Player controller

De player controller is een cruciaal onderdeel van onze game ervaring. Het stelt spelers in staat om vloeiend door de virtuele wereld te navigeren. Met de controller kunnen spelers lopen, rennen, springen in de game. Dankzij de aanpasbaarheid van de controller is het makkelijk om eigenshappen als de snelheid of de speler kan bewegen of roteren.

## Getting Started
To get started with the Cybersecurity Awareness Game simply click on the "downloads" button above.

## How to Play
ACCESS DENIED is a Unity game that can be played on any modern PC. To play the game, simply navigate to the location you downloaded the game and double click the downloaded ACCESS_DENIED.exe file and follow the on-screen instructions.

The game consists of an office where you play as a worker of the municipality of Amsterdam completing different tasks, each of which presents a different cybersecurity threat. The player must navigate through each Task within the allotted time, avoiding threats and answering questions about cybersecurity best practices. The game is designed to be both educational and fun, with engaging graphics and challenging gameplay.

## Questions/Suggestions/Bug Reports
Please read the [Issues List](https://github.com/T3rabyte/Examen/issues) before suggesting a feature. If you have a question, need troubleshooting help, or want to brainstorm a new feature, please start a [Discussion](https://github.com/T3rabyte/Examen/discussions). If you'd like to suggest a feature or report a reproducible bug, please open an [Issue](https://github.com/T3rabyte/Examen/issues/new) on this repository. View the [contributing page](https://github.com/T3rabyte/Examen/blob/main/CONTRIBUTING.md) for more information.

## Contact
If you have any questions or feedback about the ACCESS DENIED, please contact us at miguelafonso939@gmail.com.
