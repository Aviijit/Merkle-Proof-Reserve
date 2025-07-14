# Merkle Proof of Reserve

This repository implements a secure, maintainable C# solution for generating **tagged-hash-based Merkle roots** and exposing a **Proof of Reserve Web API**, as outlined in the provided technical requirements.

## Projects

* **MerkleLib**
  A reusable C# library for:

  * Performing **BIP340-compatible tagged hashes**
  * Computing **Merkle roots** consistent with **Bitcoin transaction structure**
  * Generating **Merkle proofs**

* **MerkleLib.Tests**
  Unit tests validating:

  * Correctness of tagged hashes
  * Accurate Merkle root for test input `["aaa", "bbb", "ccc", "ddd", "eee"]`
  * Merkle proof structure and data integrity

* **ProofOfReserveApi**
  A minimal **ASP.NET Core Web API** providing:

  * Merkle root for current user balances
  * Merkle proof for any specific user by ID

## Third-Party Libraries

* `Microsoft.NET.Sdk` / `Microsoft.NET.Sdk.Web` – Base SDKs for building the class library and web API.
* `xunit` – Testing framework used to validate library logic.

> No additional third-party libraries were used to preserve maintainability and minimize dependencies. All core functionality is implemented using .NET runtime libraries.

## Building the Solution

Build the solution:

```bash
dotnet build
```

Run unit tests:

```bash
dotnet test
```

Launch the API:

```bash
dotnet run --project ProofOfReserveApi
```

## Testing the Merkle Root

Test data for root calculation:

```json
["aaa", "bbb", "ccc", "ddd", "eee"]
```

Hash Tag: `"Bitcoin_Transaction"`

This root is verified within the test project.

## API Features

### 1. Generate Merkle Root for All Users

* Uses in-memory database:

  ```
  (1,1111), (2,2222), (3,3333), (4,4444),
  (5,5555), (6,6666), (7,7777), (8,8888)
  ```
* Each entry serialized as `"(UserID,Balance)"` in UTF-8
* Hash Tags:

  * Leaf: `"ProofOfReserve_Leaf"`
  * Branch: `"ProofOfReserve_Branch"`

### 2. Get Merkle Proof for a Specific User

* Input: User ID
* Output:

  * Balance
  * Proof path (direction + sibling hash pairs)

## Versioning

* Git tags are used to mark the completion of:

  * Initial Merkle tree implementation
  * API integration updates

## Future Improvements (Optional)

Suggestions for production deployment include:

* Persistent database integration
* Secure API authentication
* Performance optimization for large datasets
* Caching and proof invalidation logic
* Logging, metrics, and monitoring
* Automated deployment via CI/CD pipelines
