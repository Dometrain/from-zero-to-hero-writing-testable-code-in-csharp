# Order Management Refactoring Exercise - Minimise Side Effects

## Objective
Refactor the Order Management code to improve testability by applying the principles of minimizing side effects that we've covered throughout this section.

## Exercise Description
The provided code in the project contains several classes that violate the side effect minimization principles we've discussed. Your task is to refactor this code to address the problems covered in our lectures:

1. **Avoiding Test Interference**
2. **Converting Static Dependencies to Testable Code**
3. **Implementing Pure Functions**
4. **Replacing Traditional Singletons**

**Important:** You are free to create new interfaces, abstractions, and adapters as needed. The goal is to make the code testable while preserving the essential business functionality. Don't worry about breaking the public contracts.

## Starting Code
You'll be working with several classes in the project that demonstrate different side effect issues:

- `OrderService` - Contains various side effect issues we need to fix
- Supporting classes: `PromotionEngine`, `InventorySystem`, `OrderProcessor`, and `UserContext`

## Issues to Address

### 1. Avoiding Test Interference

The `PromotionEngine` class uses global state that causes test interference:

- It uses static fields to manage discount information that affects all orders
- Tests using the promotion system will interfere with each other
- There's no way to isolate the discount effects between different test cases
- Test run order can affect results in unpredictable ways

**Your Task:** Refactor the promotion logic to eliminate global state and avoid test interference.

### 2. Converting Static Dependencies to Testable Code

The `InventorySystem` is implemented as a static class that makes testing difficult:

- The `OrderProcessor` directly depends on this static inventory system
- It's impossible to substitute the inventory system for testing
- Edge cases like stock shortages are hard to simulate
- Testing error paths becomes particularly challenging

**Your Task:** Refactor the inventory system dependencies to make them explicit and testable.

### 3. Implementing Pure Functions

The `OrderProcessor.CalculateOrderTotal` method is impure with multiple side effects:

- It modifies the order and its items while performing calculations
- It has side effects like logging and changing object state
- The calculation logic is mixed with side effects
- Multiple calls to the same method can produce different results

**Your Task:** Refactor to separate pure calculation logic from side effects, making it predictable and testable.

### 4. Replacing Traditional Singletons

The `UserContext` singleton is used for authorization but creates testing challenges:

- It's impossible to substitute different users in tests
- Authorization scenarios are difficult to verify
- Tests may interfere with each other when changing the current user
- Parallel test execution becomes problematic

**Your Task:** Replace the singleton pattern with a testable alternative that allows proper dependency injection.

## Need help?
Go back and rewatch the previous lectures. It usually helps out.
If you still need help after that, don't hesitate to reach out (https://guiferreira.me/about)!

## Looking for an accountability partner?
Tag me on X (@gsferreira) or LinkedIn (@gferreira), and I will be there for you.

Let's do it!
