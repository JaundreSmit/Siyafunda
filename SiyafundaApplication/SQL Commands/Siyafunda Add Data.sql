-- Populate DaysOfTheWeek
INSERT INTO [dbo].[DaysOfTheWeek] (day_name)
VALUES 
    ('Monday'),
    ('Tuesday'),
    ('Wednesday'),
    ('Thursday'),
    ('Friday'),
    ('Saturday'),
    ('Sunday');

-- Populate Roles
INSERT INTO [dbo].[Roles] (role_id, role_name, role_desc)
VALUES 
    (2, 'System Admin', 'Unrestricted access to entire system'),
    (3, 'System Developer', 'Maintains the functionality of the system'),
    (4, 'Moderator', 'Responsible for file approval and general site specifics'),
    (6, 'Educator', 'Administrative control over a module'),
    (7, 'Student', 'Access to shared resources and other assigned work');

-- Populate Users
INSERT INTO [dbo].[Users] (Name, Surname, Username, Email, Password, Role_id)
VALUES 
    ('Jaundre', 'Smit', 'JaundreSmit', 'jaundre123.smit@gmail.com', 'Jaundre1', 2),
    ('System', 'Developer', 'SystemDev', 'systemdevexample@gmail.com', 'SystemDev1', 3),
    ('Mod', 'Erator', 'Moderator', 'moderatorexample@gmail.com', 'Moderator1', 4),
    ('Edu', 'Cator', 'Educator', 'educator@gmail.com', 'Educator1', 6),
    ('Stu', 'Dent', 'Student', 'student@gmail.com', 'Student1', 7);

-- Populate DocStatuses
INSERT INTO [dbo].[DocStatuses] (status_name)
VALUES 
    ('Rejected'),
    ('Approved'),
    ('In Progress');


-- Insert Admins (role_id = 2)
INSERT INTO Users (name, surname, username, email, password, role_id) VALUES
('Freddie', 'Mercury', 'FreddieM', 'freddie.mercury@gmail.com', 'Queen123!', 2),
('David', 'Bowie', 'DavidB', 'david.bowie@yahoo.com', 'Starman456!', 2),
('Elton', 'John', 'EltonJ', 'elton.john@gmail.com', 'RocketMan789!', 2),
('Janis', 'Joplin', 'JanisJ', 'janis.joplin@outlook.com', 'PieceOfMyHeart321!', 2);

-- Insert System Developers (role_id = 3)
INSERT INTO Users (name, surname, username, email, password, role_id) VALUES
('Jimi', 'Hendrix', 'JimiH', 'jimi.hendrix@gmail.com', 'PurpleHaze123!', 3),
('Kurt', 'Cobain', 'KurtC', 'kurt.cobain@yahoo.com', 'SmellsLikeTeenSpirit456!', 3),
('Jim', 'Morrison', 'JimM', 'jim.morrison@gmail.com', 'LizardKing789!', 3),
('Janis', 'Joplin', 'JanisJDev', 'janis.joplin.dev@outlook.com', 'MercedesBenz321!', 3),
('Brian', 'May', 'BrianM', 'brian.may@gmail.com', 'GuitarHero123!', 3);

-- Insert Moderators (role_id = 4)
INSERT INTO Users (name, surname, username, email, password, role_id) VALUES
('Mick', 'Jagger', 'MickJ', 'mick.jagger@yahoo.com', 'Satisfaction123!', 4),
('Bon', 'Scott', 'BonS', 'bon.scott@gmail.com', 'HighVoltage456!', 4),
('Axl', 'Rose', 'AxlR', 'axl.rose@outlook.com', 'ParadiseCity789!', 4),
('Steven', 'Tyler', 'StevenT', 'steven.tyler@gmail.com', 'DudeLooksLikeLady321!', 4),
('Bono', 'Vox', 'BonoV', 'bono.vox@yahoo.com', 'OneLove123!', 4);

-- Insert Educators (role_id = 6)
INSERT INTO Users (name, surname, username, email, password, role_id) VALUES
('Liam', 'Gallagher', 'LiamG', 'liam.gallagher@gmail.com', 'Wonderwall123!', 6),
('Noel', 'Gallagher', 'NoelG', 'noel.gallagher@yahoo.com', 'ChampagneSupernova456!', 6),
('Dave', 'Grohl', 'DaveG', 'dave.grohl@gmail.com', 'Everlong789!', 6),
('Chester', 'Bennington', 'ChesterB', 'chester.bennington@outlook.com', 'Numb321!', 6),
('Scott', 'Weiland', 'ScottW', 'scott.weiland@gmail.com', 'Plush123!', 6),
('Kurt', 'CobainEducator', 'KurtCE', 'kurt.cobain.edu@gmail.com', 'Lithium456!', 6),
('Jim', 'MorrisonEducator', 'JimME', 'jim.morrison.edu@yahoo.com', 'RidersOnTheStorm789!', 6),
('James', 'Hetfield', 'JamesH', 'james.hetfield@gmail.com', 'EnterSandman321!', 6),
('Ozzy', 'Osbourne', 'OzzyO', 'ozzy.osbourne@yahoo.com', 'CrazyTrain123!', 6),
('Robert', 'Plant', 'RobertP', 'robert.plant@gmail.com', 'StairwayToHeaven456!', 6);

