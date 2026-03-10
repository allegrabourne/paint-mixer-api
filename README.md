\# Paint Mixer API



\## Overview



This project implements a REST API for interacting with a paint mixing device. The API allows clients to submit paint mixing jobs, query the status of existing jobs, and cancel jobs that are currently queued or running.



The implementation follows a layered architecture separating domain logic, service orchestration, and HTTP concerns. The supplied paint mixer emulator is used as the device interface and is wrapped behind an abstraction to keep infrastructure concerns isolated from the application logic.



The solution is implemented using ASP.NET Core 8 with minimal APIs through FastEndpoints.



\## Architecture



The solution is divided into four projects.



PaintMixer.Domain  

Contains core domain objects and enumerations. These represent the conceptual model of the system and enforce domain invariants.



PaintMixer.Service  

Contains the service layer responsible for orchestration and interaction with the paint mixer device. This layer exposes application behaviour through interfaces and isolates the device implementation behind an adapter.



PaintMixer.Api  

Contains the HTTP API implementation. This layer defines endpoints, request and response models, validation rules, diagnostics, and middleware.



PaintMixer.Test  

Contains unit tests for the service layer and middleware using xUnit and Moq.



The overall dependency flow is:



API > Service > Device abstraction > Emulator



This ensures the HTTP layer remains thin and that device specific concerns do not leak into higher layers.



\## Technologies



ASP.NET Core 8  

FastEndpoints for endpoint definition  

FluentValidation for request validation  

xUnit for unit testing  

Moq for mocking dependencies



\## API Endpoints



Submit a paint mixing job



POST /api/paint-jobs



Accepts dye quantities and submits a job to the mixer.



Example request:



{

&nbsp; "red": 20,

&nbsp; "blue": 10,

&nbsp; "yellow": 0,

&nbsp; "white": 30,

&nbsp; "black": 0,

&nbsp; "green": 10

}



Example response:



{

&nbsp; "jobCode": 42

}



Query job status



GET /api/paint-jobs/{jobCode}



Returns the current status of the job.



Example response:



{

&nbsp; "jobCode": 42,

&nbsp; "status": "Running"

}



Cancel a job



DELETE /api/paint-jobs/{jobCode}



Cancels a job that is queued or running.



Returns HTTP 204 No Content if the cancellation succeeds.



\## Validation



Request validation is implemented using FluentValidation.



Validation rules include:



Each dye amount must be between 0 and 100  

The combined dye total must not exceed 100



Domain objects also enforce these constraints through constructor validation to guarantee that domain instances are always in a valid state.



\## Diagnostics and error handling



The API provides human readable diagnostic responses through a dedicated middleware component.



Error responses include:



A diagnostic code  

A status message  

A correlation identifier derived from the HTTP request



Example error response:



{

&nbsp; "code": "invalid\_paint\_mix",

&nbsp; "message": "The paint mix is invalid. The total dye amount must not exceed 100.",

&nbsp; "correlationId": "00-1a2b3c..."

}



Diagnostic messages are provided through an abstraction called IDiagnosticMessageProvider. This centralises user facing messages and allows future localisation support.



\## Localisation support



The diagnostic message provider is designed to support localisation. Messages are selected based on the Accept-Language request header and default to English when no language preference is provided.



This approach allows the API to support multiple languages without coupling localisation logic directly to endpoint implementations.



\## Paint mixer behaviour



The supplied device emulator is used to simulate the paint mixer hardware. The emulator enforces several constraints including:



A maximum of 32 active jobs  

Validation of dye values between 0 and 100  

A total dye amount not exceeding 100



When the queue capacity is exceeded or a submission is invalid, the emulator rejects the request and the service layer returns an appropriate diagnostic response.



\## Device abstraction



The paint mixer emulator is wrapped behind the IPaintMixerDevice interface through an adapter implementation. This ensures that infrastructure concerns remain isolated from the rest of the application.



Only the adapter is registered with dependency injection. The emulator itself remains internal to the service layer.



In a production implementation the adapter would also forward asynchronous disposal of the underlying device resource. This was intentionally left out to keep the task focused on API behaviour rather than lifecycle management.



\## Testing



Unit tests are implemented using xUnit and Moq.



Tests focus on the service layer and diagnostic middleware.



Test naming follows a Given When Then structure to clearly express the behaviour being verified.



Examples include:



GivenValidPaintMix\_WhenSubmitJobAsync\_ThenReturnsJobCode  

GivenUnknownJobCode\_WhenGetStatusAsync\_ThenReturnsUnknown  

GivenArgumentException\_WhenInvokeAsyncCalled\_ThenReturnsBadRequestErrorResponse



\## Running the project



Prerequisites



.NET 8 SDK



Build the solution:



dotnet build



Run the API:



dotnet run --project PaintMixer.Api



Run the tests:



dotnet test



\## Future improvements



Possible improvements for a production system include:



More granular error handling for device level failures  

Additional localisation through resource files  

Integration tests for endpoint behaviour  

Structured logging and distributed tracing integration  

Explicit disposal of the device adapter when the application shuts down

