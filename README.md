# Siyafunda

## Introduction
Siyafunda is an online centralized platform designed for educators and learners to facilitate resource sharing and enhance accessibility. The platform enables institutions and educators to register and create an online space where students can access educational resources, tests, assignments, and communication tools.

## Operating Environment
The system is web-based and accessible through any modern web browser on both mobile and desktop platforms.

## User Roles
- **System Admin**: Has unrestricted access to all parts of the system and can manage users and business-related functions.
- **System Developer**: Maintains system functionality and available modules.
- **Moderator**: Assigned to manage files that are uploaded and moderate activity on the site.
- **Educator**: Manages the resources, files, tests, and grading within their allocated section.
- **Student**: Accesses resources, assignments, and communication tools provided by their educators.

## Key Features
- Account Creation & Secure Sign-in
- File Uploading & Storage
- File Moderation and Reporting
- Document Tagging and Searching
- Analytics to Monitor User Engagement
- Document Rating System
- FAQ Page Management

## Technical Overview
### Backend
- **Framework**: ASP.NET
- **Data Storage**: SQL database for user data, file metadata, and analytics.
- **Security**: JWT for authentication, HTTP-only cookies, and encrypted passwords.
- **API**: Handles request behavior and manages complex interactions between the frontend and backend.

### Frontend
- **Framework**: React
- **Responsive Design**: Accessible via desktop and mobile browsers.

### Hosting
The platform is hosted on Microsoft Azure, ensuring scalability and reliability.

### Data Requirements
- **File Storage**: Documents are stored in a file storage system.
- **File Metadata**: Subject, grade, keywords, and ratings are stored in an SQL database.
- **User Data**: Securely stores user roles, login details, and activity logs.

### Constraints
- **Compatibility**: Primarily a web application.
- **Modularity**: The system is designed to be flexible and extendable for future expansion.
- **Open-Source Libraries**: All frameworks and libraries used will be open-source, minimizing costs.
- **Security**: Standard data security practices will be followed.
- **Project Timeline**: The project will be completed by October 16, 2024.
- **Version Control**: Managed with GitHub.

## Setup Instructions for Developers

### Prerequisites
- **Visual Studio 2022** or later
- **SQL Server** (LocalDB or a full SQL Server instance)
- **.NET Framework 4.8.1**
- **Node.js** (for the frontend)
- **Git** (for version control)

### Cloning the Repository
Clone the repository using:
  ```git clone https://github.com/yourusername/Siyafunda.git```

### Backend Setup
1. Open the SiyafundaApplication.sln (solution file) in visual studio 2022
3. Open SQL Server Management Studio and run the necessary SQL scripts to create the database schema (these should be provided in the Scripts folder).
4. Build the solution to restore NuGet packages and ensure everything compiles without errors.
5. Run the application using the built-in Visual Studio web server (IIS Express).
Important to note: The system uses .Net framework version 4.8.1, so make sure that is installed locally beforehand.

## API Integration
### QuickChart Watermark API
Siyafunda integrates with the QuickChart Watermark API to apply watermarks to images uploaded by educators. The following parameters are used in the POST request:

* mainImageUrl: URL of the main image to which the watermark will be applied.
* markImageUrl: URL of the watermark image (e.g., institution logo).
* markRatio: The ratio of the watermark size relative to the main image.
Usage
To apply a watermark during the file upload process, the application makes an HTTP GET request to the QuickChart Watermark API using the image URLs and mark ratio. The resulting watermarked image can then be saved back to the server, replacing the original image.

Example Integration
In the ASP.NET backend, the watermarking process is handled in the UploadFileAsync method, which manages file uploads and interacts with the QuickChart Watermark API.

For instance, the following code demonstrates how the API is called: ```csharp string mainImageUrl = $"/{relativeFilePath}"; string watermarkUrl = $"/Assets/SiyafundaLogo.png"; // Your logo path on the server watermarkedImageUrl = await AddWatermark(mainImageUrl, watermarkUrl, 0.25); ```


# Conclusion
Feel free to reach out with any questions regarding the site!

