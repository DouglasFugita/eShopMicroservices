# Welcome to eShopMicroservices!
Hi! This study project was created to improve the project [AspnetMicroservicesExample](https://github.com/DouglasFugita/AspnetMicroservicesExample/), based on udemy course [Microservices architecture and implementarion on dotnet](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/).

The project will start based on .net 7.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=DouglasFugita_eShopMicroservices&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=DouglasFugita_eShopMicroservices)

[![.net + SonarCloud](https://github.com/DouglasFugita/eShopMicroservices/actions/workflows/main.yml/badge.svg)](https://github.com/DouglasFugita/eShopMicroservices/actions/workflows/main.yml)

## Tests
- [x] [Sonarqube](https://www.sonarsource.com/products/sonarqube/downloads/) was containerized to be used as the code quality checker.
- [x] [Sonarlint](https://www.sonarsource.com/products/sonarlint/) was configured at Visual Studio 2022 integrating with Sonarqube
- [x] [OpenCover](https://github.com/OpenCover/opencoverCode) is the tool responsible for the code coverage reports generation.
- [x] [Bogus](https://github.com/bchavez/Bogus) to faker data on Tests.
For the local integration with Sonarqube, the commands used to are:
```
dotnet sonarscanner begin /k:"eShopMicroservices"
/d:sonar.host.url="http://localhost:9100" 
/d:sonar.login= [sonarqube code]
/d:sonar.cs.opencover.reportsPaths=coverage.xml 
 
dotnet build --no-incremental
 
[OpenCoverPath]\OpenCover.Console.exe -target:"dotnet.exe" -targetargs:"test --no-build" -returntargetcode -output:coverage.xml -register:user

dotnet sonarscanner end /d:sonar.login=[sonarqube code]
```
## CI
- [x] Github Action

## Logs
- [x] [ELK Stack](https://www.elastic.co/pt/elastic-stack/) to centralized logging
- [ ] [Beats](https://www.elastic.co/pt/beats/) to retrieve data from log files
- [x] [Serilog](https://serilog.net/) for "log file convenience" connecting directly to Elastic

## Metrics
- [x] [OpenTelemetry](https://opentelemetry.io/) cloud native option to collect metrics
- [x] [Prometheus](https://prometheus.io/) time series database to store metrics
- [x] [Grafana](https://grafana.com/) observability platform to visualize Prometheus data

## Tracing
- [x] [OpenTelemetry](https://opentelemetry.io/) cloud native option to send tracing
- [x] [Jaeger](https://www.jaegertracing.io/) end-to-end distributed tracing

## Ports
- Portainer: http://localhost:9000
- SonarQube: http://localhost:9100/
- MongoExpress: http://localhost:8081/
- Kibana: http://localhost:9010/
- Prometheus: http://localhost:9090/
- Grafana: http://localhost:3000/
- Jaeger: http://localhost:16686/