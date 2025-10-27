# QuickFeedback: Full-Stack Guestbook App

A full-stack guestbook application built with a .NET 8 backend, a simple JavaScript frontend, and deployed to a multi-stage (Dev/QA/Prod) environment on Microsoft Azure using GitHub Actions. This project demonstrates a complete, professional software development lifecycle: from local development to a fully automated, production-grade CI/CD pipeline with manual approvals.


---

## Features

- **Full-Stack:** A single .NET 8 application serves both the backend API and the frontend UI.
- **API:** A simple GET / POST REST API for managing guestbook messages.
- **Database:** All messages are persisted in an Azure SQL Database.
- **Automated Pipeline:** A complete CI/CD pipeline using GitHub Actions.
- **Multi-Stage Environments:** A professional branching strategy (`dev`, `staging`, `main`) that deploys to isolated Azure App Service Slots, each with its own database.
- **Deployment Gating:** Manual approval is required to promote builds from QA to Production.

---

## Tech Stack

- **Backend:** .NET 8 Web API, Entity Framework Core 8
- **Frontend:** JavaScript, HTML5, CSS3
- **Database:** Azure SQL Database
- **Cloud Platform:** Azure App Service (Linux)
- **DevOps:** GitHub Actions

---

## Professional Deployment Architecture

### 1. Git Branching Strategy

- **dev:** All feature branches are merged here. A push triggers an automatic deployment to the Development environment.
- **staging:** When `dev` is stable, a Pull Request is made to `staging`. A push triggers a deployment to the QA environment.
- **main:** When `staging` is approved by QA, a Pull Request is made to `main`. A push triggers a deployment to the Production environment.

### 2. Azure Infrastructure

- **Azure App Service:** A single App Service hosts the application.
- **Deployment Slots:** Slots create isolated environments without paying for new App Services.
  - **dev Slot:** Development environment.
  - **staging Slot:** QA environment.
  - **Production Slot:** Live, public-facing application.
- **Databases:** Each environment has its own isolated database.
  - `QuickFeedbackDB-Dev`
  - `QuickFeedbackDB-QA`
  - `QuickFeedbackDB` (Production)
- **Slot Configuration:** Each slot's `DefaultConnection` string is "pinned" as a Deployment slot setting, ensuring the staging slot always points to the QA database even after swaps.

### 3. CI/CD Pipeline (GitHub Actions)

The `.github/workflows/deploy.yml` file defines a three-job pipeline:

- **deploy-dev:**
  - Triggers: On push to `dev` branch.
  - Deploys to: dev slot.
  - Approval: None (for fast developer feedback).

- **deploy-qa:**
  - Triggers: On push to `staging` branch.
  - Deploys to: staging slot.
  - Approval: Manual Approval Required. A reviewer must approve the deployment in GitHub Actions.

- **deploy-prod:**
  - Triggers: On push to `main` branch.
  - Deploys to: Production slot.
  - Approval: Manual Approval Required. A reviewer must grant final approval for the production release.

---

## Key Troubleshooting & Learning

- **Problem:** The deployed application could not connect to the Azure SQL Database, showing `Microsoft.Data.SqlClient.SqlException` in the logs.
- **Root Cause:** The Azure SQL Server firewall was blocking the App Service's outbound IP addresses.
- **Solution:** Go to the App Service -> Properties, copy all Additional Outbound IP Addresses, and add them to the Azure SQL Server -> Networking -> Firewall rules.

---




