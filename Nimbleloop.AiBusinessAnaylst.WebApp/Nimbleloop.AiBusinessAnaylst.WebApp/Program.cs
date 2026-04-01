using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Nimbleloop.AiBusinessAnaylst.WebApp.Client.Pages;
using Nimbleloop.AiBusinessAnaylst.WebApp.Components;
using Nimbleloop.AiBusinessAnaylst.WebApp.Components.Account;
using Nimbleloop.AiBusinessAnaylst.WebApp.Data;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveWebAssemblyComponents()
		.AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddAuthentication(options =>
		{
			options.DefaultScheme = IdentityConstants.ApplicationScheme;
			options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
		})
		.AddIdentityCookies();
builder.Services.AddAuthorization();

// MongoDB / Azure Cosmos DB (Mongo API)
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") ?? throw new InvalidOperationException("Connection string 'MongoDb' not found.");
var mongoDatabaseName = builder.Configuration["MongoDb:DatabaseName"] ?? "AiBusinessAnalyst";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseMongoDB(mongoConnectionString, mongoDatabaseName));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
		{
			options.SignIn.RequireConfirmedAccount = true;
			options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddSignInManager()
		.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// OpenAI
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
if (!string.IsNullOrEmpty(openAiApiKey))
{
	builder.Services.AddSingleton(new OpenAIClient(openAiApiKey));
}

// Anthropic
var anthropicApiKey = builder.Configuration["Anthropic:ApiKey"];
if (!string.IsNullOrEmpty(anthropicApiKey))
{
	builder.Services.AddSingleton(new Anthropic.SDK.AnthropicClient(new Anthropic.SDK.APIAuthentication(anthropicApiKey)));
}

// ClickUp API HttpClient
builder.Services.AddHttpClient("ClickUp", client =>
{
	var clickUpBaseUrl = builder.Configuration["ClickUp:BaseUrl"] ?? "https://api.clickup.com/api/v2";
	client.BaseAddress = new Uri(clickUpBaseUrl);

	var clickUpApiKey = builder.Configuration["ClickUp:ApiKey"];
	if (!string.IsNullOrEmpty(clickUpApiKey))
	{
		client.DefaultRequestHeaders.Add("Authorization", clickUpApiKey);
	}
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
		.AddInteractiveWebAssemblyRenderMode()
		.AddAdditionalAssemblies(typeof(Nimbleloop.AiBusinessAnaylst.WebApp.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
