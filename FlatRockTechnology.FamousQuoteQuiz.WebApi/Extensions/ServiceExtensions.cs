using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations.QuestionServices;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations.UserQuestionsHistory;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FlatRockTechnology.FamousQuoteQuiz.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDBContext(this IServiceCollection services)
        {
            services.AddDbContext<QuoteQuizContext>(
                  x => x.UseSqlServer("Data Source=localhost;Initial Catalog=FlatRockTechnology.FamousQuoteQuiz;Integrated Security=True")
                  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                  ServiceLifetime.Transient); // Adding DB Context To The Container
        }

        public static void ConfigureServicesInjections(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Question>, Repository<Question>>();
            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IRepository<Answer>, Repository<Answer>>();
            services.AddTransient<IRepository<UserAnsweredQuestion>, Repository<UserAnsweredQuestion>>();

            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IQuestionsHistoryService, QuestionsHistoryService>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserQuestionsHistoryService, UserQuestionsHistoryService>();

            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<UserManager<User>>();
        }


        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(q =>
            {
                q.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<QuoteQuizContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var key = jwtSettings.GetSection("Key").Value;

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.GetSection("Key").Value))
                };
            });
        }
    }
}
