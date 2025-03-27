# Order Management Refactoring Exercise - Narrow Responsibilities

## Objective
Refactor the Order Management code to improve testability by applying the principles of narrow responsibilities that we've covered throughout this section.

## Exercise Description
The provided code in the project contains an `OrderService` class that violates the narrow responsibilities principle in multiple ways. Your task is to refactor this code while addressing the problems we've covered in our lectures:

1. **Identifying and Extracting Responsibilities**
2. **Dealing with Constructor Bloat**
3. **Creating Effective Injection Points**
4. **Injectable vs. Newable Objects**
5. **Composition of Narrow Components**


**Important:** You are free to create new classes, interfaces, and refactor as needed. The goal is to improve testability while preserving the essential business functionality. Don't worry about breaking the public contracts.

## Starting Code
You'll be working with several classes in the project that contain issues related to responsibilities:
- `OrderService`
- `OrderNotificationService`

## Issues to Address

### Main Class: OrderService

#### 1. Multiple Responsibilities
The `OrderService` class handles:
- Inventory validation
- Price calculation with discounts and taxes
- Payment processing
- Inventory updates
- Customer loyalty management
- Order persistence
- Notifications

Extract these responsibilities into focused classes with single purposes.

#### 2. Constructor Bloat
The `OrderService` creates many dependencies directly in its constructor:
- `DiscountCalculator`
- `TaxCalculator`
- `PaymentProcessor`
- `EmailService`
- `PushNotificationService`
- Plus internal collections and connection string

Break these into logical groups and make dependencies explicit through constructor injection.

#### 3. Hard-coded Dependencies
Several methods use hard-coded dependencies that should be injectable:
- `DateTime.Now` appears in multiple places for:
  - Seasonal discount calculations
  - Setting the order date
  - Setting notification timestamps
- Direct console logging for low stock alerts
- Direct console logging for customer updates

Replace these with proper abstractions and injection points.

#### 4. Monolithic Methods
The `ProcessOrder` method tries to do everything at once:
- Inventory validation
- Price calculation
- Payment processing
- Inventory updates
- Customer updates
- Order creation
- Notifications

Refactor this into a composition of narrow, focused components.

### Secondary Class: OrderNotificationService

#### 5. Injectable vs. Newable Objects
This class demonstrates confusion over what should be injected vs. created directly:
- Services with behavior are created directly: `EmailService`, `LoyaltyPointsCalculator`, `PushNotificationService`
- Data transfer objects: `OrderNotification`

Refactor to properly distinguish between what should be injected and what can be created directly.

## Need help?
Go back and rewatch the previous lectures. It usually helps out.
If you still need help after that, don't hesitate to reach out (https://guiferreira.me/about)!

## Looking for an accountability partner?
Tag me on X (@gsferreira) or LinkedIn (@gferreira), and I will be there for you.

## Ready to tackle the next principle?
After completing this exercise, you'll be ready for the "Isolate Dependencies" section, which builds on these concepts.

Let's do it!
