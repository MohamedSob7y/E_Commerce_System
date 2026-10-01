using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            //Generate Dictionary has All Errors as Key+ Value ممكن الParameter الواحد يحتوى على List Of errors
            var errors = actionContext.ModelState.Where(X => X.Value.Errors.Count > 0)
            .ToDictionary(
                X => X.Key,
             X => X.Value.Errors.Select(e => e.ErrorMessage).ToArray());//لازم errors >0 عشان يبقى list
            var problem = new ProblemDetails
            {
                Title = " Validation Errors",
                Detail = "One Or More Validation Error Occured",
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                               {
                                   {"Errors" ,errors}
                               },
            };
            return new BadRequestObjectResult(problem);
        }
    }
}
