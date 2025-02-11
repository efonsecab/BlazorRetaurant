﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.Shared.Promos;
using System.Net.Http.Json;
using BlazorRestaurant.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class PromotionControllerTests : TestsBase
    {
        private static readonly PromotionModel TestPromotionModel = new()
        {
            Name = "TESTPROMOTION",
            ImageUrl = "TESTPROMOTIONURL",
            Description = "TEStPROMOTIONDESCRIPTION"
        };

        [ClassCleanup]
        public static async Task CleanTests()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testEntity = await blazorRestaurantDbContext.Promotion.Where(p => p.Name == TestPromotionModel.Name).SingleAsync();
            blazorRestaurantDbContext.Promotion.Remove(testEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
        }

        [TestMethod()]
        public async Task AddPromotionTest()
        {
            var authorizedHttpClient = await base .CreateAuthorizedClientAsync(Role.Admin);
            var response = await authorizedHttpClient.PostAsJsonAsync("api/Promotion/AddPromotion", TestPromotionModel);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }
        }

        [TestMethod()]
        public async Task ListPromotionsTest()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            blazorRestaurantDbContext.Promotion.Add(new DataAccess.Models.Promotion()
            {
                Name = TestPromotionModel.Name,
                Description = TestPromotionModel.Description,
                ImageUrl = TestPromotionModel.ImageUrl
            });
            await blazorRestaurantDbContext.SaveChangesAsync();
            var authorizedHttpClient = await base .CreateAuthorizedClientAsync(Role.User);
            var response = await authorizedHttpClient.GetFromJsonAsync<PromotionModel[]>("api/Promotion/ListPromotions");
            Assert.IsTrue(response.Length > 0);
        }

        [TestMethod()]
        public async Task DeletePromotionTest()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testPromotionEntity = await blazorRestaurantDbContext.Promotion
                .Where(p => p.Name == TestPromotionModel.Name).FirstOrDefaultAsync();
            Assert.IsNotNull(testPromotionEntity);
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync(Role.Admin);
            var response = await authorizedHttpClient.DeleteAsync($"api/Promotion/" +
                $"DeletePromotion?promotionId={testPromotionEntity.PromotionId}");
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }
        }
    }
}