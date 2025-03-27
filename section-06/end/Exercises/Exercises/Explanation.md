# Exercise - Solution Explanation

This solution focuses on applying the principles of minimizing side effects covered in this section of the course. The goal was to refactor code to make side effects explicit and controlled, resulting in more testable and maintainable code.

## Changes made to address issues

### 1. Avoiding Test Interference

The original code used a static `PromotionEngine` class with global state that caused test interference:
- Static fields for discount percentage and special offer status
- Global state affecting all orders in the system
- Tests would interfere with each other when run in parallel

To address this, I:

1. **Created an IPromotionManager interface**
    - Defined methods for getting and setting promotion state
    - Made the state local to each instance

2. **Implemented the interface with PromotionManager**
    - Encapsulates state within the instance
    - Maintains the original behavior for production use
    - Each instance has its own independent state

3. **Modified dependencies to use the interface**
    - Now accepts the promotion manager via injection
    - Tests can create isolated instances for each test case

The result is that each test can have its own isolated promotion state, preventing test interference and making tests reliable when run in any order or in parallel.

### 2. Converting Static Dependencies to Testable Code

The original code used a static `InventorySystem` class that was directly called from the `OrderProcessor`:
- The static inventory system couldn't be substituted in tests
- Testing inventory edge cases was virtually impossible
- Business logic was tightly coupled to implementation details

To solve this, I:

1. **Created an IInventorySystem interface**
    - Defined methods for checking and deducting stock
    - Made the dependency explicit and injectable

2. **Implemented the interface with InventorySystem**
    - Contains the same logic but as an instance class
    - Maintains the original behavior for production use

3. **Modified OrderProcessor to use the interface**
    - Now accepts the inventory system via constructor injection
    - Uses the injected implementation instead of static calls

This approach allows us to create test implementations of the inventory system that can simulate different scenarios (like stock shortages) without modifying actual inventory data.

### 3. Implementing Pure Functions

The original `CalculateOrderTotal` method was impure with multiple side effects:
- It modified order and item state during calculation
- It performed logging operations
- It had multiple responsibilities mixed together
- Each call could produce different results

To implement pure functions, I:

1. **Created an IOrderCalculator interface**
    - Defined a pure calculation method that doesn't modify state
    - Takes all required inputs as parameters

2. **Implemented OrderCalculator**
    - Contains pure calculation logic without side effects
    - Returns calculated total without modifying objects
    - Doesn't perform logging or other side effects

3. **Refactored OrderProcessor**
    - Now uses the calculator for the calculation logic
    - Explicitly applies side effects after calculation
    - Separates calculation from state changes

This separation makes testing much easier since we can test calculation logic independently from side effects. The pure function always returns the same output for the same input, making tests predictable and reliable.

### 4. Replacing Traditional Singletons

The original code used a `UserContext` singleton that was impossible to substitute in tests:
- The singleton pattern hard-coded the current user
- Testing with different user roles was difficult
- Tests could interfere with each other when changing user context

To address this, I:

1. **Created an IUserContext interface**
    - Defined methods for getting username and role
    - Made the dependency explicit and injectable

2. **Implemented the interface with UserContext**
    - Simple class that stores username and role
    - No longer a singleton but an instance class
    - Takes values via constructor

3. **Modified dependent classes to use the interface**
    - Now accept user context via constructor injection
    - Use the interface methods instead of singleton access

This approach allows us to create different user contexts for different tests, making it easy to test various authorization scenarios without test interference.

## Additional Improvements

Beyond the four main issues, I made some additional improvements to further minimize side effects:

1. **Created IDateTimeProvider**
    - Abstracts away direct dependencies on DateTime.Now
    - Makes time-dependent logic testable and deterministic

2. **Created IAuditLogger**
    - Separates audit logging concerns
    - Makes logging behavior explicitly testable

3. **Restructured OrderProcessor**
    - Made side effects explicit and localized
    - Improved separation of concerns
    - Reduced hidden dependencies

## What We've Achieved

1. **Eliminated Test Interference**: Tests can now run in isolation or in parallel without affecting each other
2. **Improved Testability**: All dependencies are explicit and can be substituted in tests
3. **Made Side Effects Explicit**: Side effects are now clearly visible and controlled
4. **Increased Determinism**: Tests now produce consistent results regardless of environment or execution order
5. **Enhanced Modularity**: Components have clear boundaries and can evolve independently

By applying the principles of minimizing side effects, we've transformed unpredictable, hard-to-test code into a reliable, testable design. The code now clearly separates pure logic from side effects, makes dependencies explicit, and eliminates hidden global state. This makes the system easier to understand, test, maintain, and extend.