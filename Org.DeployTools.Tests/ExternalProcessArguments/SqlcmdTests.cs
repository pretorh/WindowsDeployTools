﻿using System.Data.SqlClient;
using NUnit.Framework;
using Org.DeployTools.Shared.ExternalProcessArgumentBuilder;

namespace Org.DeployTools.Tests.ExternalProcessArguments
{
    internal class SqlcmdTests
    {
        private SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        [SetUp]
        public void Setup()
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "s",
                InitialCatalog = "d",
            };
        }

        [Test]
        public void IntegratedSecurity()
        {
            _sqlConnectionStringBuilder.IntegratedSecurity = true;

            var builder = SqlcmdArgumentsBuilder.Build(_sqlConnectionStringBuilder);
            var arguments = builder.ToString();

            Assert.IsTrue(arguments.Contains("-S \"s\" -d \"d\""), "server and database details missing in " + arguments);
            Assert.IsFalse(arguments.Contains("-U \"user\" -P \"pass\""), "user and password details present in " + arguments);
        }

        [Test]
        public void SqlAuthentication()
        {
            _sqlConnectionStringBuilder.IntegratedSecurity = false;
            _sqlConnectionStringBuilder.UserID = "user";
            _sqlConnectionStringBuilder.Password = "pass";

            var builder = SqlcmdArgumentsBuilder.Build(_sqlConnectionStringBuilder);
            var arguments = builder.ToString();

            Assert.IsTrue(arguments.Contains("-S \"s\" -d \"d\""), "server and database details missing in " + arguments);
            Assert.IsTrue(arguments.Contains("-U \"user\" -P \"pass\""), "user and password details missing in " + arguments);
        }
    }
}
