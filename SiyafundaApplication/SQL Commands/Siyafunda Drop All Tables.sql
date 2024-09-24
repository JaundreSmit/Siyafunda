--------------------------------------------------------
-- Drop foreign key constraints
--------------------------------------------------------

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Res_to_status_DocStatuses')
    ALTER TABLE [dbo].[Res_to_status] DROP CONSTRAINT FK_Res_to_status_DocStatuses;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Res_to_status_Resources')
    ALTER TABLE [dbo].[Res_to_status] DROP CONSTRAINT FK_Res_to_status_Resources;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Res_to_status_Users')
    ALTER TABLE [dbo].[Res_to_status] DROP CONSTRAINT FK_Res_to_status_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Stu_To_Module_Modules')
    ALTER TABLE [dbo].[Stu_To_Module] DROP CONSTRAINT FK_Stu_To_Module_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Stu_To_Module_Users')
    ALTER TABLE [dbo].[Stu_To_Module] DROP CONSTRAINT FK_Stu_To_Module_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_TimeTable_Users')
    ALTER TABLE [dbo].[TimeTable] DROP CONSTRAINT FK_TimeTable_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_TimeTable_Modules')
    ALTER TABLE [dbo].[TimeTable] DROP CONSTRAINT FK_TimeTable_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_TimeTable_DaysOfTheWeek')
    ALTER TABLE [dbo].[TimeTable] DROP CONSTRAINT FK_TimeTable_DaysOfTheWeek;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_QuizResponses_Users')
    ALTER TABLE [dbo].[QuizResponses] DROP CONSTRAINT FK_QuizResponses_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_QuizResponses_QuizQuestions')
    ALTER TABLE [dbo].[QuizResponses] DROP CONSTRAINT FK_QuizResponses_QuizQuestions;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_QuizQuestions_Quizzes')
    ALTER TABLE [dbo].[QuizQuestions] DROP CONSTRAINT FK_QuizQuestions_Quizzes;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Quizzes_Modules')
    ALTER TABLE [dbo].[Quizzes] DROP CONSTRAINT FK_Quizzes_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Gradebook_Modules')
    ALTER TABLE [dbo].[Gradebook] DROP CONSTRAINT FK_Gradebook_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Gradebook_Users')
    ALTER TABLE [dbo].[Gradebook] DROP CONSTRAINT FK_Gradebook_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Modules_Users')
    ALTER TABLE [dbo].[Modules] DROP CONSTRAINT FK_Modules_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FAQs_Modules')
    ALTER TABLE [dbo].[FAQs] DROP CONSTRAINT FK_FAQs_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FAQs_Users')
    ALTER TABLE [dbo].[FAQs] DROP CONSTRAINT FK_FAQs_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Announcements_Users')
    ALTER TABLE [dbo].[Announcements] DROP CONSTRAINT FK_Announcements_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Announcements_Modules')
    ALTER TABLE [dbo].[Announcements] DROP CONSTRAINT FK_Announcements_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Resources')
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT FK_Reviews_Resources;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Users')
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT FK_Reviews_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Files_Resources')
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT FK_Files_Resources;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Resources_Users')
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT FK_Resources_Users;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Resources_Modules')
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT FK_Resources_Modules;

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Role')
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT FK_Users_Role;



--------------------------------------------------------
--  Drop all Tables
--------------------------------------------------------
DROP TABLE IF EXISTS [dbo].[Orders];
DROP TABLE IF EXISTS [dbo].[Address];
DROP TABLE IF EXISTS [dbo].[Customers];
DROP TABLE IF EXISTS [dbo].[Parts];
DROP TABLE IF EXISTS [dbo].[Parts_In_Order];
DROP TABLE IF EXISTS [dbo].[Parts_In_Repair];
DROP TABLE IF EXISTS [dbo].[Provinces];
DROP TABLE IF EXISTS [dbo].[Repairs];
DROP TABLE IF EXISTS [dbo].[Status];
DROP TABLE IF EXISTS [dbo].[Technicians];
DROP TABLE IF EXISTS [dbo].[Departments];
DROP TABLE IF EXISTS [dbo].[Users];
DROP TABLE IF EXISTS [dbo].[Resources];
DROP TABLE IF EXISTS [dbo].[Files];
DROP TABLE IF EXISTS [dbo].[Reviews];
DROP TABLE IF EXISTS [dbo].[Announcements];
DROP TABLE IF EXISTS [dbo].[FAQs];
DROP TABLE IF EXISTS [dbo].[Modules];
DROP TABLE IF EXISTS [dbo].[Gradebook];
DROP TABLE IF EXISTS [dbo].[Quizzes];
DROP TABLE IF EXISTS [dbo].[QuizQuestions];
DROP TABLE IF EXISTS [dbo].[QuizResponses];
DROP TABLE IF EXISTS [dbo].[UserSessions];
DROP TABLE IF EXISTS [dbo].[Roles];
DROP TABLE IF EXISTS [dbo].[Stu_To_Module];
DROP TABLE IF EXISTS [dbo].[DocStatuses];
DROP TABLE IF EXISTS [dbo].[Res_to_status];
DROP TABLE IF EXISTS [dbo].[TimeTable];
DROP TABLE IF EXISTS [dbo].[DaysOfTheWeek];
