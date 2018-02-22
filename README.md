# AzureWebJobTest
Testing unexpected behavior for queue triggered azure webjobs with .net core 2.0

This is a small sample project to demonstrate some strange behavior with database connections when using EF Core in a continuous Azure webjob.

The sample is two projects.  `AzureWebJobTest.WebJob` is a webjob that is triggered by simple items in an Azure queue. The job performs a long-ish running sql query to demonstrate that you can quickly get the webjob to fail.

- If the sql connection string is designated with `MultipleActiveResultSets=False;` and more than one invocation is executed the job will fail telling you the MultipleActiveResultSets must be enabled.
- If `MultipleActiveResultSets=True;` the job will work fine up to about 15-20 simultaneous invocations.  Once you reach that number (or higher) random jobs will fail with a sql connection closed error.
