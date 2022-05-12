create database Attendence_Monitoring_System

use Attendence_Monitoring_System

create table Roles(
	RoleId int identity(101,1) primary key,
	RoleName varchar(100) not null
)

create table Users(
	UserId int identity(1001,1) primary key,
	Email varchar(100) not null,
	Password varchar(100) not null,
	RoleId int not null References Roles(RoleId)
)

create table Section(
	SectionId int identity(1,1) primary key,
	Description varchar(100) not null
)

create table UserDetails(
	Id int identity(1,1) primary key,
	UserId int not null References Users(UserId),
	KeyName varchar(200) not null,
	Value varchar(400) not null,
	SectionId int References Section(SectionId)
)


create table UserLog(
	Id int identity(1,1) primary key,
	UserId int not null References Users(UserId),
	Time Datetime,
	Status varchar(50)
)

create table AttendenceLog(
	Id int identity(1,1) primary key,
	UserId int not null References Users(UserId),
	Date Datetime,
	TotalHours float
)

Alter table AttendenceLog alter column TotalHours varchar(100);

Alter table Regularization alter column TotalHours varchar(100);

create table Regularization(
	Id int identity(1,1) primary key,
	UserId int not null References Users(UserId),
	InTime DateTime,
	OutTime DateTime,
	TotalHours float,
	Status varchar(100),
	Reason varchar(200)
)



--Data Insertion of User Details
Insert into UserDetails Values(1004, 'Employee Id', '1004',1);
Insert into UserDetails Values(1004, 'Name', 'Prakash Naveen Kale',1);
Insert into UserDetails Values(1004, 'MobileNo', '8976109828',1);
Insert into UserDetails Values(1004, 'Date of Birth', '25/08/1995',1);
Insert into UserDetails Values(1004, 'Gender', 'Male',1);
Insert into UserDetails Values(1004, 'Blood Group', 'O+',1);
Insert into UserDetails Values(1004, 'Address', 'Subhash Nagar, Nigdi, Pune-418290',1);
Insert into UserDetails Values(1004, 'Maritial Status', 'Married',1);
Insert into UserDetails Values(1004, 'Img Path', '~/Images/dhoni.jpg',1);
Insert into UserDetails Values(1004, 'Designation', 'Associate Manager',2);
Insert into UserDetails Values(1004, 'Department', 'Administration',2);
Insert into UserDetails Values(1004, 'Date of Joining', '14/11/2016',2);
Insert into UserDetails Values(1004, 'Fresher', 'No',2);
Insert into UserDetails Values(1004, 'Experince in Months', '60',2);
Insert into UserDetails Values(1004, 'SSC school name', 'St. Vincent Highchool, Pune',3);
Insert into UserDetails Values(1004, 'SSC Year of Completion', '2010',3);
Insert into UserDetails Values(1004, 'SSC Percentage', '79.70',3);
Insert into UserDetails Values(1004, 'HSC school name', 'Gogte-Joglekar College, Pune',3);
Insert into UserDetails Values(1004, 'HSC Year of Completion', '2012',3);
Insert into UserDetails Values(1004, 'HSC Percentage', '68.90',3);
Insert into UserDetails Values(1004, 'Degree college name', 'Pimpri Chinchwad College of Engineering ',3);
Insert into UserDetails Values(1004, 'Degree Branch', 'IT',3);
Insert into UserDetails Values(1004, 'Degree Year of Completion', '2014',3);
Insert into UserDetails Values(1004, 'Degree Percentage', '72',3);


