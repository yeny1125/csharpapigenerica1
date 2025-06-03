//Program.cs 

using System; // Agrega esta línea para usar TimeSpan 

using Microsoft.AspNetCore.Builder; // Importa el espacio de nombres necesario para construir y configurar la aplicación web. 

using Microsoft.Extensions.DependencyInjection; // Importa el espacio de nombres necesario para configurar los servicios de la aplicación. 

using Microsoft.Extensions.Hosting; // Importa el espacio de nombres necesario para trabajar con diferentes entornos (desarrollo, producción, etc.). 

using csharpapigenerica.Services; // Importa los servicios personalizados que se utilizarán en la aplicación. 

using Microsoft.OpenApi.Models; // 🔹 Importa el espacio de nombres necesario para habilitar Swagger. 

 

var builder = WebApplication.CreateBuilder(args); // Crea un constructor para configurar la aplicación web ASP.NET Core. 

 

builder.Services.AddControllers(); // Agrega soporte para controladores MVC, permitiendo manejar solicitudes HTTP a través de acciones en los controladores. 

builder.Services.AddSingleton<ControlConexion>(); // Registra el servicio ControlConexion como singleton, asegurando que haya una única instancia compartida en toda la aplicación. 

builder.Services.AddSingleton<TokenService>(); // Registra el servicio TokenService como singleton, asegurando una única instancia compartida en toda la aplicación. 

 

builder.Services.AddCors(options => // Configura CORS (Cross-Origin Resource Sharing) para la aplicación. 

{dotgit  

    options.AddPolicy("AllowAllOrigins", // Define una política de CORS llamada "AllowAllOrigins". 

        builder => builder.AllowAnyOrigin() // Permite solicitudes desde cualquier origen (dominio). 

                          .AllowAnyMethod() // Permite cualquier método HTTP (GET, POST, etc.). 

                          .AllowAnyHeader()); // Permite cualquier encabezado en las solicitudes. 

}); 

 

builder.Services.AddDistributedMemoryCache(); // Agrega un proveedor de caché distribuida en memoria para almacenar datos de sesión. 

builder.Services.AddSession(options => // Configura el servicio de sesión para la aplicación. 

{ 

    options.IdleTimeout = TimeSpan.FromMinutes(30); // Establece el tiempo de inactividad de la sesión a 30 minutos. 

    options.Cookie.HttpOnly = true; // Configura la cookie de sesión como HTTPOnly para mayor seguridad. 

    options.Cookie.IsEssential = true; // Marca la cookie de sesión como esencial, necesaria para el funcionamiento de la aplicación. 

}); 

 

// 🔹 Habilitar Swagger 

builder.Services.AddEndpointsApiExplorer(); 

builder.Services.AddSwaggerGen(c => 

{ 

    c.SwaggerDoc("v1", new OpenApiInfo 

    { 

        Title = "Api Genérica C#",  

        Version = "v1", 

        Description = "API de prueba con ASP.NET Core y Swagger", 

        Contact = new OpenApiContact 

        { 

            Name = "Soporte API", 

            Email = "soporte@miapi.com", 

            Url = new Uri("https://miapi.com/contacto") 

        } 

    }); 

}); 

 

var app = builder.Build(); // Construye la aplicación con las configuraciones especificadas anteriormente. 

 

if (app.Environment.IsDevelopment()) // Verifica si la aplicación está en el entorno de desarrollo. 

{ 

    app.UseDeveloperExceptionPage(); // Habilita una página de excepción detallada, útil para depurar errores durante el desarrollo. 

 

    // 🔹 Middleware de Swagger 

    app.UseSwagger(); 

    app.UseSwaggerUI(c => 

    { 

        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Genérica C#"); 

        //c.RoutePrefix = string.Empty; // Hace que Swagger esté disponible en la raíz (http://localhost:5266/) 

        c.RoutePrefix = "swagger"; //  Swagger estará en http://localhost:5266/swagger 

    }); 

} 

 

app.UseHttpsRedirection(); // Fuerza la redirección de las solicitudes HTTP a HTTPS para mejorar la seguridad. 

 

app.UseCors("AllowAllOrigins"); // Aplica la política de CORS que permite solicitudes desde cualquier origen. 

app.UseSession(); // Habilita el soporte de sesiones en el middleware de la aplicación. 

app.UseAuthorization(); // Habilita el middleware de autorización, necesario para proteger rutas que requieren autenticación o autorización. 

 

app.MapControllers(); // Configura las rutas de los controladores para manejar las solicitudes HTTP. 

 

app.Run(); // Inicia la aplicación y comienza a escuchar las solicitudes entrantes. 

 

 