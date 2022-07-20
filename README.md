## Tedu AspNetCore Microservice:

Go to folder contrain file `docker-compose`

1. Using docker-compose
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build
```


## Application URLs - LOCAL Environment (Docker Container):

- Product API: http://localhost:6002/api/products
- Customer API: http://localhost:6003/api/customers
- Basket API: http://localhost:6004/api/baskets

## Docker Application URLs - LOCAL Environment (Docker Container):

- Portainer: http://localhost:9000 - username: admin ; pass: admin123456789
- Kibana: http://localhost:5601 - username: elastic ; pass: admin
- RabbitMQ: http://localhost:15672 - username: guest ; pass: guest

2. Using Visual Studio 2022
- Open aspnetcore-microservice.sln - `aspnetcore-microservice.sln`
- Run Compound to start muti projects

## Application URLs - DEVELOPMENT Environment
- Product API: http://localhost:5002/api/products
- Customer API: http://localhost:5003/api/customers
- Basket API: http://localhost:5004/api/baskets

------------
## Application URLs - PRODUCTION Environment

------------

## Package References

## Install Environment

- https://dotnet.microsoft.com/download/dotnet/6.0
- https://visualstudio.microsoft.com

## References URLs 
- https://github.com/jasontaylordev/CleanArchitecture


## Docker Commands:

docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build

## Useful commands:

- ASPNETCORE_ENVIRONMENT=Development dotnet ef database update
- dotnet watch run --environment "Development"
- dotnet restore
- dotnet build
- Migrations commands:
    dotnet ef migration add "SampleMigration"
