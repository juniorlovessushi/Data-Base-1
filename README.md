RaceDay System – Part 1: System Planning and Database
Overview

This repository contains the planning phase for the RaceDay system — a platform for managing events, organisers, participants, categories, and enrolments. Before any application code is written, this repository documents the full system design: the relational database structure, the API endpoint plan, and the SQL script used to build and seed the database.

The goal of this planning phase is to ensure the system is fully thought through — data model, roles, and API structure — before implementation begins in Part 2.

Roles

The system defines two primary user roles:

Organiser – Responsible for creating and managing events, categories, and overseeing participant enrolments.
Participant – Registers for events, enrols in categories, and views their own results and enrolment history.

Role-based access is factored into the API endpoint plan, where each endpoint specifies which role (if any) is required to access it.Entity Relationship Diagram (ERD)

The ERD (docs/ERD (1).pdf) models the full RaceDay data structure, including a minimum of six entities, their attributes, primary/foreign keys, and cardinality between relationships (one-to-many, many-to-many, etc.).

The SQL script in this repository matches the ERD exactly — any deliberate differences are explained below.

No deviations between the ERD and SQL script at this stage.

API Endpoint Plan

The endpoint plan (docs/POEPART 1 endpoint.md) lists every planned API endpoint for the system, including:

HTTP method
Route
Description
Role required
Request body (if any)
Expected response

This plan covers authentication, user profiles, events, categories, event enrolments, and results, and will be closely followed during implementation in Part 2.

SQL Database Script

The SQL script (docs/POE part 1 SQL.sql) was written and tested in SQL Server Management Studio (SSMS) and includes:

CREATE TABLE statements for every entity defined in the ERD
Primary keys, foreign keys, and constraints (NOT NULL, UNIQUE, DEFAULT) where applicable
INSERT statements seeding realistic sample data, including at minimum:
2 Organisers
2 Participants
3 Events, with categories for each event
Sample enrolments

The script runs without errors on a clean SQL Server instance.

CI/CD

This repository uses GitHub Actions to automatically validate the repository structure on every push and pull request to main/master. Continuous Integration (CI) means these checks run automatically as soon as code is pushed, rather than being verified manually — catching missing or misplaced files early.

The workflow (.github/workflows/Data Base 1.yml) checks that:

A /docs folder exists in the repository
The required documentation files are present inside /docs:
ERD (1).pdf
POE part 1 SQL.sql
POEPART 1 endpoint.md
