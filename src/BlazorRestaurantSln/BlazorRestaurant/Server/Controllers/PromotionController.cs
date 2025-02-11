﻿using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Promos;
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
    /// In charge of promotions management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PromotionController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="PromotionController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        public PromotionController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Adds a new promotion to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task AddPromotion(PromotionModel model)
        {
            var entity = Mapper.Map<PromotionModel, Promotion>(model);
            await this.BlazorRestaurantDbContext.AddAsync(entity);
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Lists all Promotions
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<PromotionModel[]> ListPromotions()
        {
            var result = await this.BlazorRestaurantDbContext.Promotion.Select(p =>
            this.Mapper.Map<Promotion, PromotionModel>(p)).ToArrayAsync();
            return result;
        }

        /// <summary>
        /// Delete the promotion with the specified id
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<IActionResult> DeletePromotion(long promotionId)
        {
            var promotionEntity = 
                await this.BlazorRestaurantDbContext.Promotion.Where(p => p.PromotionId == promotionId).SingleOrDefaultAsync();
            if (promotionEntity == null)
                throw new Exception($"Promotion with id: {promotionId} does not exist");
            else
            {
                this.BlazorRestaurantDbContext.Promotion.Remove(promotionEntity);
                await this.BlazorRestaurantDbContext.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
