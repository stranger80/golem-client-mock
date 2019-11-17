# golem-client-mock
Mockup of Golem Client with all the standardized APIs.

## Building the project
### Windows
### Ubuntu
#### Prerequisites
- .NET Core SDK 2.2
- NPM (Node.js package manager) - required for React frontend application

**Setup prerequisites:**
(As per [https://dotnet.microsoft.com/download/linux-package-manager/ubuntu16-04/sdk-current], but note .NET Core 2.2. is required rather than the latest version) :

```
wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

Get .NET Core 2.2. SDK

```
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.2
```

Get NPM:

```
sudo apt install npm
```

Get project sources:
```
git clone https://github.com/stranger80/golem-market-api.git
```

Get NPM dependencies for client app:
```
cd golem-market-api/GolemCLientMockAPI/ClientApp
npm install
```


