# Adapter Design Pattern

The **Adapter Pattern** is a **Structural Design Pattern** that allows two incompatible classes to work together.

> **Core idea:** The Adapter acts as a translator between the interface your application expects and the interface an existing class provides.

---

## 📌 Simple Definition

> The Adapter Pattern allows classes with incompatible interfaces to work together by converting the interface of one class into an interface expected by the client.

---

# 🔌 Real-World Example

Imagine you have a charger with a USA plug:

```text
USA Charger
    │
    │
    ▼
   ||
   ||
```

But your wall socket uses a different format:

```text
Pakistan Socket
   ●   ●
```

The charger cannot connect directly.

So you use an adapter:

```text
USA Charger
     │
     ▼
  Adapter
     │
     ▼
Pakistan Socket
```

The adapter doesn't change the charger or the socket.

It simply **makes them compatible**.

That's exactly what the Adapter Pattern does in software.

---

# 💻 Software Example

Imagine an e-commerce application that supports multiple payment providers.

Our application expects every payment provider to have:

```csharp
Pay(decimal amount);
```

So we define an interface:

```csharp
public interface IPaymentGateway
{
    void Pay(decimal amount);
}
```

---

# ❌ The Problem

Suppose we already have a third-party or legacy payment library:

```csharp
public class LegacyPaymentGateway
{
    public void MakePayment(double amount)
    {
        Console.WriteLine($"Legacy Payment Successful : {amount}");
    }
}
```

Our application expects:

```text
Pay(decimal amount)
```

But the legacy library provides:

```text
MakePayment(double amount)
```

There are two differences:

| Application Expects | Legacy Class Provides |
| ------------------- | --------------------- |
| `Pay()`             | `MakePayment()`       |
| `decimal`           | `double`              |

These interfaces are incompatible.

We cannot do this:

```csharp
IPaymentGateway payment = new LegacyPaymentGateway();
```

because `LegacyPaymentGateway` doesn't implement `IPaymentGateway`.

---

# ✅ Solution: Adapter

Create an adapter between the application and the legacy class:

```text
Application
     │
     ▼
IPaymentGateway
     │
     ▼
PaymentAdapter
     │
     ▼
LegacyPaymentGateway
```

The adapter translates the application's request into something the legacy class understands.

---

# 🧩 Structure of Adapter Pattern

The basic structure looks like this:

```text
        Client
          │
          ▼
   Target Interface
          │
          ▼
       Adapter
          │
          ▼
   Adaptee / Existing Class
```

### Components

| Component   | Responsibility                                |
| ----------- | --------------------------------------------- |
| **Client**  | Uses the expected interface                   |
| **Target**  | Interface expected by the client              |
| **Adapter** | Converts Target calls into Adaptee calls      |
| **Adaptee** | Existing class with an incompatible interface |

---

# 💻 Complete C# Example

## 1. Target Interface

This is what our application expects:

```csharp
public interface IPaymentGateway
{
    void Pay(decimal amount);
}
```

---

## 2. Adaptee — Existing Class

This is the existing class that we cannot or don't want to modify:

```csharp
public class LegacyPaymentGateway
{
    public void MakePayment(double amount)
    {
        Console.WriteLine($"Legacy Payment Successful : {amount}");
    }
}
```

The class has a different interface.

---

## 3. Adapter

The adapter implements the interface expected by our application.

```csharp
public class PaymentAdapter : IPaymentGateway
{
    private readonly LegacyPaymentGateway _legacyGateway;

    public PaymentAdapter()
    {
        _legacyGateway = new LegacyPaymentGateway();
    }

    public void Pay(decimal amount)
    {
        _legacyGateway.MakePayment((double)amount);
    }
}
```

The important part is:

```csharp
public void Pay(decimal amount)
{
    _legacyGateway.MakePayment((double)amount);
}
```

The adapter:

1. Receives `Pay()`
2. Converts `decimal` to `double`
3. Calls `MakePayment()`
4. Delegates the operation to the legacy class

---

