## "Player Death"
  - Displays Killer and Victim with their Roles, Names, UserIDs, whether the Victim was cuffed or not and the used weapon.
## "Player Join"*
  - Displays Username, UserID, their Ping & IP (/"Do Not Track").
## "Player Leave"*
  - Displays Username, UserID, their Ping & IP (/"Do Not Track").
## "Round Start Spawn"
  - Displays all spawned Roles and the amount of Players as that Role.
## "Console Command"
  - Displays the user's Username and the issued Command.
## "Remote Admin Command"
  - Displays the user's Username and the issued Command.
## "Player Ban"
  - Displays the issuer's Name, UserID, the banned Player's Name, UserID, the Reason, the Duration in Minutes, Hours, Days and Years.  

\* Disclaimer  
The Discord Api has a ratelimit for sending messages of 5/5s, 5 messages per 5 seconds.  
For a server of 30 people, the Discord Bot would queue up all the join / leave messages on round restart and every other message from the bot would queue up, up to 30 seconds.  
So be mindful when activating this.
