# Azure Serverless Principal
- Azure Functions
	- Azure Based Serverless Object Model
	- .NET 6 Runtime
		- Use dotnet.exe to load the function code and execute
	- .NET 6, 7 and V4 Isolated Process
		- Use an isolated dedictaed Process to execute the function
- .NET 6 Runtime
	- Mirosoft.NET.Sdk.Functions
		- An object model that has
			- All Triggers
			- Execution
			- Resource Management
				- Dependencies
	- FunctionNameAttribute class
		- The class taht is applied on the method to set behavior to that method as a function method
			- function method, is the method that will be invoked inside the runtime based on Trigger
	- local.settings.json
		- Configuration file for local execution
		- This will contains
			- Connection String to Storage
			- Any other configuration values which we cannot /  should not store in code
	- host.json
		- This will nbe usd by Azure DeploymentEnvironment
		- Same as local.settings.json for Azure deployment
- For deploying on Azure follow steps as mentioned below
	- MUST have Azure Subscription
		- https://azure.microsoft.com/en-us/free/
	- Create a resource group
		- A Logical Container (Like Folder) where all the Azure Resources e.g. Storage, VM, API App, Databse etc. are stored
	- We can create an Azure Function using Azure Portla inside the resource group
	- Selecting Deployment Plan for Azure Function
		- Consumption (Serverless)
			- Optimized for serverless and event-driven workloads.  	
				- CPU, RAM and other resource will be allocated only when the trigger is fired
		- Functions Premium
			- Event based scaling and network isolation, ideal for workloads running continuously.  
			- e.g.
				- Process of large Datasets
		- App service plan
			- Fully isolated and dedicated environment suitable for workloads that need large SKUs or need to co-locate Web Apps and Functions.
			- Use Azure function as a Full Implementation of the API Apps
- HttpTriggers
- BLOBTriggers
- Durable Functions
	- Chain
	- FanOut/FanIn
	- Monitor
- Security
- Deplopyment
- Logic Apps
- Availability
	

# Azure Function App Development

1. Configure Data Access Layer
	- ORM
		- Entity Framework Core (EF Core)
			- Install Packages for the Project from dotnet CLI
			- Install EF Global Tool
				- dotnet tool install --global dotnet-ef
			- Install Following Packages 
				- dotnet add package Microsoft.EntityFrameworkCore -v 6.0.13
				- dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.13
				- dotnet add package Microsoft.EntityFrameworkCore.Relational -v 6.0.13
				- dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.13
				- dotnet add package Microsoft.EntityFrameworkCore.Tools -v 6.0.13
		- Generate Model Classes and Data Access using EF Core
			- dotnet ef dbcontext scaffold "DATABASE-CONNECTION-STRING" Microsoft.EntityFrameworkCore.SqlServer -o Models
			- dotnet ef dbcontext scaffold "Data Source=.;Initial Catalog=Company;Integrated Security=SSPI" Microsoft.EntityFrameworkCore.SqlServer -o Models

		- Package for Dependency Container
			- Microsoft.Azure.Functions.Extensions

# Function Authorization Level
	- Admin
		- aka Master, the Mastre Key is needed to call functions in App
		- This is also known as 'host' key
	- Function
		- Each function will have seperate Key
		- We can share seperate key to client to make sure that they can access only function of  which key is available with them
	- Anonymous (No recommended)
		- Anybody can directly access function
		- Any Valid HTTP request can access fucntion
# Using Azure Storage Services with Functions
	- Create an Azure Storage Account
		- The Service That manages Azure Storage Services
			- Table
				- NoSQL Data Store
				- Schema Independant (Free) data store
				- Accessible using TableTrigger
			- BLOB
				- Binary Files Stores
					- Text, Xml, Json, Csv, Images, and Documents
				- Page Blobs
					- Streaming
					- Size if More than 10 GBs
				- BlobkBLOBS
					- File Operations are performed by dividing the BLOB into Chunks 
				- Accessibel using BlobTrigger
			- Queue
				- Messaging
				- Binary Encoded Message
				- 7 Days of Store
				- Accessible using QueueTrigger
			- Files
				- Large Files
				- VHD File
		- Use the Following package to access Queue Storage
			- Azure.Storage.Queues