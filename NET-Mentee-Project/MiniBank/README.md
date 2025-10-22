# 🏦 Console Banking System

## Objective
Create a **console application** that simulates a simple banking system.  
The system should store a list of bank accounts in memory and display a console menu that allows the user to perform various operations.

## Menu Options
1. **List Accounts** – Display all existing accounts.  
2. **Create Account** – Add a new account to the system.  
3. **Deposit** – Deposit a certain amount into an account.  
4. **Withdraw** – Withdraw a certain amount from an account.  
5. **View Statement** – Display the transaction history for a specific account.  
6. **Run Month-End** – Apply monthly rules and interest for all accounts.  
7. **Exit** – Exit the application.  

## Account Types

### 1. CheckingAccount
- Allows overdraft (negative balance) up to a specified limit (e.g., -200).  
- No monthly interest is applied.  

### 2. SavingsAccount
- Does **not** allow overdraft (balance cannot go below 0).  
- At month-end, if the balance is **greater than 0**, a **1% interest** is added.  

### 3. LoanAccount
- The balance is **negative** (represents debt).  
- A **deposit** represents a **repayment** (balance moves closer to 0).  
- A **withdrawal** represents a **new loan** (balance becomes more negative).  
- At month-end, a **2% interest** is applied to the negative balance.  


## Validation Rules
a. Checking accounts can have a negative balance down to **-200**.  
b. Savings accounts earn **1% monthly interest** if their balance is positive.  
c. Loan accounts accumulate **2% monthly interest** on their outstanding debt.  
d. All user inputs must be validated (IDs, amounts, text fields, etc.).  
