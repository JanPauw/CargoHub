# CargoHub

A web app developed for logistics businesses to help keep track of their cargo fleet, customers, and employees. 

Originally the web app was created with ASP.NET Framework, but as I learned more efficient and modern languages and solutions, I am actively updating the system to ASP.NET Core 6. 
This web app also uses Entity Framework Core along with dependency injection, but the SQL Database used to store all the data is self-hosted by me, using Docker.

[Current Version Live Demo](https://cargohubweb.azurewebsites.net)

## Current Features
- Admin can add Employees to register on the system with their own unique employee number.
- Different depots can be loaded onto the system. These will be used to determine where cargo is coming from and going to.
- Customers can be loaded onto the system.
- Orders can be loaded on the system, and assigned to a driver and a customer.
- Order's status can be changed between: 
  - "New" (New Order - Not assigned to a driver)
  - "Assigned" (A driver has been assigned)
  - "Complete" (The delivery to the destination depot has been made)

## Features to Implement
- Truck Management. 
  - Trucks can be assigned to specific employees.
  - Trucks can be booked for servicing.
  - Trucks are tracked at which depot they last were.
- Driver Management
  - View which Drivers are linked to which trucks.
  - Show assigned orders for that specific driver.
- Timesheet Management
  - Keep track of employees work times.
  - See log of any employees work times.
- Reporting
  - You will be able to generate reports based on multiple features.
