﻿using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using War.Contestants;
using War.Contestants.Sql;
using War.Matches;
using War.Matches.Factories;
using War.Matches.Sql;
using War.RankingServices;
using War.Users;
using War.Users.Sql;
using War.Votes;
using War.Votes.Sql;
using War.Wars;
using War.Wars.Sql;
using WarApi.Mappers;

namespace WarApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            ConfigureDependencyInjection(config);
            ConfigureRouting(config);
        }

        private static void ConfigureRouting(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureDependencyInjection(HttpConfiguration config)
        {
            var sqlServerConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["WarDb"].ConnectionString;
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<Mapper>().As<IMapper>();

            builder.RegisterType<SumDistinctWinsRankingStrategy>().As<IRankingService>();
            builder.RegisterType<RandomMatchStrategy>().As<IMatchFactory>();
            builder.Register(c => new ContestantRepository(sqlServerConnectionString)).As<IContestantRepository>();
            builder.Register(c => new MatchRepository(sqlServerConnectionString)).As<IMatchRepository>();
            builder.Register(c => new WarRepository(sqlServerConnectionString)).As<IWarRepository>();
            builder.Register(c => new UserRepository(sqlServerConnectionString)).As<IUserRepository>();
            builder.Register(c => new VoteRepository(sqlServerConnectionString)).As<IVoteRepository>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
