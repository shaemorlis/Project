
create table UserRole (RoleID int not null primary key identity(1,1), 
RoleName varchar(50), RoleDescription varchar(100)
);

create table Users (UserID int not null primary key identity(1,1), age int, DOB date, UserName varchar(50), email varchar(50) 
, FirstName varchar(50), LastName varchar(50), UserPassword varchar(50)
, RoleID  int not null foreign key references UserRole(RoleID) on delete cascade 
);

Create table UserAdmin(AdminID int not null primary key  identity(1,1),

UserID int not null foreign key references Users(UserID) on delete cascade );

Create table UserGamer(GamerID int not null primary key  identity(1,1),

UserID int not null foreign key references Users(UserID) on delete cascade );

Create table Forum (ForumID int not null primary key identity(1,1), Title varchar(50) not null,
Content varchar(50), AddedDate date, addedBy int not null foreign key references Users(UserID) on delete cascade
);

create table Reply (ReplyID int not null primary key identity(1,1),
ReplyContent varchar(100)
, ForumID int not null foreign key references Forum(ForumID)
, ReplyBy int not null foreign key references Users(UserID)
, ReplyDate date
);


Create table Category(CategoryId int not null primary key identity(1,1)
, CategoryName varchar (50) not null
, CategoryAddedBy int not null foreign key references Users(UserID)
);

Create table Transaction( TransactionID int not null primary key identity (1,1)
, TransactionDate date
, TransactionResponse varchar(50)
, TransactionRequestedBy int not null foreign key references UserGamer(GamerID)
, TransactionRequestedTo int not null foreign key references UserGamer(GamerID)
);


Create table GameCode(GameCodeID int not null primary key identity(1,1)
, GameCodeImage varchar(200)
, GameCodeTitle varchar(200)
, GameCodeDescription varchar(200)
, GameCodePrice float
, GameCodeDiscount float
, GameCodeAddedDate date
, GameCodeAddedBy int not null foreign key references UserGamer(GamerID)

);
Create table Cart (CartID int not null primary key identity(1,1)
, CreatedAt date
, Quantity int
, TotalPrice float
, CreatedBy int not null foreign key references UserGamer(GamerID)
);

Create table CartGameCode( CartGameCodeID int not null primary key identity(1,1)
, GameCode int not null  foreign key references GameCode(GameCodeID)
, CartID int not null  foreign key references Cart(CartID)
);

Create table UserMessages(MessageID int not null primary key identity(1,1),

MessageContent varchar(200)
, MessageDate date
, SendTo int not null  foreign key references Users(UserID)
, SendBy int not null  foreign key references Users(UserID)
);
