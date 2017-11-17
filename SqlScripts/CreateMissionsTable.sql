Create Table Missions (
	MissionID int Primary Key,
	MissionName varchar(120),
	FK_Tour int,
	Foreign Key (FK_Tour)
		References Tours(TourID)
);