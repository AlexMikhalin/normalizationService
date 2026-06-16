# Normalization Service

Backend prototype for accounting data normalization.

The API reads five mocked accounting source formats from JSON, normalizes them into one canonical accounting entry format, stores the normalized rows in PostgreSQL through EF Core, and exposes them through one GET endpoint.

## Stack

- .NET 10
- ASP.NET Core Minimal API
- EF Core with Npgsql
- PostgreSQL in Docker

## Source Formats

The mock feed contains five intentionally different accounting formats:

- `QuickLedgerInvoice` - sales invoices with subtotal, tax, total due, and payment state.
- `CloudBooksJournalEntry` - journal lines with debit/credit fields and approval state.
- `PayFlowSettlement` - payment settlements with gross, fee, net, and tax withheld.
- `TaxDeskExpense` - vendor expenses with VAT and deductible flag.
- `BankBridgeStatementLine` - bank statement lines with direction, bank account suffix, and matching reference.

All records are normalized into `NormalizedAccountingEntry`.

## Run With Docker

From the repository root:

```powershell
docker compose up --build
```

The API will be available at:

```text
http://localhost:8080
```

Swagger UI will be available at:

```text
http://localhost:8080/swagger
```

PostgreSQL will be available on:

```text
localhost:5432
```

## Postman Endpoint

Use this request:

```http
GET http://localhost:8080/api/normalized-entries
Accept: application/json
```

On first startup the service creates the database schema with EF Core `EnsureCreated()`, reads `NormalizationService.Api/Data/Mock/accounting-feed.json`, normalizes the records, and seeds PostgreSQL.

## Local Run Without Docker

Start PostgreSQL yourself with these credentials:

```text
Database: normalization
User: normalizer
Password: normalizer_password
Port: 5432
```

Then run:

```powershell
dotnet run --project NormalizationService.Api
```

## Tests

Run the application tests with:

```powershell
dotnet test NormalizationService.slnx
```
