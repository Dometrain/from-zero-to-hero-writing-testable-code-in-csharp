# Order Management Refactoring Exercise

## Objective
Refactor the Order Management code to improve testability by applying the principles of explicit behaviors that we've covered throughout this section.

## Exercise Description
The provided `OrderService` class contains several issues that make it difficult to test and maintain. 
Your task is to refactor this code while addressing the problems in each of the four areas we've covered:

1. **Clear Inputs and Outputs**
2. **Explicit Error Handling**
3. **Separation of Construction and Logic**
4. **Visible State Changes**

**Important:** Don't worry about breaking the public contracts.

## Starting Code
You'll be working with the `OrderService` class that manages product inventory and order processing. This class contains various issues that compromise its testability.

## Issues to Address

### Clear Inputs and Outputs
- Replace magic numbers (like the integer status codes) with strongly typed enums
- Make return values explicit and consistent across methods
- Add parameter validation where missing
- Fix optional parameters that don't have default values
- Create a consistent return type pattern for methods

### Explicit Error Handling
- Address silent failures when products aren't found
- Implement proper error communication for insufficient stock
- Make error handling consistent across methods
- Avoid exception swallowing

### Separation of Construction and Logic
- Remove business logic from the constructor
- Make dependencies explicit rather than hiding them as class fields
- Separate order creation from inventory management logic

### Visible State Changes
- Make state mutations explicit and visible
- Ensure methods that change state clearly communicate those changes
- Return modified entities to make state changes visible
- Make side effects obvious or eliminate them

## Need help?
Go back and rewatch the previous lectures. It usually helps out. 
If you still need help after that, don't hesitate to reach out (https://guiferreira.me/about)!

## Looking for an accountability partner?
Tag me on X (@gsferreira) or LinkedIn (@gferreira), and I will be there for you.

Let's do it!