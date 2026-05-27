# JWT Parsing Logic Notes

## 1. The JWT Structure
A JSON Web Token (JWT) consists of three parts separated by dots:
`Header.Payload.Signature`

The `ParseClaimsFromJwt` method focuses exclusively on the **Payload**, which contains the user's identity information (Claims).

---

## 2. Code Breakdown

### Step 1: Isolate the Payload
```csharp
var payload = jwt.Split('.')[1];
```
- **Action**: Splits the token string by the `.` character.
- **Result**: Selects index `[1]`, which is the middle section (the Payload).

### Step 2: Decode Base64
```csharp
var jsonBytes = ParseBase64WithoutPadding(payload);
```
- **Action**: Converts the Base64Url encoded string back into a raw byte array.
- **Why?**: JWTs are encoded to be URL-safe, not encrypted. This "unwraps" the encoding to reveal the JSON text.

### Step 3: Deserialize JSON
```csharp
var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
```
- **Action**: Turns the raw bytes into a C# `Dictionary`.
- **Syntax**: `<Dictionary<string, object>>` tells the computer that the keys are strings (names of claims) and values can be any object (the data).

### Step 4: Convert to Claims
```csharp
return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
```
- **Action**: Uses LINQ `.Select()` to loop through the dictionary.
- **Result**: Transforms every Key-Value pair into a `System.Security.Claims.Claim` object.

---

## 3. Visual Example

**Input Token:** `Header.eyJDbGFpbV9OYW1lIjogIlRhbWltIn0.Signature`

| Stage | Transformation | Result |
| :--- | :--- | :--- |
| **Split** | Extract middle part | `eyJDbGFpbV9OYW1lIjogIlRhbWltIn0` |
| **Decode** | Base64 $\rightarrow$ String | `{"Claim_Name": "Tamim"}` |
| **Deserialize** | String $\rightarrow$ Dictionary | `Key: "Claim_Name" | Value: "Tamim"` |
| **Select** | Dictionary $\rightarrow$ Claim | `new Claim("Claim_Name", "Tamim")` |

## 4. The Base64 Padding Fix (`ParseBase64WithoutPadding`)

Standard C# Base64 decoding (`Convert.FromBase64String`) requires the input string length to be a multiple of 4. If it isn't, the method throws an error. However, JWTs use **Base64Url** encoding, which strips away the padding (the `=` signs).

### How it works:
```csharp
switch (base64.Length % 4)
{
    case 2: base64 += "=="; break;
    case 3: base64 += "="; break;
}
```
- **`base64.Length % 4`**: Checks the remainder of the length when divided by 4.
- **Case 2**: If 2 characters are missing, add `==`.
- **Case 3**: If 1 character is missing, add `=`.

### Example:
If the payload is `SGVsbG9` (length 7):
1. $7 \div 4 = 1$ remainder **3**.
2. Matches `case 3` $\rightarrow$ adds one `=` $\rightarrow$ `SGVsbG9=`.
3. Now length is 8 (multiple of 4), and `Convert.FromBase64String` works perfectly.

---

## Summary
This method is essentially a **translator**. It takes a web-friendly encoded string and translates it into a list of C# `Claim` objects that the Blazor security system can use to authorize the user.

