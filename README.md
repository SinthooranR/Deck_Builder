# YTCG_API

Deck Builder API

## Instructions

1. **Create the `appsettings.json` File:**

   - In the root directory of your project, create a new file named `appsettings.json`.

2. **Add the Following JSON Configuration:**

   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "ConnectionStrings": {
       "DefaultConnection": "YOUR_CONNECTION_STRING"
     },
     "Jwt": {
       "Secret": "YOUR_JWT_SECRET"
     }
   }
   ```