# 4. Client Code

The application only knows about `IPaymentGateway`.

```csharp
class Program
{
    static void Main()
    {
        IPaymentGateway payment = new PaymentAdapter();

        payment.Pay(5000);

        Console.ReadLine();
    }
}
```

### Output

```text
Legacy Payment Successful : 5000
```

The client doesn't know that a legacy payment system is being used.

---

# 🔄 Execution Flow

When the application executes:

```csharp
payment.Pay(5000);
```

the flow is:

```text
Application
     │
     ▼
PaymentAdapter.Pay()
     │
     ▼
Convert decimal → double
     │
     ▼
LegacyPaymentGateway.MakePayment()
     │
     ▼
Payment Successful
```

The client thinks it is calling:

```text
Pay()
```

But the adapter translates that call into:

```text
MakePayment()
```

---

# 🎯 Why Use an Adapter?

Adapters are especially useful when working with:

* Legacy code
* Third-party libraries
* NuGet packages
* External APIs
* Vendor SDKs
* Systems with different interfaces
* Existing classes that cannot be modified

For example:

```text
Your Application
       │
       ▼
IPaymentGateway
       │
       ▼
PaymentAdapter
       │
       ▼
Third-Party SDK
```

---

# 🚫 Why Not Modify the Existing Class?

Imagine `LegacyPaymentGateway` comes from:

* A third-party DLL
* A NuGet package
* An external vendor
* An old system
* A closed-source library

You may not be able to change its source code.

Even if you can change it, modifying third-party or legacy code may introduce unnecessary risk.

Instead, create an adapter:

```text
Existing Code
     │
     ▼
  Adapter
     │
     ▼
Your Application
```

This keeps the existing code untouched.

---

# 📝 Another Example: Logger

Suppose your application expects:

```csharp
public interface ILogger
{
    void Log(string message);
}
```

But a third-party logging library provides:

```csharp
public class FileLogger
{
    public void WriteToFile(string text)
    {
        Console.WriteLine(text);
    }
}
```

The interfaces don't match.

---

## Create an Adapter

```csharp
public class LoggerAdapter : ILogger
{
    private readonly FileLogger _logger = new FileLogger();

    public void Log(string message)
    {
        _logger.WriteToFile(message);
    }
}
```

Now the application can use:

```csharp
ILogger logger = new LoggerAdapter();

logger.Log("Application Started");
```

The application only knows:

```text
ILogger
```

It doesn't need to know:

```text
WriteToFile()
```

---

# 🔥 Why Use an Interface?

The client should depend on the abstraction:

```csharp
IPaymentGateway payment;
```

instead of the concrete implementation:

```csharp
LegacyPaymentGateway payment;
```

This keeps the application flexible.

For example, we could have:

```text
IPaymentGateway
     │
     ├── StripePayment
     ├── PayPalPayment
     ├── PaymentAdapter
     ├── JazzCashPayment
     └── EasyPaisaPayment
```

The client can work with any implementation of `IPaymentGateway`.

---

# 🏗️ Adapter in a Real Application

A common architecture might look like:

```text
                Application
                     │
                     ▼
              IPaymentGateway
                     │
            ┌────────┴────────┐
            │                 │
       StripePayment     PaymentAdapter
                              │
                              ▼
                    LegacyPaymentGateway
```

Your business logic doesn't need to know about the legacy system.

This is one of the main benefits of using an adapter.

---

# ⭐ Advantages

## 1. Reuse Existing Code

You can use an existing class without modifying it.

```text
Existing Class
      ↓
   Adapter
      ↓
Your Application
```

---

## 2. Integrates Third-Party Libraries

You can adapt external libraries to your application's interfaces.

---

## 3. Keeps Client Code Clean

The client only works with the interface it understands:

```csharp
payment.Pay(5000);
```

It doesn't need to know how the underlying implementation works.

---

## 4. Follows Single Responsibility

The adapter has a focused responsibility:

> Convert one interface into another.

---

## 5. Protects Your Application from External APIs

