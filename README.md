

# PrezUpServerAPI

**Version:** 1.0.0

## Overview  
PrezUpServerAPI is the backend service for the PrezUp system, built using .NET 9. It provides a RESTful API for managing user presentations, analyzing presentation quality, and storing feedback.  

## Features  
- User authentication using JWT.  
- Presentation management (create, update, delete).  
- File storage in AWS S3.  
- Integration with PrezUpServerNLP for AI-driven feedback analysis.  

## Requirements  
- .NET 9  
- MySQL database  
- AWS S3 credentials for file storage  

## Installation  
1. Clone the repository:  
   `git clone https://github.com/yourusername/PrezUpServerAPI.git`  
2. Navigate to the project directory:  
   `cd PrezUpServerAPI`  
3. Restore dependencies:  
   `dotnet restore`  
4. Build the project:  
   `dotnet build`  
5. Run the server:  
   `dotnet run`  

## API Usage  
The API runs at `http://localhost:5000`. You can test endpoints using Postman or cURL.  

## Contribution  
Contributions are welcome! Feel free to submit issues or pull requests.  

## License  
This project is licensed under the MIT License.  

## Author  
Chaya Ziv  


