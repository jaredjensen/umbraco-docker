# Umbraco in Docker

## Overview

The important points of this solution are:

* Use a single Umbraco instance for administration and scheduling (aka "master server")
* Use multiple load-balanced Umbraco instances for rendering (aka "front-end" servers)
* All Umbraco instances point at the same SQL Express container
* Static content is stored on a shared file system

To get things set up, you will:

1. Start the SQL container and create a blank database
1. Run Umbraco on the host and complete the setup wizard
1. Build the Umbraco Docker image
1. Run everything locally with docker-compose

## Setup

### Start the SQL container and create a blank database

Select a strong password for the `sa` account.  Then run a SQL container to be accessible at `sqlexpress2017` from the other containers and with a host volume mounted at `C:\data`:

```bash
docker run -d -h sqlexpress2017 -e ACCEPT_EULA=Y -e sa_password="[YourStrongPassword]" --mount type=volume,source=umbraco_data,target=C:/data --name sqlexpress2017 microsoft/mssql-server-windows-express:2017-latest
```

Now inspect the container to get its IP address:

```bash
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" sqlexpress2017
```

Connect using SSMS and create a new, empty database:

```sql
USE [master]
GO

CREATE DATABASE [umbraco] ON PRIMARY 
( NAME = N'umbraco_data', FILENAME = N'C:\data\umbraco.mdf' )
LOG ON
( NAME = N'umbraco_log', FILENAME = N'C:\data\umbraco_log.ldf' )
GO
```

### Run Umbraco on the host and complete the setup wizard

Download [Umbraco](https://our.umbraco.org/download/) and unzip to **src/umbraco/site**.  [Run the site in IIS Express](https://our.umbraco.org/documentation/Getting-Started/Setup/Install/install-umbraco-with-vs-code) (or however you'd like) and proceed through the setup wizard.

At the first step, select **Customize**.  Next, select **Custom connection string** and enter the following (substituting the password):

```text
Server=sqlexpress2017;Database=umbraco;User Id=sa;Password=[YourStrongPassword];MultipleActiveResultSets=True
```

**Important!** Allow Umbraco to generate a machine key for your site in the last step.  Installing a starter kit site is optional.

You can now shut down IIS Express and stop/destroy the SQL container:

```bash
docker stop sqlexpress2017
docker container rm sqlexpress2017
```

### Build the Umbraco Docker image

Now that our Umbraco web.config is updated, we can build our Umbraco Docker image.  This will include our custom bits in `App_Code/LoadBalancing.cs`:

```bash
docker build -t umbraco ./src/umbraco
```

### Run everything locally with docker-compose

At this point, the necessary components should be in place:

* Initialized Umbraco database stored in host volume `umbraco_data`
* Umbraco image that can be used as either master or slave

To fire everything up, run:

```bash
docker-compose up
```

You should now be able to access your Umbraco site at http://localhost:8000.

## References

* [Umbraco flexible load balancing](https://our.umbraco.org/documentation/getting-started/setup/server-setup/load-balancing/flexible)
* [Explicit master scheduling server](https://our.umbraco.org/documentation/getting-started/setup/server-setup/load-balancing/flexible-advanced#explicit-master-scheduling-server)