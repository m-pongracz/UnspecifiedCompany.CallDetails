# CallDetails Service

## Maturity checklist

- [ ] Monitoring
- [ ] Logging
- [ ] Health checks
- [x] Integration tests
- [ ] Performance tests
  - should be easy to implement since integration tests run against a real database in docker
- [ ] Security
- [ ] CI/CD
- [ ] Deployment
- [ ] Documentation
  - some API documentation is in swagger just as a demonstration of how I would do it
- [ ] Resiliency
  - some resiliency is ensured by doing `InsertOrUpdate` in the upload EP so the upload can at least be retried

## Description

This service is responsible for storing and retrieving Call Detail Records.

### Project structure

- `src` folder
  - DDD layers
    - Domain
    - Application
    - Persistence
      - Persistence.Migrations
        - Contains EF migrations and migration runnable
    - WebApi*
- `test` folder
  - `Tests.Integration`
    - Contains integration tests
  - `CsvGenerator`
    - Contains a tool to generate CSV files with random or specific data
    - Contains a runnable to create a CSV file on your system
  - `WebApi.Client`
    - C# API definition is generated from swagger.json so it can be used in tests
- `tools`
  - `local-compose.yml`
    - contains a MSSQL service so you can run your service locally

## Tech stack
- Entity Framework
- MSSQL
- .NET 7
- Docker
- xUnit
  - FluentAssertions
  - TestContainers
  - Respawner
  - NSwag
    - C# API definition is generated from swagger.json so it can be used in tests

## Testing

- The solution contains fully automated E2E (input and output is tested with EPs) tests using docker.

### Prerequisites

- docker with linux containers

### Running the tests

- Run the tests in your IDE.
- There is an optional env variable `KeepDatabaseBetweenTests` which when set to true will keep the containers from being deleted
so you can inspect the database after the tests have run. This is useful for debugging. The default is false.

## Notes

### Constraints

- My time.
- I wanted to use a real database for the integration tests so I could test the SQL queries.
- Context in which the service will be used.

### Technology choices

- MSSQL database
  - I chose MSSQL because I had a project template that I could use to get the project up and running quickly, but any SQL DB would have worked.
  - From the nature of the data a document DB could have been considered as well, but since we should be ready to "iteratively add features"
SQL is the safest choice.

#### Upload EP

- The main challenge was to tackle big file uploads. The upload EP streams CSV rows by batches into the DB so the whole file is not loaded into memory.
- I used a `EFCore.BulkExtensions` nuget package to insert the rows into the DB. This is a moderately fast way to insert rows into the DB and it is easy to setup as well.

#### Querying

- There is nothing special, just an index over `{ CallerId, CallDate, CallType }`.
- `reference` is a PK (clustered index) so querying by this will always be fast.
- I tested the queries on tens of millions of records and they are fast. This is still nothing compared to the amount of data that a real system would have to handle but it should be a good start (for this example project anyway).

### Assumptions

- Uploader should be able to retry the upload if it fails
- Example CSV does not contain `call_type` so I assume if it is missing it is a `Domestic` call
- `CallDetail` domain object naming might be a bit unfortunate if "CDR" is a technical term, but I assume this naming decision would be subject to a discussion so a proper name can be used.
To me the *Record* part of the abbreviation is a little redundant as it is a record in the DB.


### Instructions for running the project

- Use `Giacom.CallDetails.WebApi` launch settings to run the API
- Use Swagger UI to use the EPs
- Alternatively run the integration tests in your IDE
- I used Intellij Rider to develop the solution but I didn't use anything exotic so everything should work in VS as well

### Future considerations/enhancements

- The upload can be optimized further.
  - `SqlBulkCopy` should be even faster.
  - Parallelism.
  - Dropping indexes and recreating them after the upload.
    - This can be tricky if the query EPs should be readily while upload is running. Read replicas could be used to solve this.
- Table is locked so simultaneous uploads aren't supported.
  - Data could be inserted into a temporary table and then merged into the main table.  
- Upload process
  - The upload process is very simple and it doesn't handle errors well.
    - If the upload fails there is at least the possibility to reupload the same file again and it will not crash as there is `InsertOrUpdate` used
but this makes the upload slower as well.
    - Further development would depend on who the uploader is. If the uploader is "smart" we can send him the last row which was uploaded successfully and he can continue from there.
    - We could also just let the uploader upload the file into a object/blob storage and then we can process the file at our own pace, which might be preferred as we shouldn't burden the uploader too much.
      - There would be a `LongRunningTask` entity in our system which would run when the file is uploaded and we get it's url (or we can just do this blob upload ourselves).
      - A job would then grab this LongRunningTask and process the file stream from the blob storage while saving the progress. This way we can continue from where we failed as soon as e.g. DB transient error is resolved.
      - We could also chunk this file and then process these chunks in parallel.
- Queries
  - Duration statistics can be precalculated for each day and inserted into a separate table, but the complexity of this code depends on the nature of the data we receive in the CSV file.
    - E.g. if the data contains CDRs from just last 24 hours we can keep a dictionary of duration per day & per type while we process the CSV stream and update the other table at the end of processing.
    - If the data contains CDRs from a longer period we can do the same but update the table when our dictionary gets too large.
  - There should be an "archivation" job. I think there is not much use for records older than a year or something like that so we can offload some data to "cold storage".
  - DB could be maybe sharded, but it's important to know more about our API use case first.
- Code
  - The "query part" of the CQRS pattern could be improved further on the repository side of things.
  - Client generator configuration in tests could use some tuning, especially in nullability and method names.
  - Code in the controller could be reused a little better.
  - Exceptions should be converted into appropriate status codes.
  - Client generator can be setup to run automatically on build.
  - More tests.
  - More documentation.
- Other
  - Everything that is not checked in the maturity checklist.

## Closing thoughts on this exercise

It was pretty fun. I have never had to process CSV files this large or come up with how to query I assume hundreds of millions of rows in this way. I think I made the right choices by streaming the rows into the DB and the data is indexed in a "good enough" way so this should be fine for an MVP.
I also had fun with the analysis on potential new features and improvements. The code is in my opinion very extendable and is setup in a very good way to allow further rapid development especially thanks to how the integration testing is done.