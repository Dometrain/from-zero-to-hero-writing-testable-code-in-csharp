# Exercise - Explanation

This solution focuses strictly on the explicit behaviors principles covered in this section of the course, without introducing concepts from other sections like dependency injection or interface separation.

## Changes made to address issues

### 1. Clear Inputs and Outputs

1. **Strongly Typed Enums**
    - Replaced the integer status code with an `OrderStatus` enum
    - This makes the code more readable and eliminates "magic numbers"

2. **Consistent Return Types**
    - Created a `Result` pattern for all operations
    - Each method now returns a strongly-typed result that clearly communicates success/failure

3. **Parameter Validation**
    - Added explicit validation for all inputs at the start of each method
    - All methods return an appropriate error result when validation fails

4. **Default Values for Optional Parameters**
    - Added proper default values for optional parameters (e.g., `notes = null, priority = false`)

5. **Clear Method Signatures**
    - Made customer an explicit parameter in `CreateOrder` instead of using internal state
    - Parameters are ordered with required parameters first, followed by optional ones

### 2. Explicit Error Handling

1. **Explicit Error Communication**
    - Replaced silent failures with explicit error messages in the `Result` objects
    - Inventory errors are now collected and reported clearly

2. **Consistency in Error Handling**
    - Used the same `Result` pattern throughout all methods
    - No more mixing of return types for errors

3. **No Exception Swallowing**
    - Removed the try/catch in `ApplyDiscount` that was swallowing exceptions
    - Each error condition is now explicitly checked and reported

4. **Result Pattern**
    - Implemented the Result pattern for explicit error handling
    - This separates success/failure status from the actual return data

### 3. Separation of Construction and Logic

1. **Removed Logic from Constructor**
    - Moved business logic from the constructor to a separate method `DetermineExpressShippingAvailability`
    - Constructor now only performs simple initialization

2. **Explicit Customer Parameter**
    - Changed `CreateOrder` to take a `Customer` parameter instead of relying on internal state
    - This makes the dependency explicit and avoids hidden preconditions

3. **Simplified Constructor**
    - Constructor no longer makes complex decisions or performs side effects
    - Only basic initialization logic remains in the constructor

### 4. Visible State Changes

1. **Explicit State Mutations**
    - All methods that change state now return the modified objects
    - `ProcessOrder` returns the order with its updated status

2. **Return Values Reflect Changes**
    - Methods return not just success/failure but the actual modified entities
    - `ApplyDiscount` returns both the order and the discount amount

3. **Clear Method Names**
    - Method names clearly indicate when they change state
    - No more ambiguous methods that hide their true purpose

## Example Test Cases

With these refactorings, we can now easily test the `OrderService` class.
Here, we have written some tests using xUnit.

## What we've achieved

1. **Made inputs and outputs clear** - All methods now have explicit parameters and return types
2. **Implemented explicit error handling** - Errors are now part of the normal control flow
3. **Separated construction from logic** - Business logic is no longer in the constructor
4. **Made state changes visible** - All mutations are clearly communicated through return values

The code is now more testable, more maintainable, and communicates its intent more clearly.