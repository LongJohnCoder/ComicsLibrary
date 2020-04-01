﻿using ComicsLibrary.Data;
using PostDeploymentTools;
using System;

namespace ComicsLibrary.PostDeployment
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
                throw new Exception("Incorrect number of arguments");

            var appName = args[0];
            var connectionString = args[1];
            var databaseName = args[2];
            var schemaName = args[3];

            var postDeploymentService = new PostDeploymentService(appName, connectionString, databaseName, schemaName);
            postDeploymentService.UpdateDatabase(() => new ApplicationDbContext(connectionString, schemaName));

            postDeploymentService.CreateTaskUser();
            postDeploymentService.GrantTaskPermission("SELECT, INSERT, UPDATE, DELETE");

            postDeploymentService.CreateApiUser();
            postDeploymentService.GrantApiPermission("SELECT, INSERT, UPDATE, DELETE");
		}
    }
}