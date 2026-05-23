# Database Design Cheat Sheet

This document contains key concepts and mental models for designing relational databases using C# and Entity Framework Core, established while building _Clash of Codes_.

---

## 1. Finding Table Relationships

To figure out the relationship between any two tables (Table A and Table B), you only need to ask yourself two simple "Can one have many?" questions:

- **Question 1:** "Can one A have many B's?"
- **Question 2:** "Can one B have many A's?"

### The Results

- **Yes & No:** It is a **One-to-Many** relationship. (Requires a Foreign Key).
- **Yes & Yes:** It is a **Many-to-Many** relationship. (Requires a Join Table).
- **No & No:** It is a **One-to-One** relationship. (Consider merging them into one table).

---

## 2. One-to-Many Relationships

**Example:** `Problem` and `McqQuestion`

- **Q1:** Can one `Problem` have many `McqQuestions`? **(Yes)**
- **Q2:** Can one `McqQuestion` belong to many `Problems`? **(No)**

### Where does the Foreign Key go?

**The Golden Rule:** In a One-to-Many relationship, the Foreign Key ALWAYS goes on the "Many" table.

**Why?**
A fundamental rule of SQL databases is that a single cell can only hold one piece of information.

- **Wrong Way:** If you put `McqQuestionId` on the `Problem` table, you would have to stuff `"101, 102, 103"` into a single cell, which SQL cannot properly read or query.
- **Right Way:** Put `ProblemId` on the `McqQuestion` table. Now you can have 3 separate MCQ rows, and all 3 rows just contain the number `1`, pointing perfectly back to the parent problem.

**Common Examples:**

- A Mother has Many Children -> Put `MotherId` on the `Child` table.
- A Post has Many Comments -> Put `PostId` on the `Comment` table.
- A Problem has Many MCQs -> Put `ProblemId` on the `McqQuestion` table.

---

## 3. Many-to-Many Relationships

**Example:** `User` and `Match`

- **Q1:** Can one `User` be in many `Matches`? **(Yes)**
- **Q2:** Can one `Match` have many `Users`? **(Yes)**

Because it answers "Yes" in both directions, we cannot solve this with a simple Foreign Key on either table.

### Why you cannot use a Foreign Key here:

- If you put `MatchId` on the `User` table, that User can only ever play exactly ONE match in their lifetime.
- If you put `UserId` on the `Match` table, that Match can only have ONE player in it. (We need two for Clash of Codes!)

### The Join Table Solution

We solve this by creating a third table that sits strictly in the middle, known as a **Join Table** (e.g., `MatchPlayer`).

- `User #1` -> Plays in -> `MatchPlayer Row A` -> Which links to -> `Match #100`
- `User #2` -> Plays in -> `MatchPlayer Row B` -> Which links to -> `Match #100`

Now, `Match #100` successfully has two players. Furthermore, `User #1` can have hundreds of rows in the `MatchPlayer` table pointing to hundreds of different matches!

**Bonus Benefit:**
The Join Table gives us the perfect place to store data that belongs strictly to that specific event. Properties like `CrownsEarned` or `SpellsUsed` don't belong to the `User` globally, nor do they belong to the `Match` as a whole—they belong specifically to "How User #1 performed during Match #100".
