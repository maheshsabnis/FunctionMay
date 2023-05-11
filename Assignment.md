# Date:09-May-2023

1. Clone the Git Repository
https://github.com/maheshsabnis/FunctionMay.git
2. Complete the Function by adding a new Service for Employee Operations and New Http Trigger Function for GET/POST/PUT/DELETE Operations 


# Date: 10-May-2023
1. Modify the EmployeeService created on Date:09-May-2023  to pass the Employee data to different Queues created for Each Department e.g. it-queue, hr-queue, etc.
2. Create Azure Fucntions with Queue Trigger to read data for Employees from Different queues and Show them in such a way that, the Application Should show employees per department
3. If the Department Capacity is full with Employees e.g. if IT Department has capacity of 100 Employees and there are already 100 Employees present then The new employee will not be added into Employee Table but it will be added in queue with name as 'no-dept-employees' and the azure fucntion should read and show that also  

# Date: 11-May-2023

1. CReate aDurable Function with the FanOut/FanIn Patrtern that performs the following operations
	- FanOut will Read Files (text/xml/json) from source path (hardcode it)
	- FanIn will Open each file and te read its contents, aftre reading contents from these files, they will be written to the Queue as new message 
