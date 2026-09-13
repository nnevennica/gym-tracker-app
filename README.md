# GymTracker 🏋️‍♂️

GymTracker is a full-stack web application designed to help users track their workouts, monitor weekly consistency, and gain smart recovery insights based on their training intensity and fatigue ratings.

---

## 🚀 Tech Stack

* **Backend:** ASP.NET Core (.NET), C#, Entity Framework Core, MySQL, MediatR (CQRS pattern)
* **Frontend:** Angular, TypeScript, Bootstrap 5, HTML5, CSS3
* **External Services:** SendGrid API (for welcome email notifications upon user registration)

---

## 🛠️ Key Features

* **User Authentication & Management:** Secure registration and login flow with automated welcome emails via SendGrid.
* **Workout Logging:** Track exercise types, duration, burned calories, intensity ratings, fatigue levels, and custom notes.
* **Progress Tracking & Analytics:** Real-time calculation of weekly statistics, monthly progress metrics, and workout goal completion percentages.
* **Smart Recovery Assistant:** Automated feedback system analyzing weekly average fatigue and intensity to recommend recovery days or optimal training pacing.
* **Concurrency Control:** Utilizes Entity Framework Core Optimistic Concurrency to safely handle simultaneous resource updates.

---

## ⚙️ Getting Started / Local Setup

### Prerequisites

* .NET SDK 8.0+
* Node.js & Angular CLI
* MySQL Database Server
* SendGrid Account & API Key (for email notifications)

### 1. Backend Setup

1. Clone the repository and navigate to the backend folder.
2. Update the connection string in `appsettings.json` to point to your local MySQL database.
3. Configure your **SendGrid API Key** and sender email settings inside `appsettings.json` (or user secrets) so that the welcome email notification service can operate correctly upon registration.

---

## 🎥 Video Presentation

Watch the short video presentation of GymTracker here:

https://youtu.be/B9yLvKt9Ol8