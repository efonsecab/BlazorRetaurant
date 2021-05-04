﻿using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Server.CustomProviders;
using BlazorRestaurant.Shared.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of orders management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        /// <summary>
        /// Creates a new instance of <see cref="OrderController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="httpContextAccessor"></param>
        public OrderController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
            this.HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds a new order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> AddOrder(OrderModel orderModel)
        {
            var claims = this.HttpContextAccessor.HttpContext.User.Claims;
            var oidc = claims.Where(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").SingleOrDefault();
            var userEntity = await this.BlazorRestaurantDbContext
                .ApplicationUser.SingleOrDefaultAsync(p => p.AzureAdB2cobjectId.ToString() == oidc.Value);
            Order orderEntity = this.Mapper.Map<OrderModel, Order>(orderModel);
            orderEntity.ApplicationUserId = userEntity.ApplicationUserId;
            await this.BlazorRestaurantDbContext.Order.AddAsync(orderEntity);
            foreach (var singleLine in orderEntity.OrderDetail)
            {
                this.BlazorRestaurantDbContext.Entry<Product>(singleLine.Product).State = EntityState.Unchanged;
            }
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
