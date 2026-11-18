# Changelog

## v1.3.2 — Second independent code review (2026)

A second reviewer went over the v1.3.1 changes. Fixed:
- **Security — guessing passwords through "change password":** `usp_ChangeOwnPassword` accepted any
  user name and did not count wrong current passwords, so a worker could guess the manager's password
  without limit. It now runs the same check as login (lock and failure counting) and audits the change.
- **Security — parallel guesses and user enumeration:** `usp_TryLogin` counts failures under a row lock
  (`UPDLOCK, HOLDLOCK`), and an unknown user gets the same answer as a wrong password from the
  procedure itself, not only in the app.
- **Bug — "Admin" vs "admin":** the session kept the name as typed, so a manager logged in as "Admin"
  could deactivate their own account. The database returns the stored name; the self-check uses the id.
- **Bug — the minimum fee changed old totals:** each case now keeps the fees of its case type from when
  it was opened (`MinimumFee`, `CourtFee`), so editing the catalogue never turns a paid case into a debt.
- **Upgrade path:** `database/upgrade-from-v1.3.sql` (generated from `create-database.sql`) upgrades a
  v1.3 database without losing data, in one transaction. CI upgrades a real v1.3 database every build.
- A payment can be marked cancelled only together with its credit note (trigger); the app saves the
  credit note first, in one transaction. Audit entries record the real database login (`DbLogin`).
- An existing customer stored without the leading 0 of their ID is found as a duplicate.
- CI: sqlcmd runs with `QUOTED_IDENTIFIER ON` (`-I`), every refused query must fail for the expected
  reason, guessing through the procedures must lock, missing LocalDB fails the build, and the release
  zip is built in its own job only after every check passed.
- Code: `CaseForm.cs` (851 lines) split into partial files by topic; a unit test reads the lock numbers
  from the SQL script; 8 new integration tests, including one without an outer transaction.

## v1.3.1 — Independent code review and screen polish (2026)

### Code review findings
- **Bug — a retried save doubled a new case's timeline:** when saving a new case failed (for example a
  typo that overflowed the amount column) and the user pressed Save again, every step was added twice.
  Saving a case now lives in `CaseService.SaveCase`; a failed save is rolled back in the database and
  in memory, so a retry starts clean.
- **Bug — a failed save left changes behind:** after a lost connection or a timeout, the unsaved payment
  (or step change) stayed in the screen's context and was written by the next save, e.g. a payment
  recorded twice. Every failed save now discards its pending changes (`ChangeDiscarder`), and the payment
  window reloads the current balance.
- **Security — a worker could make themselves a manager through SQL:** `security.sql` let workers read
  every password hash and update the `Users` table. Workers now have no access to `Users`: login and
  password change run as stored procedures that compare the hash inside the database and count failed
  attempts. Workers can only set the "cancelled" flag of a payment, a trigger refuses any change to a
  recorded payment, and only a manager can re-open a completed step. CI checks all of this as a worker.
- The "case opened" audit entry now records the new case number.
- An Israeli ID with or without its leading 0 is the same customer (stored as 9 digits).
- Case rules (amounts, percentages, client ≠ opposing party) are checked in `CaseService`, not only in
  the form. Catalogue changes (courts, case types, procedures, holidays) check the manager role in the
  business layer and are written to the audit log.
- Amounts are capped at 1,000,000,000 ₪; a step date more than 20 years away is refused as a typo.
- The case type's fee is now a real **minimum fee**: the case total is never below it.
- Login gives the same answer, in the same time, for an unknown user and a wrong password; a
  deactivated user is told so only after typing the right password.
- A new case window is found by its case number after its first save, so the same case does not open twice.
- Database: foreign keys for the opposing party and lawyer and for credit notes, a payment can be
  cancelled only once, `CHECK` constraints on amounts, percentages, roles and VAT.
- Code: generic control names (`button1`, `dataGridView1`, `listView2`, `btnOK_Click_1`) and leftover
  names (`subject`, `process`, `StatusId`) renamed; one "late / due soon" rule shared by every screen;
  the model and SQL generators are now in `tools/`; image resources have English names.
- Tests: 134 unit and 31 integration tests. New: retry after a failed save, audit entry of a new case,
  manager-only catalogue, payment immutability, login salt for unknown users, minimum fee, ID
  normalisation, money limits. Misplaced and weak tests were fixed.

### Screen polish
Found while taking the README screenshots on Windows:
- **Unreadable text on the dark list screens:** the cases list (and the other dark lists) inherited a
  light text color, so rows, the color legend and the "clear filter" button showed light text on a light
  background. Grid cells, headers, the legend and the button now use dark text.
- **Cut-off values:** money totals on the case screen (`18,20` instead of `18,200.00 ₪`) and the date,
  phone, payment method and description on the receipt were clipped by labels that were too narrow.
- **Table formatting:** money columns show `#,##0.00` and date columns `dd/MM/yyyy` in every list;
  id columns take less room. The payments screen is wider so no column is cut.
- The home screen greeting no longer hides behind the "office settings" button, and the cases list
  window has a real title instead of `;`.

## v1.3 — English code base and edge cases (2026)

