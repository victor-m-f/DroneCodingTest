# Project Overview

This project is designed to solve a drone delivery optimization problem, where each drone can carry a certain weight and deliver packages to multiple locations. The main aim is to minimize the number of trips required by each drone to complete all deliveries. This is essential, as each time a drone returns to the home base, refueling and reloading costs are incurred.

## 🚀 Live Demo

👉 [Try the application live](https://dronecodingtest.azurewebsites.net/)

> This application is automatically deployed using GitHub Actions and is hosted on Azure.

## Tech Stack

- **Backend**: .NET 6, C# 10
- **Testing**: xUnit 2.4.2
- **Frontend**: Blazor with Radzen.Blazor 4.18.1

## Project Structure

- **Domain**: Contains all the domain entities (Drone and Location).
- **Application (UseCases)**: Handles the business logic and use cases.
- **Presentation**: Holds the front-end and API logic.

## Customizations

Two types of delivery strategies have been implemented:
1. **No Travel Time**: Focuses on reducing the number of trips by using the drone with the highest payload capacity.
2. **Has Travel Time**: Considers a fixed travel time for each drone and aims to distribute the deliveries across multiple drones concurrently.

You can toggle between these strategies when hitting the endpoint.

## How to Run

1. Restore all NuGet packages.
2. Build the project.
3. Run the .Server project.
4. Execute unit tests to verify everything is working as expected.

## Testing

Unit tests are available in the Application layer to ensure the logic is working correctly.

## Coding Standards

- Keep the code as DRY as possible.
- Proper indentation for readability.
- Stick to the SOLID principles and clean architecture.

## Libraries Used and Why

### MediatR
Used for decoupling the components, making it easier to use cases calls. It simplifies the code structure and makes it more maintainable.

### Humanizer
It improves the user experience converting text into a format that's easier to read and understand.

### FluentAssertions
Chosen for unit testing to write more expressive assertions. Makes the tests easier to read and understand.

### Moq
Employed for mocking objects in unit tests. It isolates the code under test, making sure external dependencies don't affect the outcomes.

