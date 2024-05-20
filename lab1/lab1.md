# Lab 1

## Part 1: English to Schema

### Grocery Store Inventory
- Product [SKU (integer), name (string), price (real)]
- Inventory [SKU (integer), quantity (integer)]
- Foreign Key: Inventory.SKU references Product.SKU

### Grocery Store with Aisles
- Product [SKU (integer), name (string), price (real)]
- ProductLocation [SKU (integer), aisleNum (integer), shelfNum (integer)]
- Foreign Key: ProductLocation.SKU references Product.SKU
- Primary Key: (SKU, aisleNum)

### Car Dealership
- Car [VIN (string), make (string), model (string), year (integer), color (string)]
- Salesperson [SSN (integer), name (string)]
- Assignment [VIN (string), SSN (integer)]
- Foreign Keys:
  Assignment.VIN references Car.VIN
  Assignment.SSN references Salesperson.SSN
- Primary Key: (VIN, SSN)

## Part 2: SQL Table Declarations

### SQL-like Table Descriptions of a Library Database

CREATE TABLE Patrons (
  Name (string),
  CardNum (integer) PRIMARY KEY,
  Phone (string)
);

CREATE TABLE Books (
  ISBN (integer) PRIMARY KEY,
  Title (string),  
  PublishedYear (integer)
);

CREATE TABLE Authors (
  AuthorID (integer) PRIMARY KEY,
  Name (string)
);

CREATE TABLE Title_Author (
  ISBN (integer),
  AuthorID (integer),
  PRIMARY KEY (ISBN, AuthorID),
  FOREIGN KEY (ISBN) REFERENCES Books(ISBN),
  FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID)
);

CREATE TABLE Inventory (
  ISBN (integer) PRIMARY KEY,
  TotalCopies (integer),
  AvailableCopies (integer),
  FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

CREATE TABLE CheckedOut (
  CardNum (integer),
  ISBN (integer),
  PRIMARY KEY (CardNum, ISBN),
  FOREIGN KEY (CardNum) REFERENCES Patrons(CardNum),
  FOREIGN KEY (ISBN) REFERENCES Books(ISBN)
);

## Part 3: Fill in Tables

### Cars table:

| VIN |  Make  |  Model | Color | Year |
| --- | ------ | ------ | ----- | ---- |
| 1   | Toyota | Tacoma | Red   | 2008 |
| 2   | Toyota | Tacoma | Green | 1999 |
| 3   | Tesla  | Moddel3| White | 2018 |
| 4   | Subaru | WRX    | Blue  | 2016 |
| 5   | Ford   | F150   | Red   | 2004 |

### Salesperson table:

| SSN |  namme  |
| --- | ------- |
| 123 | Arnold  |
| 456 | Hannah  |
| 789 | Steve   |

### Assignment table:

|    VIN   |   SSN   | aID |
| -------- | ------- | --- |     
| 1        | 123     |1    |
| 2        | 123     |2    |
| 5        | 456     |3    |
| 3        | 789     |4    |
| 4        | 135     |5    |



## Part 4: Keys and Superkeys

| Attribute Sets | Superkey? |   Proper Subsets | Key?  |
| -------------- | --------- | ---------------- | ----- |
| {A1}           | No        | {}               | No    |
| {A2}           | No        | {}               | No    |
| {A3}           | No        | {}               | No    |
| {A1, A2}       | Yes       | {A1}, {A2}       | Yes   |
| {A1, A3}       | No        | {A1}, {A3}       | No    |
| {A2, A3}       | No        | {A2}, {A3}       | No    |
| {A1, A2, A3}   | Yes       | {A1}, {A2}, {A3}, {A1, A2}, {A1, A3} {A2, A3} | No    |


## Part 5: Abstract Reasoning

- True. Any superset of a superkey is also a superkey, as the superkey already uniquely identifies tuples.
- False. A superset of a key may contain redundant attributes and not be a key. Keys must be minimal superkeys.
- True. A superkey and a key can share columns.
- False. All three not being superkeys does not imply one must be a superkey. E.g. {A,B,C} can be a superkey while {A},{B},{C} are -
- True. If no proper subset of {x,y,z} is a key, then {x,y,z} itself must be the minimal set that uniquely identifies tuples, i.e. a key.