### English names
The code and database used Hebrew words written in Latin letters (`Tikim_Letipul_Orech_Din`,
`Tashlumim_Letik`, `frmAddCostemer`). Everything is now English: tables (`Cases`, `CaseSteps`,
`Payments`, `Customers`, ...), columns, entity classes (`LegalCase`, `CaseStep`, `Payment`, ...), the
context (`QuedinDbContext`), forms (`CaseForm`, `CustomerForm`, `HomeForm`, ...), controls and local
variables. The screens stay in Hebrew. The EF model is generated from one spec. Unused columns that
held sensitive data (card CVV, ID copy, payer ID number, card expiry) were removed.

### Edge cases (from the edge-case report)
- **Bug — court dates were overwritten:** completing or undoing an earlier step recalculated a date
  the court had set. Such steps are now *fixed*; later steps are counted from them, and the user is
  warned when an earlier step is expected after a fixed date.
- **Bug — deleted case while open:** a user saving a case that a manager deleted got a confusing error
  and a window on a case that no longer existed. Now they are told it was deleted and the window closes.
- A court date may be earlier than the estimate of an open earlier step (only a completed step limits it).
- Open case windows notice changes by others when they get focus (reload, or a warning when the user
  has unsaved edits). Closing a window with unsaved changes asks to save; logout warns only about
  windows with real unsaved changes.
- Customers have optimistic concurrency like cases.
- Warning before opening a second open case of the same client and case type.
- Jewish holidays on which courts are closed are skipped (computed from the Hebrew calendar); managers
  add extra days (Independence Day, recess) in a new **Office settings** screen.
- Steps of cases opened before v1.2 now store their duration on first use, so catalogue changes no
  longer move them.
- VAT rate and office details are stored in the database and shared by every workstation.
- Refund of an overpayment (after lowering the claim amount), with a warning when the new total is
  below what was paid; after cancelling a payment the user is offered to record the correct one.
- The advance cannot be larger than the case total.
- "Today" comes from the database server; a warning at login when the PC clock is off.
- Backup reminder on the home screen, and `database/backup-nightly.cmd` for a scheduled nightly backup.
- `database/security.sql`: SQL Server roles so the rules hold outside the app; the audit log is
  append-only (trigger + DENY).
- Users are locked for 15 minutes after 5 wrong passwords (a password reset releases them) and can be
  deactivated by a manager.
- Unit tests: 104 → 115. Integration tests: 15 → 24.

## v1.2 — Fixes from a user test and a code review (2026)

Two reviews were written against v1.1: one from the point of view of an office worker using the
app, one from a reviewer looking for edge cases in the code. Everything below comes from them.

### Daily work
- **Home screen "what's urgent today":** the next step of every open case that is late or due soon.
  An empty system shows a setup banner; managers get a step-by-step **setup guide**.
- **Timeline:** mark a step done on the real date (not in the future, not before the previous step);
  set a date decided by the court, with a note; managers can undo the last completion; notes per step;
  "days late" column in the case and the cases list. Deadlines never fall on Friday or Saturday.
- **Cases list:** free search over every column, a clear-filters button, a colour legend and a
  "showing X of Y" counter. Case windows are no longer modal: several cases can be open side by side.
- **Opposing parties:** ID number is optional (ID / company number / passport / none, with an internal
  code), company names such as `אלפא נכסים בע"מ` are accepted, international phone numbers too.
  Any person can be picked as the other side. The customer screen lists the person's cases and the
  customers list filters by type.
- **Payments:** payment kind (fee / court fee and expenses). Cancelling adds a credit note instead of
  deleting; the original stays visible and marked. Receipts show the office details, the VAT rate
  stored with the payment, no VAT on court fees, and "credit note" wording for cancellations.
- **Smaller fixes:** power of attorney is a "signed on" checkbox and date; warning before switching
  court case / deal hides data already typed; Caps Lock / Hebrew keyboard warning on login; username
  field inside the change-password box; manager password reset; confirmations appear as a status bar
  instead of pop-ups; logout asks for confirmation; friendlier database error messages; typos
  ("עידכון", "טירחה").

### Correctness (code review)
- `"12,5"` was parsed as 125 and `0.004` was saved as ₪0. Amounts are now parsed strictly and a
  payment must be at least ₪0.01.
- The balance was copied from the last payment, so changing the claim amount was ignored. It is now
  always computed from the case total and all payments; "paid" and "advance paid" are derived, not
  checkboxes.
- Steps were ordered by date, so a court-set date could reorder the timeline. They keep creation order.
- Editing a process duration moved deadlines of existing cases. Each step now stores its own duration.
- Two users editing the same case silently overwrote each other. Cases have a `rowversion` and the
  second save is refused.
- A case opened by mistake could not be removed. Managers can delete it while nothing was paid or done.
- No record of who changed what: new audit log (`Yoman`) with a viewer, and a database backup button.

### Schema (v1.2)
New table `Yoman`; new columns `Customers.SugMezahe`, `Tikim_Letipul_Orech_Din.RowVersion`,
`Tahalich_Tipul_Betik.Hearot / MeshechYamim`, `Tashlumim_Letik.SugTashlum / VatRate / Mevutal /
MevatelTashlum`; contact fields of customers are nullable and phone columns are 20 characters.
Run `database/create-database.sql` again to create the new schema.

### Not done yet
- Israeli public holidays are not in the business calendar (only Friday/Saturday).
- The integration tests run on Windows with LocalDB; CI runs the unit tests and checks the SQL script
  on SQL Server, but not the integration tests.
- A few original management screens (courts, case types, processes) still have the old styling.

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
