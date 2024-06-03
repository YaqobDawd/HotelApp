using HotelAppC;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Business.IRepo;
using Business.Repo;
using HotelAppC.Service.IService;
using HotelAppC.Service;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });

builder.Services.AddAuthorizationCore();
builder.Services.AddLocalization();
builder.Services.AddScoped<AuthenticationStateProvider,AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IHotelRoomService,HotelRoomService>();
builder.Services.AddScoped<IHotelAmenityService, HotelAmenityService>();
builder.Services.AddScoped<IRoomOrderService, RoomOrderService>();
builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
