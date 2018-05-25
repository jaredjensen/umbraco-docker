# Umbraco in Docker

## Getting Started

1. Download [Umbraco](https://umbraco.com/) and unzip to **src/umbraco**.
1. Build the Umbraco container:  
    `docker build -t umbraco-admin ./src/umbraco`
1. Start all containers:  
    `docker-compose up`
1. Get the Umbraco container IP address:  
    `docker inspect –format='{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}'  umbraco-admin`
1. Navigate to http://**{umbraco-ip}**:8000/umbraco
