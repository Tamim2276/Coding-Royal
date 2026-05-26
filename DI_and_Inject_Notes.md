# Dependency Injection in Blazor (`@inject` vs `@using`)

When building Blazor components, you will almost always see these two commands at the top of the file:

```html
@using ClashOfCodes.Services @inject AuthService AuthService
```

While they look similar, they do two completely different jobs.

---

## 1. `@using` (The Dictionary)

`@using` is simply a **navigational tool**.

Computers need exact paths. If you tell Blazor to use an `AuthService`, Blazor will panic and say, _"I have 10,000 files in this project, I don't know which 'AuthService' you are talking about!"_

When you write `@using ClashOfCodes.Services`, you are telling the compiler:
_"If you see me type the word `AuthService`, go look inside the `ClashOfCodes.Services` folder to find the definition for it."_

**Analogy:** Looking up a word in a dictionary to understand what it means. It doesn't give you the physical object, it just tells you _about_ it.

---

## 2. `@inject` (The Delivery Guy)

`@inject` is the core of **Dependency Injection (DI)**. This is how you actually get a working piece of machinery into your web page.

### The Problem without `@inject`

Normally in C#, to use a class, you create it manually using the `new` keyword:

```csharp
AuthService myService = new AuthService();
```

But `AuthService` might require an `HttpClient` and a `LocalStorageManager` to even turn on. Those things might require _other_ things! Creating it manually becomes a massive spaghetti web of code.

### The Solution

When your app starts (in `Program.cs`), you tell ASP.NET to build these services once and put them on a virtual "tray" held by an invisible Waiter.
(`builder.Services.AddScoped<AuthService>();`)

When you type `@inject AuthService AuthService` at the top of your UI page, you are telling Blazor:
_"Hey Waiter, when you load this page for the user, please grab the fully-built, ready-to-use `AuthService` from your tray and hand it to me inside a variable called `AuthService`."_

**Analogy:** You aren't building a pizza from scratch (`new Pizza()`); you are calling UberEats and having the fully cooked, perfectly assembled pizza delivered directly to your front door (`@inject Pizza`).

---

## Summary

- **`@using`** shows the compiler _where_ the blueprint for an object is located.
- **`@inject`** asks the background system to _deliver an already built, working copy_ of that object straight to your page for immediate use.
