# CSCA5028 Final Project - "Retail Real Time Sales"

## By Nikhil Rajwade

## 1. Overview

The goal of this project is to create a real time sales dashboard for a retail company. The dashboard will be updated every 5 minutes with the latest sales data. The dashboard will be created using the following technologies:
.Net Core 7.0, ASP.NET Core, MSTests, SQL Server Express, Prometheus, RabbitMQ, Grafana and Docker.

## 2. Architecture

The architecture of the project is as follows:


![Architecture](

## 3. Data Structure

The data structure of the project is as follows:

![Data Structure](

## 4. Data Flow

The data flow of the project is as follows:


![Data Flow](

## 6. Components

### Point-Of-Sale (POS) System

The POS system is a .Net Core 7.0 console application that generates random sales data every 5 minutes. The data is then sent to the RabbitMQ message broker.

### credit-card-processor

The credit-card-processor is a .Net Core 7.0 console application that consumes the sales data from the RabbitMQ message broker. The data is then processed and stored in the SQL Server Express database.

### sales-collector

The sales-collector is a .Net Core 7.0 console application that consumes the sales data from the RabbitMQ message broker. The data is then processed and stored in the SQL Server Express database.

### sales-web-app

The sales-web-app is a ASP.NET Core web application that displays the sales data in a dashboard. The dashboard is updated every 5 minutes with the latest sales data.

### Evidence for the Project Rubric
#### 1. Web Application basic form, reporting, and data visualization
![Web Application](

#### 2. Data Collection
![Data Collection](

#### 3. Data Analyzer

#### 4. Unit Tests
![Unit Tests]() ```

#### 5. Data Persistence

#### 6. REST Collaboration

#### 7. Product Environment

#### 9. Integration Tests

#### 10. Using Mock Objects or Test Doubles

#### 11. Continuous Integration

#### 12. Production monitoring instumentation

#### 13. Event Collaboration messaging

#### 14. Continuous Delivery

## Instructions to run the project



