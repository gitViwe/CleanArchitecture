namespace WebAPI.Extensions
{
    /// <summary>
    /// The implementation of the Application builder services in the <see cref="Program"/> class
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application to use Open API / Swagger UI
        /// </summary>
        internal static void UseWebAPISwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // specify the swagger endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1.0");
                options.DisplayRequestDuration();
            });
        }
    }
}