-- Insert Students (role_id = 7)
INSERT INTO Users (name, surname, username, email, password, role_id) VALUES
('Taylor', 'Swift', 'TaylorS', 'taylor.swift@gmail.com', 'LoveStory123!', 7),
('Katy', 'Perry', 'KatyP', 'katy.perry@yahoo.com', 'Firework456!', 7),
('Bruno', 'Mars', 'BrunoM', 'bruno.mars@gmail.com', 'Grenade789!', 7),
('Adele', 'Adkins', 'AdeleA', 'adele.adkins@gmail.com', 'Hello321!', 7),
('Rihanna', 'Fenty', 'RihannaF', 'rihanna.fenty@yahoo.com', 'Umbrella123!', 7),
('Lady', 'Gaga', 'LadyG', 'lady.gaga@gmail.com', 'PokerFace456!', 7),
('Billie', 'Eilish', 'BillieE', 'billie.eilish@yahoo.com', 'BadGuy789!', 7),
('Justin', 'Bieber', 'JustinB', 'justin.bieber@gmail.com', 'Baby321!', 7),
('Selena', 'Gomez', 'SelenaG', 'selena.gomez@yahoo.com', 'LoseYouToLoveMe123!', 7),
('Shawn', 'Mendes', 'ShawnM', 'shawn.mendes@gmail.com', 'TreatYouBetter456!', 7),
('Ed', 'Sheeran', 'EdS', 'ed.sheeran@yahoo.com', 'ShapeOfYou789!', 7),
('Ariana', 'Grande', 'ArianaG', 'ariana.grande@gmail.com', 'NoTearsLeftToCry123!', 7),
('Halsey', 'Ashley', 'HalseyA', 'halsey.ashley@yahoo.com', 'WithoutMe456!', 7),
('Camila', 'Cabello', 'CamilaC', 'camila.cabello@gmail.com', 'Havana123!', 7),
('Demi', 'Lovato', 'DemiL', 'demi.lovato@yahoo.com', 'Skyscraper456!', 7),
('Dua', 'Lipa', 'DuaL', 'dua.lipa@gmail.com', 'Levitating123!', 7),
('Nicki', 'Minaj', 'NickiM', 'nicki.minaj@yahoo.com', 'SuperBass456!', 7),
('Lizzo', 'Borgia', 'LizzoB', 'lizzo.borgia@gmail.com', 'Juice123!', 7),
('Sam', 'Smith', 'SamS', 'sam.smith@yahoo.com', 'StayWithMe456!', 7),
('Miley', 'Cyrus', 'MileyC', 'miley.cyrus@gmail.com', 'WreckingBall123!', 7),
('Khalid', 'Robinson', 'KhalidR', 'khalid.robinson@yahoo.com', 'Talk123!', 7),
('Post', 'Malone', 'PostM', 'post.malone@gmail.com', 'Rockstar456!', 7),
('Charlie', 'Puth', 'CharlieP', 'charlie.puth@yahoo.com', 'Attention123!', 7),
('Sia', 'Furlan', 'SiaF', 'sia.furlan@gmail.com', 'CheapThrills456!', 7),
('Tones', 'AndI', 'TonesA', 'tones.andI@yahoo.com', 'DanceMonkey123!', 7),
('Olivia', 'Rodrigo', 'OliviaR', 'olivia.rodrigo@gmail.com', 'driverslicense456!', 7),
('Zayn', 'Malik', 'ZaynM', 'zayn.malik@yahoo.com', 'PillowTalk123!', 7),
('Kelsea', 'Ballerini', 'KelseaB', 'kelsea.ballerini@gmail.com', 'HomecomingQueen456!', 7),
('Ava', 'Max', 'AvaM', 'ava.max@yahoo.com', 'SweetButPsycho123!', 7),
('Niall', 'Horan', 'NiallH', 'niall.horan@gmail.com', 'SlowHands456!', 7),
('Shawn', 'Mendes2', 'ShawnM2', 'shawn.mendes2@gmail.com', 'Stitches789!', 7),
('Jason', 'Derulo', 'JasonD', 'jason.derulo@yahoo.com', 'TalkDirty123!', 7),
('Lauv', 'Larson', 'LauvL', 'lauv.larson@gmail.com', 'ILikeMeBetter456!', 7),
('Tori', 'Kelly', 'ToriK', 'tori.kelly@yahoo.com', 'Peace123!', 7),
('Alessia', 'Cara', 'AlessiaC', 'alessia.cara@gmail.com', 'ScarsToYourBeautiful456!', 7);

--Populate modules
INSERT INTO [dbo].[Modules] (title, description, created_at)
VALUES
    ('CMPG121', 'Structured Programming', '2024-09-27 00:00:00'),
    ('ACCC 122', 'Intro to accounting', '2024-03-26 00:00:00'),
    ('ACCE 222', 'Accounting SNR & FET', '2024-02-18 00:00:00'),
    ('ACCF 121', 'Principles of accounting.', '2024-01-06 00:00:00'),
    ('BMAN 221', 'Management principles and practices.', '2024-02-14 00:00:00'),
    ('COMS 321', 'Communication science', '2024-03-14 00:00:00'),
    ('ECON 221', 'Introduction to microeconomics.', '2024-01-23 00:00:00'),
    ('MATH 221', 'Calculus and analytical geometry.', '2024-04-20 00:00:00'),
    ('PSYC 221', 'Fundamentals of psychology.', '2024-01-25 00:00:00'),
    ('BSCI 371', 'Biochemistry of macromolecules.', '2024-05-22 00:00:00'),
    ('ACCE 423', 'High level accounting', '2024-02-07 00:00:00');


