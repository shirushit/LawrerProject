# Changelog

## v1.1 — Review & refactor (2026)

A full code review of the original 2024 final project, followed by fixes. The original code is
kept as the first commit of this repository so every change can be compared.

### Fixed — crashes and broken features
- **Infinite recursion:** `getCustomerNegdi()` / `getOrechDinNegdi()` called themselves, so opening
  an existing case crashed with a `StackOverflowException`.
- **Login screen:** the login button had no click handler, and the code read the *add-user*
  textboxes instead of the login ones. The app skipped login entirely and opened the home screen.
- **Adding a customer always failed:** `City`, `House` and the person type are required by the
  database but had no input fields, and the save button was hidden.
- **Opening a case:** the form fields were never copied into the entity, the step timeline was never
  created, and the update path compared `"UpDate"` to `"Update"`, so edits were never saved.
- **Completing a step:** `Convert.ToInt32(SubItem)` inside a LINQ-to-Entities query threw at runtime;
  the list had no checkboxes, so `CheckedItems` was always empty. The following deadlines were
  never updated.
- **Payments:** a loop ran over the processes list while indexing the payments list
  (`IndexOutOfRangeException`). A payment can no longer exceed the open balance.
- **Receipt:** the receipt screen had no logic (the old code, copied from another project, was
  commented out). It now shows the net amount, VAT and total, and prints.
- **Search filters:** each keystroke filtered the already-filtered list, so deleting characters
  never brought results back.
- **Navigation:** the "update" buttons opened the wrong screen or the "add" handler; a list form
  used its database context before creating it.
- **Customer validation:** errors were displayed but did not stop the save; the age check was inverted.

### Security & privacy
- Passwords are stored as salted **PBKDF2-SHA256** hashes. Existing plain-text passwords are
  upgraded automatically on the next successful login.
- Changing a password requires the current password.
- Card payments keep **only the last 4 digits**. The full card number, expiry date and CVV are
  no longer stored.
- The database file with real data was removed; the repository ships a script with fictional
  demo data. Personal contact details were removed from the About screen.

### Architecture
- New business layer: `CaseScheduler`, `FeeCalculator`, `Validation`, `PasswordHasher` (pure, no UI or
  database) and `CaseService`, `AuthService` (Entity Framework).
- Validation no longer shows message boxes: the rules return results and the screens display them.
- Empty `catch` blocks were replaced with user-facing error messages; unhandled UI exceptions are caught.
- Each screen owns and disposes its `DbContext`.
- Removed dead code: 6 unfinished or duplicate forms, 3 unused typed DataSets, a 7 MB unused
  animation and orphan resource files.

### New
- Manager-only **user management** screen.
- **Payments & receipts** screen with search and totals.
- Cases list sorted by the nearest deadline, with **late (red) / due soon (yellow)** highlighting.
- Courts offered for a case are filtered by the case type they handle.
- Quick "add new" buttons for client, opposing party and opposing lawyer while opening a case.
- `database/create-database.sql`: schema generated from the EDMX, constraints, indexes and demo data.
- 57 unit tests, 9 integration tests against the real database (rolled back after each test),
  and a GitHub Actions pipeline that builds the app and runs the database script on SQL Server 2022.
- Configurable VAT rate (18%) and "due soon" window in `App.config`.

## v1.0 — Final project (2024)

Original high-school final project, built with a law office as the client.
