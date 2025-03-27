# Order Management Refactoring Exercise - Isolate Dependencies

## Objective
Refactor the Order Management code to improve testability by applying the principles of isolating dependencies that we've covered throughout this section.

## Exercise Description
The provided code in the project contains several classes that violate the dependency isolation principles we've discussed. Your task is to refactor this code to address the problems covered in our lectures:

1. **Working with Static Dependencies**
2. **Breaking Dependency Chains**
3. **Creating Abstractions for External Services**
4. **Isolating Framework Code**

**Important:** You are free to create new interfaces, abstractions, and adapters as needed. The goal is to make the code testable while preserving the essential business functionality. Don't worry about breaking the public contracts.

## Starting Code
You'll be working with several classes in the project that demonstrate different dependency issues:

- `OrderService` - Contains all the dependency issues we need to fix
- Supporting classes: `ShippingCostCalculator`, `OrderPrinter`, `OrderConfigProvider`, and `Metrics`

## Issues to Address

### 1. Working with Static Dependencies

The `OrderService.ProcessOrder` method uses static dependencies that make it difficult to test:

- It relies on the static `Metrics` class to record order processing events
- There's no way to intercept or verify these metrics calls in tests
- Testing error conditions becomes particularly difficult

**Your Task:** Refactor the order processing logic to remove static dependencies and make metric recording behavior testable.

### 2. Breaking Dependency Chains

The `ShippingCostCalculator` class contains dependency chains that make testing difficult:

- It navigates through multiple objects (order → shipping address → country, order → customer → membership → level)
- Long method chains make it impossible to test parts of the chain in isolation
- Edge cases are difficult to simulate without setting up complex object graphs

**Your Task:** Break the dependency chains to make the shipping cost calculation logic testable in isolation.

### 3. Creating Abstractions for External Services

The `OrderPrinter` class has direct dependencies on external services:

- It directly uses `Console` for output operations
- There's no way to test the printing logic without capturing console output
- The formatting is tightly coupled with the implementation

**Your Task:** Create appropriate abstractions for console operations to make the printing functionality testable without output side effects.

### 4. Isolating Framework Code

The `OrderConfigProvider` and `OrderService` classes use framework dependencies that introduce non-determinism:

- Direct usage of `Environment.GetEnvironmentVariable` makes tests dependent on environment
- `DateTime.Now` and `Random` usage makes tests unpredictable
- HTTP calls via `HttpClient` are difficult to test

**Your Task:** Isolate these framework dependencies so the code becomes deterministic and testable.

## Need help?
Go back and rewatch the previous lectures. It usually helps out.
If you still need help after that, don't hesitate to reach out (https://guiferreira.me/about)!

## Looking for an accountability partner?
Tag me on X (@gsferreira) or LinkedIn (@gferreira), and I will be there for you.

Let's do it!