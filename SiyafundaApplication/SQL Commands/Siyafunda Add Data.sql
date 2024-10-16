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
