using ApiTransation.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiTransation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Configuração dos serviços, como Entity Framework, autenticação, etc.
            services.AddMvc();
            services.AddControllers();// Exemplo de configuração do MVC
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configuração de tratamento de erros em ambiente de produção
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Configuração de middleware, roteamento, autenticação, etc.
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
