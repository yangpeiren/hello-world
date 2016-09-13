Create table Work
(WorkSymbol nchar(5),
WorkName nvarchar(41) Not Null,
TeamName nvarchar(15) Not Null,
GroupName nvarchar(15)Not Null,
ShopName nvarchar(10)Not Null,
Primary Key (WorkSymbol));

Create table Checkup
(CheckNO int identity,
Code as right('00000000'+cast(CheckNo as varchar(8)),8),
WorkSymbol nchar(5),
Checker nvarchar(4),
CheckerName nvarchar(20),
CheckerID int,
ID char(1),
Checktime date,
Primary Key (CheckNo),
Foreign Key (WorkSymbol) References Work(WorkSymbol));

create table PDCA
(ProblemNO int identity,
CheckNO varchar(8) not null,
Pname nvarchar(50),
CreatedBy int,
CreatedDate date,
Depart nvarchar(50),
PD nvarchar(MAX),
RC nvarchar(MAX),
CM nvarchar(MAX),
Pstate bit not null default 1,
ClosedBy int,
CloseDate date,
primary key (ProblemNO),
foreign key (CreatedBy) references Management(ID),
foreign key (ClosedBy) references Management(ID));

create table Management
(ID int identity,
SuperID int,
Name nvarchar(20),
Position char(2),
ShopName nvarchar(10),
GroupName nvarchar(15),
TeamName nvarchar(15),
WorkGroup char(1),
UserName nvarchar(50),
Password char(32),
Limit char(4),
Active bit Not Null default 0,
Primary Key (ID));