--------------------------------------------------------
--  Create all Tables
--------------------------------------------------------


CREATE TABLE [dbo].[Users]
(
    [user_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(35) NOT NULL,
    [Surname] NVARCHAR(35) NOT NULL,
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Email] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(50) NOT NULL,
    [Role_id] INT NOT NULL
);

CREATE TABLE [dbo].[Resources]
(
    [resource_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [user_id] INT NOT NULL,
    [module_id] INT NOT NULL,
    [title] NVARCHAR(100) NOT NULL,
    [description] NVARCHAR(MAX) NOT NULL,
    [upload_date] DATETIME NOT NULL
);

CREATE TABLE [dbo].[Files]
(
    [file_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [resource_id] INT NOT NULL,
    [file_path] NVARCHAR(500) NOT NULL,
    [file_type] NVARCHAR(20) NOT NULL,
    [file_size] INT NOT NULL
);

CREATE TABLE [dbo].[Reviews]
(
    [review_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [resource_id] INT NOT NULL,
    [user_id] INT NOT NULL,
    [rating] INT NOT NULL,
    [comment] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [dbo].[Announcements]
(
    [announcement_id] INT   IDENTITY (1, 1) NOT NULL,
    [user_id] INT NOT NULL,
    [module_id] INT NOT NULL,
    [title] NVARCHAR(100) NOT NULL,
    [content] NVARCHAR(MAX) NOT NULL,
    [created_at] DATETIME NOT NULL
);

CREATE TABLE [dbo].[FAQs]
(
    [faq_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [module_id] INT NOT NULL,
    [user_id] INT NOT NULL,
    [created_at] DATETIME NOT NULL,
    [question] NVARCHAR(MAX) NOT NULL,
    [answer] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [dbo].[Modules]
(
    [module_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [title] NVARCHAR(100) NOT NULL,
    [description] NVARCHAR(MAX) NOT NULL,
    [created_at] DATETIME NOT NULL,
    [educator_id] INT NOT NULL
);

CREATE TABLE [dbo].[Gradebook]
(
    [gradebook_id]INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [module_id] INT NOT NULL,
    [user_id] INT NOT NULL,
    [grade] FLOAT NOT NULL,
    [comments] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [dbo].[Quizzes]
(
    [quiz_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [module_id] INT NOT NULL,
    [duration] INT NOT NULL,
    [title] NVARCHAR(50) NOT NULL,
    [created_at] DATETIME NOT NULL,
    [due_date] DATETIME NOT NULL
);

/*Need to accomodate the different question types and also store the options with the Multiple Choice Questions.*/

CREATE TABLE [dbo].[MCQuestions]
(
    [mcq_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [quiz_id] INT NOT NULL,
    [question_text] NVARCHAR(MAX) NOT NULL,
    [correct_answer] NVARCHAR(100) NOT NULL,
    [option_a] NVARCHAR(100) NOT NULL,
    [option_b] NVARCHAR(100) NOT NULL,
    [option_c] NVARCHAR(100) NOT NULL,
    [option_d] NVARCHAR(100) NOT NULL,
    [points] INT NOT NULL
);

/*Long Form Questions are difficult to mark automatically, thus it'll be evaluated manually.*/

CREATE TABLE [dbo].[LFQuestions]
(
    [mcq_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [quiz_id] INT NOT NULL,
    [question_text] NVARCHAR(MAX) NOT NULL,
    [points] INT NOT NULL
);

/*Working with the assumption that Fill in the Blank questions will only have one fill space.*/

CREATE TABLE [dbo].[FBQuestions]
(
    [mcq_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [quiz_id] INT NOT NULL,
    [question_text] NVARCHAR(MAX) NOT NULL,
    [correct_answer] NVARCHAR(100) NOT NULL,
    [points] INT NOT NULL
);

CREATE TABLE [dbo].[QuizResponses]
(
    [response_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [user_id] INT NOT NULL,
    [question_id] INT NOT NULL,
    [selected_answer] NVARCHAR(MAX) NOT NULL,
    [submitted_at] DATETIME NOT NULL
);

CREATE TABLE [dbo].[UserSessions]
(
    [session_id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [user_id] INT NOT NULL,
    [session_token] NVARCHAR(255) NOT NULL,
    [created_at] DATETIME NOT NULL,
    [expires_at] DATETIME NOT NULL
);

CREATE TABLE [dbo].[Roles]
(
    [role_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [role_name] NVARCHAR(50) NOT NULL,
    [role_desc] NVARCHAR(150) NOT NULL
);

CREATE TABLE [dbo].[Stu_To_Module]
(
    [stu_to_mod_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [module_id] INT NOT NULL,
    [user_id] INT NOT NULL
);

CREATE TABLE [dbo].[DocStatuses]
(
    [status_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [status_name] NVARCHAR(150) NOT NULL
);

CREATE TABLE [dbo].[Res_to_status]
(
    [res_to_status_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [status_id] INT NOT NULL,
    [resource_id] INT NOT NULL,
    [user_id] INT NOT NULL,
    [feedback] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [dbo].[TimeTable]
(
    [time_table_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [user_id] INT NOT NULL,
    [module_id] INT NOT NULL,
    [day_id] INT NOT NULL,
    [class_start_time] DATETIME NOT NULL,
    [class_end_time] DATETIME NOT NULL
);

CREATE TABLE [dbo].[DaysOfTheWeek]
(
    [day_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [day_name] NVARCHAR(15) NOT NULL
);


--------------------------------------------------------
-- Add foreign key constraints
--------------------------------------------------------

    -- Users foreign keys
ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT FK_Users_Role FOREIGN KEY ([Role_id]) REFERENCES [dbo].[Roles]([role_id]);

    -- Resources foreign keys
ALTER TABLE [dbo].[Resources]
    ADD CONSTRAINT FK_Resources_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

ALTER TABLE [dbo].[Resources]
    ADD CONSTRAINT FK_Resources_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

    -- Files foreign keys
ALTER TABLE [dbo].[Files]
    ADD CONSTRAINT FK_Files_Resources FOREIGN KEY ([resource_id]) REFERENCES [dbo].[Resources]([resource_id]);

    -- Reviews foreign keys
ALTER TABLE [dbo].[Reviews]
    ADD CONSTRAINT FK_Reviews_Resources FOREIGN KEY ([resource_id]) REFERENCES [dbo].[Resources]([resource_id]);

ALTER TABLE [dbo].[Reviews]
    ADD CONSTRAINT FK_Reviews_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

    -- Announcements foreign keys
ALTER TABLE [dbo].[Announcements]
    ADD CONSTRAINT FK_Announcements_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

ALTER TABLE [dbo].[Announcements]
    ADD CONSTRAINT FK_Announcements_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

    -- FAQs foreign keys
ALTER TABLE [dbo].[FAQs]
    ADD CONSTRAINT FK_FAQs_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

ALTER TABLE [dbo].[FAQs]
    ADD CONSTRAINT FK_FAQs_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

    -- Modules foreign keys
ALTER TABLE [dbo].[Modules]
    ADD CONSTRAINT FK_Modules_Users FOREIGN KEY ([educator_id]) REFERENCES [dbo].[Users]([user_id]);

    -- Gradebook foreign keys
ALTER TABLE [dbo].[Gradebook]
    ADD CONSTRAINT FK_Gradebook_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

ALTER TABLE [dbo].[Gradebook]
    ADD CONSTRAINT FK_Gradebook_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

-- Quizzes foreign keys
ALTER TABLE [dbo].[Quizzes]
    ADD CONSTRAINT FK_Quizzes_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

-- MCQuestions foreign keys
ALTER TABLE [dbo].[MCQuestions]
    ADD CONSTRAINT FK_MCQuestions_Quizzes FOREIGN KEY ([quiz_id]) REFERENCES [dbo].[Quizzes]([quiz_id]);

-- FBQuestions foreign keys
ALTER TABLE [dbo].[FBQuestions]
    ADD CONSTRAINT FK_FBQuestions_Quizzes FOREIGN KEY ([quiz_id]) REFERENCES [dbo].[Quizzes]([quiz_id]);

-- LFQuestions foreign keys
ALTER TABLE [dbo].[LFQuestions]
    ADD CONSTRAINT FK_LFQuestions_Quizzes FOREIGN KEY ([quiz_id]) REFERENCES [dbo].[Quizzes]([quiz_id]);

-- QuizResponses foreign keys
ALTER TABLE [dbo].[QuizResponses]
    ADD CONSTRAINT FK_QuizResponses_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

ALTER TABLE [dbo].[QuizResponses]
    ADD CONSTRAINT FK_QuizResponses_MCQuestions FOREIGN KEY ([question_id]) REFERENCES [dbo].[MCQuestions]([question_id]);

ALTER TABLE [dbo].[QuizResponses]
    ADD CONSTRAINT FK_QuizResponses_FBQuestions FOREIGN KEY ([question_id]) REFERENCES [dbo].[FBQuestions]([question_id]);

ALTER TABLE [dbo].[QuizResponses]
    ADD CONSTRAINT FK_QuizResponses_LFQuestions FOREIGN KEY ([question_id]) REFERENCES [dbo].[LFQuestions]([question_id]);

    -- UserSessions foreign keys
ALTER TABLE [dbo].[UserSessions]
    ADD CONSTRAINT FK_UserSessions_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

    -- Stu_To_Module foreign keys
ALTER TABLE [dbo].[Stu_To_Module]
    ADD CONSTRAINT FK_Stu_To_Module_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

ALTER TABLE [dbo].[Stu_To_Module]
    ADD CONSTRAINT FK_Stu_To_Module_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

    -- Res_to_status foreign keys
ALTER TABLE [dbo].[Res_to_status]
    ADD CONSTRAINT FK_Res_to_status_DocStatuses FOREIGN KEY ([status_id]) REFERENCES [dbo].[DocStatuses]([status_id]);

ALTER TABLE [dbo].[Res_to_status]
    ADD CONSTRAINT FK_Res_to_status_Resources FOREIGN KEY ([resource_id]) REFERENCES [dbo].[Resources]([resource_id]);

ALTER TABLE [dbo].[Res_to_status]
    ADD CONSTRAINT FK_Res_to_status_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

    -- TimeTable foreign keys
ALTER TABLE [dbo].[TimeTable]
    ADD CONSTRAINT FK_TimeTable_Users FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users]([user_id]);

ALTER TABLE [dbo].[TimeTable]
    ADD CONSTRAINT FK_TimeTable_Modules FOREIGN KEY ([module_id]) REFERENCES [dbo].[Modules]([module_id]);

ALTER TABLE [dbo].[TimeTable]
    ADD CONSTRAINT FK_TimeTable_DaysOfTheWeek FOREIGN KEY ([day_id]) REFERENCES [dbo].[DaysOfTheWeek]([day_id]);
