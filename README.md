# RaceDay Smart Event Management System

An end-to-end ASP.NET Core web application and RESTful API built for managing race events, participant enrolments, and event logistics.

---

##  Video Demonstration
Watch the full project walkthrough and implementation explanation on YouTube:
**[Watch the Assignment Overview Video]( https://www.youtube.com/@Junior-qt7vc/playlists)**
Part 1 ()
part 2 ()
part 3 ()

---

## Project Overview

This repository contains the complete implementation across all three assignment parts:

###  Part 1: Database Architecture & Core Web Setup
* **SQL Database Implementation**: Initial schema setup, relational table structures for Events, Categories, and Enrolments, and stored procedures/queries.
* **Web Frontend Initialization**: ASP.NET Core MVC project setup with custom UI components, layout structures, and navigation.
* **CI/CD Integration**: Configured GitHub Actions workflow (`validate-part1.yml`) for automated repository checks and basic project validation.

###  Part 2: RESTful API Development & Continuous Integration
* **Backend Web API**: Built ASP.NET Core REST API targeting port `7209` handling event management, registration endpoints, and status responses.
* **Entity Framework Core**: Implemented EF Core DbContext, entity models, and migrations for seamless database persistence.
* **Automated CI Workflow**: Configured GitHub Actions workflow for building and executing unit tests across API controllers and data layers.

###  Part 3: Front-End Integration, Image Uploads & Full Stack Data Flow
* **API Service Layer**: Implemented typed `HttpClient` service (`ApiService.cs`) in the Web client to communicate directly with backend API endpoints.
* **Image Upload Pipeline**: Configured file upload handling (`IFormFile`) to store image assets in `wwwroot/uploads/` and persist relative paths (`BannerImageUrl`) to SQL via the API.
* **Form Binding & UX**: Integrated client and server-side model validation across form submissions (`OrganiserController`), complete with flash notifications and dynamic dashboard rendering.

---

##  Tech Stack & Prerequisites

* **Framework**: .NET 8.0 / ASP.NET Core MVC & Web API
* **ORM**: Entity Framework Core
* **Database**: SQL Server / LocalDB
* **CI/CD**: GitHub Actions
* **Frontend**: Razor Views, HTML5, CSS3, Bootstrap

---

##  Getting Started

### 1. Clone the Repository
```bash
git clone [https://github.com/juniorlovessushi/Data-Base-1.git](https://github.com/juniorlovessushi/Data-Base-1.git)
cd Data-Base-1
