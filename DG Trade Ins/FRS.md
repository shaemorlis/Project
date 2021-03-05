## Feature Requirements Specification
#### DG Trade - Ins Application 
#### Shae Morlis

#### 3.2 Functional requirements 
This section includes the requirements that specify all the fundamental actions of the DG Trade-Ins application. 
#### 3.2.1 User Class 1 - The User 
#### 3.2.1.1 Functional requirement 1.1 
ID: FR1 TITLE: Download/Access application DESC: A user shall download the mobile/web application through a Game Store such as Microsoft, Sony, Nintendo on any mobile device. A user can also access the application online. The application shall be free to download. RAT: For a user to download/access the mobile application. DEP: None 
#### 3.2.1.2 Functional requirement 1.2
ID: FR2 TITLE: User registration –Application DESC: Given that a user has downloaded the application, then the user shall register through the application. The user must provide first name, last name, date of birth, username, password, and e-mail address. RAT: For a user to register on the application. DEP: FR1.
#### 3.2.1.3 Functional requirement 1.3 
ID: FR3 TITLE: User log-in - Application DESC: Given that a user has registered, then the user shall log in to the application. The log-in information if saved will be stored on the application and in the future the user should be logged in automatically. RAT: For a user to login on the application. DEP: FR1, FR3. 
#### 3.2.1.4 Functional requirement 1.4 
ID: FR4 TITLE: Retrieve password DESC: Given that a user has registered, then the user should be able to retrieve his/her password by e-mail using the forgot password. RAT: For a user to retrieve his/her password. DEP: FR1.
#### 3.2.1.5 Functional requirement 1.5 
ID: FR5 TITLE: Application - Search DESC: Given that a user is logged in to the application, then the first page that is shown should be the search page. The user should be able to search game titles, according to several search options. The search options are Price, Year, Game Company, Console, Title. There should also be a free-text search option. A user shall select multiple search options in one search. RAT: For a user to search for a game. DEP: FR4.
#### 3.2.1.6 Functional requirement 1.6 
ID: FR6 TITLE: Purchase/Swap/Sell Game Code DESC: A user shall purchase the game they have searched in the database. User shall swap for a game with another user, if they have a game of same value using the gamer portal. User shall sell/return their Code by uploading their game code.  RAT: For a user to purchase/swap a game. DEP: FR1, FR2, FR3.
#### 3.2.1.7 Functional requirement 1.7
ID: FR7 TITLE: Game Code Credit DESC: A user shall receive game credit after selling their game code. Essentially, they are returning the code of that game they have purchased back to the gaming company. That credit can then be used to purchase games in the future by that company.  RAT: For a user to receive credit for their game code. DEP: FR1, FR2, FR3, FR5, FR6.
#### 3.2.1.8 Functional requirement 1.8
ID: FR8 TITLE: Shopping Cart DESC: A user shall checkout a purchase using the shopping cart. Users can upload game code to shopping cart for returns. Any game swap by gamers through the portal will be completed through the shopping cart. RAT: For user to checkout using shopping cart. DEP: FR1, FR2, FR3, FR5, FR6.