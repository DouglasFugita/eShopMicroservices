# Welcome to eShopMicroservices!
Hi! This study project was created to improve the project [AspnetMicroservicesExample](https://github.com/DouglasFugita/AspnetMicroservicesExample/), based on udemy course [Microservices architecture and implementarion on dotnet](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/).

The project will start based on .net 7.

## Tests
- [x] [Sonarqube](https://www.sonarsource.com/products/sonarqube/downloads/) was containerized to be used as the code quality checker.
- [x] [Sonarlint](https://www.sonarsource.com/products/sonarlint/) was configured at Visual Studio 2022 integrating with Sonarqube
- [x] [OpenCover](https://github.com/OpenCover/opencoverCode) is the tool responsible for the code coverage reports generation.
For the local integration with Sonarqube, the commands used to are:
```
dotnet sonarscanner begin /k:"eShopMicroservices"
/d:sonar.host.url="http://localhost:9100" 
/d:sonar.login= [sonarqube code]
/d:sonar.cs.opencover.reportsPaths=coverage.xml 
 
dotnet build --no-incremental
 
[OpenCoverPath]\OpenCover.Console.exe -target:"dotnet.exe" -targetargs:"test --no-build" -returntargetcode -output:coverage.xml -register:user

dotnet sonarscanner end /d:sonar.login=[sonarqube code]
