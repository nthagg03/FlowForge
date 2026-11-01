# FlowForge

FlowForge is a workflow automation platform built with ASP.NET Core Web API (.NET 10), Entity Framework Core, and SQL Server Express.

---

## Architecture Overview

FlowForge follows a clean layered architecture:
```
Controllers  -->  Services  -->  Repositories  -->  Database (SQL Server Express)
                      |
                ExecutionEngine
```

- **Controllers**: Thin HTTP controllers that perform request binding and status code mapping.
- **Services**: Business services orchestration (`WorkflowService`, `StepService`, `TriggerService`).
- **Repositories**: Data access layer isolating Entity Framework Core (`WorkflowRepository`, `StepRepository`, `TriggerRepository`, `ExecutionRepository`).
- **Services/ExecutionEngine**: Core workflow execution engine:
  - `IWorkflowExecutor` / `WorkflowExecutor`: Manages workflow lifecycle, step execution loops, retries, and failure handling.
  - `IActionHandler`: Action contracts and concrete implementations (`WriteLogActionHandler`, `SendEmailActionHandler`).
  - `IActionHandlerResolver` / `ActionHandlerResolver`: Singleton resolver dispatching actions by `ActionType`.
- **Infrastructure**: Centralized exception handling (`GlobalExceptionHandler`) implementing `IExceptionHandler` and outputting standard RFC 9457 `ProblemDetails`.

---

## Key Features & Design Decisions

### 1. Centralized Error Handling (`GlobalExceptionHandler`)
- Custom domain exceptions (`NotFoundException`, `ConflictException`, `BadRequestException`) are mapped to HTTP status codes `404`, `409`, and `400` respectively.
- All errors are formatted as RFC 9457 `ProblemDetails` for consistency.
- Step order uniqueness collisions (SQL Server error 2601/2627) and foreign key deletion violations (SQL Server error 547) inherit from `ConflictException` and return `409 Conflict`.
- Unhandled or unexpected exceptions return `500 Internal Server Error` with a generic message to prevent leaking internal stack traces.

### 2. Execution Engine (`WorkflowExecutor`)
- **Pre-flight Validation**:
  - Missing workflow: `NotFoundException` (404)
  - Workflow status not `Active`: `ConflictException` (409)
  - Workflow has no steps: `BadRequestException` (400)
- **Execution Lifecycle & Guarantee**:
  - An initial `WorkflowExecution` record with status `Running` is persisted immediately to establish an `Id`.
  - Sequential step execution ordered by `Order` ascending.
  - Each step creates a `StepExecutionLog` persisted immediately for real-time tracking.
  - **Guaranteed Finalization**: Handled in a `finally` block ensuring no execution is left in the `Running` state even if an unexpected failure or DB issue occurs.
- **Retry Mechanism**:
  - Up to 3 attempts with configurable exponential backoff delays (`[1000ms, 2000ms]`).
  - Retries are only performed when `ActionExecutionResult.IsRetryable` is `true`.
  - Missing action handlers or unhandled exceptions are treated as non-retryable failures.
- **Cancellation & Timeouts**:
  - A dedicated `CancellationTokenSource` with a 60-second limit guards against handler deadlocks.
  - Does **not** pass `HttpContext.RequestAborted` directly to the workflow execution loop, ensuring atomic execution recording even if the client disconnects.
- **Data Protection**:
  - Step `Output` is truncated to 2000 characters to match database column limits.
  - Sensitive configurations and credentials are excluded from execution logs and outputs.

---

## API Endpoints

### Workflows
| Method | Route | Description |
|---|---|---|
| `POST` | `/api/workflows` | Create a new workflow (Draft status by default) |
| `GET` | `/api/workflows` | List all workflows |
| `GET` | `/api/workflows/{id}` | Get workflow by ID |
| `PUT` | `/api/workflows/{id}` | Update workflow name and status (`0 = Draft`, `1 = Active`) |
| `DELETE` | `/api/workflows/{id}` | Delete workflow and cascade-delete steps & triggers |
| `POST` | `/api/workflows/{id}/run` | Execute workflow synchronously (`TriggerType.Manual`) |

### Steps
| Method | Route | Description |
|---|---|---|
| `POST` | `/api/workflows/{workflowId}/steps` | Add a step to a workflow |
| `GET` | `/api/workflows/{workflowId}/steps` | Get all steps for a workflow ordered by `Order` |
| `GET` | `/api/workflows/{workflowId}/steps/{id}` | Get step by ID |
| `PUT` | `/api/workflows/{workflowId}/steps/{id}` | Update step configuration and order |
| `DELETE` | `/api/workflows/{workflowId}/steps/{id}` | Delete step (prevents deletion if execution logs exist -> `409 Conflict`) |

### Executions
| Method | Route | Description |
|---|---|---|
| `GET` | `/api/workflows/{workflowId}/executions` | Get paginated executions for a workflow (newest first, `page` & `pageSize`) |
| `GET` | `/api/executions/{id}` | Get execution details with chronological step logs |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- SQL Server Express / LocalDB

### Configuration
Update the database connection string in `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FlowForgeDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Apply Migrations
```bash
dotnet ef database update
```

### Run the Application
```bash
dotnet run
```

### Testing the Endpoints
Open and run requests in `FlowForge.http` using Visual Studio, VS Code REST Client extension, or JetBrains Rider. The file covers all execution engine scenarios, happy paths, and edge cases.