# Siyafunda
## Introduction
Siyafunda is an online centralized platform designed for educators and learners to facilitate resource sharing and enhance accessibility. The platform enables institutions and educators to register and create an online space where students can access educational resources, tests, assignments, and communication tools. Educators and institutions can customize their experience by selecting only the services they need.

## Operating Environment
The system is web-based and accessible through any modern web browser on both mobile and desktop platforms. Additionally, a lightweight mobile version may be developed for Android and iOS devices.

## User Roles
* System Admin: Has unrestricted access to all parts of the system and can manage users and business-related functions if monetized.
* System Developer: Maintains system functionality and adds new features.
* Moderator: Assigned to an institution to manage site users, access, and resources.
* Educator: Manages the resources, files, tests, and grading within their allocated section.
* Student: Accesses resources, assignments, and communication tools provided by their educators.
## Key Features
* Account Creation & Secure Sign-in
* File Uploading & Storage
* File Moderation and Reporting
* Document Tagging and Searching
* Analytics to Monitor User Engagement
* Document Rating System
* FAQ Page Management
# Technical Overview
## Backend
* Framework: ASP.NET
* Data Storage: SQL database for user data, file metadata, and analytics.
* Security: JWT for authentication, HTTP-only cookies, and encrypted passwords.
* API: Handles request behavior and manages complex interactions between the frontend and backend.
## Frontend
* Framework: React
* Responsive Design: Accessible via desktop and mobile browsers.
## Hosting
The platform is hosted on Microsoft Azure, ensuring scalability and reliability.
## Data Requirements
* File Storage: Documents are stored in a file storage system.
* File Metadata: Subject, grade, keywords, and ratings are stored in an SQL database.
* User Data: Securely stores user roles, login details, and activity logs.
## Constraints
* Compatibility: Primarily a web application.
* Modularity: The system is designed to be flexible and extendable for future expansion.
* Open-Source Libraries: All frameworks and libraries used will be open-source, minimizing costs.
* Security: Standard data security practices will be followed.
* Project Timeline: The project will be completed by October 16, 2024.
* Version Control: Managed with GitHub.
