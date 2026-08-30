# POEPART1DB — API Endpoint Plan

This plan maps each API endpoint to the `POEPART1DB` schema and ERD defined in Sections A and C. Role checks are based on `Roles.RoleId` (`1 = Organiser`, `2 = Participant`), assigned to each `Users` record via `Users.RoleId`.

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | `/api/auth/register` | Registers a new Organiser or Participant account | None (Public) | `{"fullName": "string", "email": "string", "password": "string", "roleId": 1}` | `201 Created` / `400 Bad Request` |
| POST | `/api/auth/login` | Authenticates a user and returns a JWT access token | None (Public) | `{"email": "string", "password": "string"}` | `200 OK` / `401 Unauthorized` |
| GET | `/api/profile` | Retrieves the profile details of the logged-in user | Any (Logged in) | None | `200 OK` / `401 Unauthorized` |
| PUT | `/api/profile` | Updates the profile details of the logged-in user | Any (Logged in) | `{"fullName": "string", "email": "string"}` | `200 OK` / `400 Bad Request` |
| GET | `/api/events` | Retrieves a list of all race events | None (Public) | None | `200 OK` |
| POST | `/api/events` | Creates a new race event | Organiser | `{"eventName": "string", "eventDate": "DateTime", "location": "string"}` | `201 Created` / `403 Forbidden` |
| GET | `/api/events/{id}/categories` | Retrieves all categories for a specific event | None (Public) | None | `200 OK` / `404 Not Found` |
| POST | `/api/events/{id}/categories` | Adds a category to a race event | Organiser | `{"categoryName": "string", "maxParticipants": 100}` | `201 Created` / `403 Forbidden` |
| POST | `/api/enrolments` | Enrols the logged-in participant into an event category | Participant | `{"categoryId": 1}` | `201 Created` / `409 Conflict` |
| GET | `/api/enrolments/my-races` | Retrieves all enrolments for the logged-in participant | Participant | None | `200 OK` |
| POST | `/api/results` | Records finish times and positions for a participant | Organiser | `{"enrolmentId": 1, "finishTime": "03:45:12", "position": 12}` | `201 Created` / `403 Forbidden` |
| GET | `/api/results/event/{id}` | Displays leaderboard results for an event | None (Public) | None | `200 OK` |


- **`/api/auth/register`** inserts into `Users`, using the supplied `roleId` as the FK to `Roles`.
- **`/api/events`** (POST) inserts into `Events`, setting `OrganiserId` to the logged-in user's `UserId`. Requires the caller's `Roles.RoleName = 'Organiser'`.
- **`/api/events/{id}/categories`** (POST) inserts into `Categories`, setting `EventId` from the route parameter.
- **`/api/enrolments`** inserts into `EventEnrolments`, setting `ParticipantId` to the logged-in user's `UserId` and `CategoryId` from the body. The `409 Conflict` response maps to a violation of the `UQ_Participant_Category` unique constraint (a participant can only enrol once per category).
- **`/api/enrolments/my-races`** selects `EventEnrolments` filtered by `ParticipantId`, joined to `Categories` and `Events` for display.
- **`/api/results`** inserts into `Results`, where `EnrolmentId` is a unique FK back to `EventEnrolments` (a `1..1` relationship). Restricted to Organisers.
- **`/api/results/event/{id}`** selects `Results` joined through `EventEnrolments` → `Categories` → `Events`, filtered by `Events.EventId`.