Instead of spreading third-party API calls throughout your application:

```text
Business Logic
     │
     ▼
Your Interface
     │
     ▼
Adapter
     │
     ▼
Third-Party API
```

Only the adapter needs to understand the external API.

---

# ⚠️ Possible Disadvantages

### 1. Adds Another Class

A simple integration may become:

```text
Client → Adapter → Existing Class
```

instead of:

```text
Client → Existing Class
```

### 2. Can Add Complexity

If there are too many adapters, the architecture can become harder to understand.

Use an adapter when the interface mismatch is meaningful and the abstraction provides value.

---

# 🔄 Adapter vs Composite

Both are **Structural Design Patterns**, but they solve different problems.

| Adapter                                     | Composite                           |
| ------------------------------------------- | ----------------------------------- |
| Makes incompatible interfaces work together | Treats objects and groups uniformly |
| Acts as a translator                        | Represents a tree structure         |
| Wraps an existing class                     | Contains other components           |
| Focuses on interface compatibility          | Focuses on part-whole hierarchy     |
| Example: Payment Adapter                    | Example: Folder + File              |

### Easy Memory Trick

```text
Adapter   → Translator
Composite → Tree
```

---

# 🔄 Adapter vs Factory

Beginners often confuse these patterns.

| Adapter                         | Factory                           |
| ------------------------------- | --------------------------------- |
| Connects incompatible classes   | Creates objects                   |
| Solves interface compatibility  | Solves object creation            |
| Wraps an existing object        | Usually creates/returns an object |
| Example: Legacy Payment Adapter | Example: Create Car/Bike          |
| Focus: **Compatibility**        | Focus: **Creation**               |

### Easy Memory Trick

**Factory = Creator 🏭**

```text
Factory
   │
   ├── Car
   ├── Bike
   └── Truck
```

**Adapter = Translator 🌍**

```text
Client
  │
  ▼
Adapter
  │
  ▼
Existing Class
```

---

# 🧠 Easy Memory Trick

Think of a language translator.

You speak:

```text
English
```

The other person speaks:

```text
Urdu
```

You cannot communicate directly.

A translator converts:

```text
English
   │
   ▼
Translator
   │
   ▼
Urdu
```

That's exactly what an Adapter does:

```text
Client Interface
      │
      ▼
   Adapter
      │
      ▼
Existing Interface
```

> **Adapter = Translator**

---

# 🎤 Interview Definition

> **The Adapter Pattern is a structural design pattern that allows classes with incompatible interfaces to work together. It acts as a bridge or translator by converting the interface of an existing class into an interface expected by the client.**

---

# ❓ Common Interview Question

### Q: When would you use the Adapter Pattern?

A strong answer:

> I would use the Adapter Pattern when integrating an existing or third-party class whose interface doesn't match what my application expects. Instead of modifying the existing class, I create an adapter that implements the expected interface and delegates the calls to the existing class.

---

# ❓ How Do You Identify an Adapter?

Look for these things:

```text
1. Client expects an interface
             ↓
2. Existing class provides a different interface
             ↓
3. Adapter sits between them
             ↓
4. Adapter translates/delegates the calls
```

For example:

```text
IPaymentGateway
       ▲
       │
PaymentAdapter
       │
       ▼
LegacyPaymentGateway
```

If you see an existing class being wrapped to make it compatible with another interface, you're probably looking at an **Adapter Pattern**.

---

# 📌 Key Takeaway

The entire pattern can be remembered with one sentence:

> **"Make an existing class work with an interface it doesn't naturally support."**

Or simply:

```text
Incompatible Interfaces
          ↓
       Adapter
          ↓
     Compatible
```

### Final Picture

```text
                    Client
                      │
                      ▼
              IPaymentGateway
                      │
                      ▼
               PaymentAdapter
                      │
                      ▼
          LegacyPaymentGateway
                      │
                      ▼
               MakePayment()
```

The client only knows about `IPaymentGateway`.

The adapter handles all the differences.

That's the **Adapter Design Pattern**.
