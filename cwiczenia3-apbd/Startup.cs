using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cwiczenia3_apbd.DAL;
using cwiczenia3_apbd.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace cwiczenia3_apbd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDbService,MockDbService>();
            services.AddTransient<IStudentDbService, SqlServerDbService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            /////////////////////
            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie poda³eœ indeksu");
                    return;
                }

                string index = context.Request.Headers["Index"].ToString();

                //check in db
                using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
                using (var com = new SqlCommand())
                {


                    com.Connection = con;
                    com.Parameters.AddWithValue("Index", context.Request.Headers["Index"]);
                    com.CommandText = "SELECT count(*) as count FROM Student WHERE IndexNumber = @Index;";


                    con.Open();
                    var dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        int istn = (int)dr["count"];

                        if (istn == 0)
                        {
                            await context.Response.WriteAsync("B³êdny index!");
                            return;
                        }


                    }

                }

                    await next();
            });

            /////////////////////////////////
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
