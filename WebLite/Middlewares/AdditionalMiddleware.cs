using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Serialization;
using SqlLite;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebLite.Configurations;
using WebLite.Exceptions;
using WebLite.Filters;
using WebLite.Tokens;
using CacheLite;

namespace WebLite.Middlewares
{
    public static class AdditionalMiddleware
    {
        /// <summary>
        /// 添加数据库功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseMiddleware(this IServiceCollection services, String connectionString)
        {
            services.AddScoped((service) =>
            {
                return new Database(new MySqlConnection(connectionString));
            });

            return services;
        }

        /// <summary>
        /// 添加cookie功能
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCookieMiddleware(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            return services;
        }

        /// <summary>
        /// 添加session功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static IServiceCollection AddSessionMiddleware(this IServiceCollection services, TimeSpan expires)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = expires;
                options.CookieHttpOnly = true;
            });
            return services;
        }

        /// <summary>
        /// 生产阶段的过滤器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddProductionFilters(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelValidationFilterAttribute());
                options.Filters.Add(new ExceptionHandlerFilterAttribute());
                options.Filters.Add(typeof(GlobalException));
            });
            return services;
        }

        /// <summary>
        /// 开发阶段的过滤器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDevelopingFilters(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelValidationFilterAttribute());
            });
            return services;
        }

        /// <summary>
        /// 添加Json支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDevelopingJsonProvider(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelValidationFilterAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(option =>
            {
                option.SerializerSettings.ContractResolver = new DefaultContractResolver();
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            return services;
        }

        /// <summary>
        /// 添加Json支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddProductionJsonProvider(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelValidationFilterAttribute());
                options.Filters.Add(new ExceptionHandlerFilterAttribute());
                options.Filters.Add(typeof(GlobalException));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(option =>
            {
                option.SerializerSettings.ContractResolver = new DefaultContractResolver();
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            return services;
        }

        /// <summary>
        /// 添加Swagger支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="swaggerName">swagger名称</param>
        /// <param name="version">版本</param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <param name="contactName">联系人</param>
        /// <param name="contactMail">邮箱</param>
        /// <param name="contactUrl">地址</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerMiddleware(this IServiceCollection services, string swaggerName = "v1", string version = "v1.0.0", string title = "Core API", string description = "API Document", string contactName = null, string contactMail = null, string contactUrl = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerName, new Info
                {
                    Version = version,    //版本
                    Title = title,  //标题
                    Description = description,  //描述文字
                    TermsOfService = "None",  //服务项
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = contactName, Email = contactMail, Url = contactUrl }    //联系人信息
                });

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, $"{swaggerName}.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { swaggerName, new string[] { } } };
                c.AddSecurityRequirement(security);

                c.AddSecurityDefinition(swaggerName, new ApiKeyScheme
                {
                    Description = "JWT Authorization Info: please fill in the input blank with ' Bearer {token} ' ",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "APISecret"
                });

            });

            return services;
        }

        /// <summary>
        /// 使用中间件（swagger一般用于开发期间的使用）
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerName"></param>
        /// <param name="description"></param>
        public static void UseSwaggerMiddleware(this IApplicationBuilder app, string swaggerName = "v1", string description = "API Document V1.0")
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{swaggerName}/swagger.json", description);
            });
        }

        /// <summary>
        /// 添加授权验证
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationMiddleware(this IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(o =>
          {
              o.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,//是否验证Issuer
                  ValidateAudience = false,//是否验证Audience 
                  ValidateIssuerSigningKey = true,//是否验证IssuerSigningKey 
                  ValidIssuer = ConfigurationManager.GetSection("JwtToken:issuer"),
                  ValidateLifetime = true,
                  IssuerSigningKey = JwtTokenHelper.credit
              };
          });
            return services;
        }

        /// <summary>
        /// 添加Aop支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider AddAspectCoreMiddleware(this IServiceCollection services)
        {
            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<LogAspect>(Predicates.ForService("*Service"), Predicates.ForService("*Repository"));
            });

            return services.BuildAspectInjectorProvider();
        }
    }
}
