# 🏥 Hospital Management System

A desktop-based Hospital Management System developed using C# and SQL Server.  
The system is designed to manage core hospital operations such as patients, doctors, nurses, medical records, and medicines through a structured Windows Forms application.

![Hospital Management Screenshot](https://github.com/user-attachments/assets/0022742f-4f45-411b-9984-ef1fa7173b7b)

------------------------------------------------------------

## 📌 Project Description

This project is a Windows Forms application connected to a SQL Server database.  
It provides basic CRUD operations (Create, Read, Update, Delete) for managing hospital data in an organized and efficient way.

The system contains 8 Forms, each responsible for a specific module inside the application.

------------------------------------------------------------

## 🛠️ Technologies Used

- Programming Language: C#
- Framework: Windows Forms (.NET)
- Database: Microsoft SQL Server
- IDE: Visual Studio
- Version Control: Git & GitHub

------------------------------------------------------------

## 🧩 System Modules (Forms)

1) Login Form
- Authenticates users
- Validates username and password from the database

2) Main Form
- Main navigation interface
- Provides access to all system modules

3) Patient Form
- Add new patients
- Update patient information
- Delete patient records
- View patient list

4) Doctor Form
- Add new doctors
- Update doctor information
- Delete doctor records
- View doctor list

5) Nurse Form
- Add new nurses
- Update nurse information
- Delete nurse records
- View nurse list

6) Medical Record Form
- Store patient medical history
- Link records to specific patients and doctors

7) Medicine Form
- Add medicines
- Update medicine information
- Delete medicines
- View medicine list

8) Dashboard Form
- Displays total number of:
  • Doctors
  • Patients
  • Nurses

The dashboard provides a simple statistical overview of the hospital data.

------------------------------------------------------------

## 🗄️ Database

The system uses Microsoft SQL Server as the backend database.

Main Tables:
- Patients
- Doctors
- Nurses
- MedicalRecords
- Medicines
- Users (for login authentication)

Make sure to update the connection string according to your SQL Server configuration before running the project.

------------------------------------------------------------

## 🚀 How to Run the Project

1. Clone the repository:
   git clone https://github.com/moka865/Hospital-Management.git

2. Open the solution file in Visual Studio.

3. Create or restore the SQL Server database.

4. Update the connection string in App.config (if needed).

5. Build and run the application.

------------------------------------------------------------
## 📜 License

This project is developed for educational purposes.

