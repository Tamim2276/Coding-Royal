# Blazor Authentication State Provider Notes

## 1. Retrieving the Token
```csharp
var savedToken = await _localStorage.GetItemAsync<string>("authToken");
```
- **`<string>` (Generics)**: Tells the method to return the data as a string. Since Local Storage stores everything as text, we specify `string` to let C# know the expected type.
- **`"authToken"` (The Key)**: This is the unique identifier (like a label on a folder). You must use the exact same key name when saving the token and when retrieving it.

---

## 2. Creating the User Identity
Once a token is found, it must be converted into a format Blazor understands.

```csharp
// 1. Extract facts (Claims) from the token
var claims = ParseClaimsFromJwt(savedToken);

// 2. Put those facts on a "Digital ID Card" (Identity)
var identity = new ClaimsIdentity(claims, "jwt"); 

// 3. Assign the ID card to the User (Principal)
var user = new ClaimsPrincipal(identity);

// 4. Wrap the user in the Application State
return new AuthenticationState(user);
```

### Key Concept Definitions:
| Term | Analogy | Purpose |
| :--- | :--- | :--- |
| **Claim** | A single fact | e.g., "User is an Admin", "Email is x@y.com" |
| **ClaimsIdentity** | An ID Card | Groups all claims together and assigns an auth type (e.g., "jwt") |
| **ClaimsPrincipal** | The User | The security entity that "holds" the ID card |
| **AuthenticationState** | Login Status | Tells Blazor whether the current session is "Authenticated" or "Anonymous" |

## 3. Real-World Example

Imagine the `savedToken` is a long encrypted string like: `eyJhbGci...`

**Step 1: `ParseClaimsFromJwt`**
The code decrypts that string and finds these facts:
1. Name: `Tamim`
2. Role: `Admin`
3. UserId: `550e8400`

**Step 2: `ClaimsIdentity`**
The code creates an ID card:
- **ID Card Label:** "JWT Verified"
- **Details on Card:** [Name: Tamim, Role: Admin, ID: 550e8400]

**Step 3: `ClaimsPrincipal`**
The code assigns this card to the current session:
- *"The person visiting this website is now the holder of the JWT ID card."*

**Step 4: `AuthenticationState`**
The app now knows the user is authenticated. This allows you to do things in your UI like:
```razor
<AuthorizeView Roles="Admin">
    <Authorized>
        <p>Hello Admin Tamim! You can see the Secret Dashboard.</p>
    </Authorized>
    <NotAuthorized>
        <p>You are not an admin. You cannot see this.</p>
    </NotAuthorized>
</AuthorizeView>
```

---

## 4. Real-World Flow
1. **Request**: `GetAuthenticationStateAsync` is called on page load.
2. **Lookup**: App checks Local Storage for the `"authToken"` key.
3. **Transformation**: Raw Token $\rightarrow$ Claims $\rightarrow$ Identity $\rightarrow$ Principal $\rightarrow$ State.
4. **UI Response**: Blazor uses this state to show/hide elements using `<AuthorizeView>`.
