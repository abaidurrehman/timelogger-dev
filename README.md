## Development

To run this project you will need both .NET Core v3.1 and Node installed on your environment.

Server - `dotnet restore` - to restore nuget packages, `dotnet build` - to build the solution, `cd Timelogger.Api && dotnet run` - starts a server on http://localhost:3001. You can download Visual Studio Code. The project was tested on MacOS High Sierra and Windows 10.

The server solution contains an API only with a basic Entity Framework in memory context that acts as a database.

Client - `npm install` to install dependencies, `npm start` runs the create-react-app development server
