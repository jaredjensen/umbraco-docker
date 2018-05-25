# Umbraco in Docker

## Getting Started

1. Download [Umbraco](https://umbraco.com/) and unzip to **src/umbraco**.
1. Build the Umbraco container:  
    `docker build -t umbraco-admin ./src/umbraco`
1. Select a strong password for the SQL `sa` account and build the SQL Server container:  
    `docker build --build-arg saPassword=SA_PASSWORD -t sql-express ./src/sql`
1. Start all containers:  
    `docker-compose up`
1. Use `docker inspect` to find the IP address of the SQL Server container.  Connect using SSMS and create a new, empty database named **umbraco** in `C:\data`.
1. Use `docker inspect` to find the IP address of the Umbraco container, then navigate to http://**{umbraco-ip}**:8000.
1. Select the **Customize** option, then select **Custom connection string** and enter:  
    `Data Source=umbraco-docker_sql_1;Catalog=umbraco;User Id=sa;Password=SA_PASSWORD;MultipleActiveResultSets=True`

