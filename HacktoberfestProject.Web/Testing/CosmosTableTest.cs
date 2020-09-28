﻿using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HacktoberfestProject.Web.Testing
{
	public class CosmosTableTest
	{
		private ITableContext _context;
		private string _testUsername = "TestUser";
		private UserEntity _entityToRemove;

		public CosmosTableTest(ITableContext context)
		{
			_context = context;
		}

		public void TestInsert()
		{
			PrEntity prEntity = new PrEntity(3, "http://test");
			RepositoryEntity repositoryEntity = new RepositoryEntity("test", "test", new List<PrEntity>{ prEntity });
			UserEntity userEntity = new UserEntity(_testUsername, new List<RepositoryEntity> { repositoryEntity });

			var insertedEntity = _context.InsertOrMergeEntityAsync(userEntity).Result;
		}

		public void TestRetrive()
		{
			UserEntity userEntity = new UserEntity(_testUsername);

			_entityToRemove = _context.RetrieveEnitityAsync(userEntity).Result;
		}

		public void TestDelete()
		{
			_context.DeleteEntity(_entityToRemove);
		}

		public static void RunTableStorageTests(IServiceCollection services)
		{
			IServiceProvider sp = services.BuildServiceProvider();

			var ctt = new CosmosTableTest(sp.GetService<ITableContext>());
			ctt.TestInsert();
			ctt.TestRetrive();
			ctt.TestDelete();
		}
	}
}
