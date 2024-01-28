using Common;
using Common.Solutions.Data.MainDB;
using Common.Solutions.Data.MainDB.Configuration;
using Core;
using Experts;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();




// 1
//TransientFaultHandlingOptions options = new();
//builder
//    .Configuration
//    .GetSection(TransientFaultHandlingOptions.SectionName)
//    .Bind(options);

// 2
//var options = builder
//    .Configuration
//    .GetSection(TransientFaultHandlingOptions.SectionName)
//    .Get<TransientFaultHandlingOptions>();

// 3
//builder
//    .Services
//    .Configure<TransientFaultHandlingOptions>(
//        builder.Configuration.GetSection(
//            TransientFaultHandlingOptions.SectionName));

// 4
//builder
//    .Services
//    .AddOptionsWithValidateOnStart<TransientFaultHandlingOptions>()
//    .Bind(config.GetSection(TransientFaultHandlingOptions.SectionName)) 
//    .ValidateDataAnnotations()
//     //.Configure(options => { })
//     .Validate(options =>
//     {
//         if (options.Enabled) {
//             return options.AutoRetryDelay > TimeSpan.Zero;
//         }

//         return true;
//     }, "AutoRetryDelay must be set if Enabeled.");
//;

//// 5 
//builder
//    .Services
//    .AddMyLibraryService2();

//builder.Services
//    .AddCore()
//    .AddCommon()
//    .AddExperts()
//    ;

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
