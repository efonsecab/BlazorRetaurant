﻿using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Products;
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
    /// In charge of products management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ProductController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        public ProductController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Add a new product
        /// <paramref name="productModel"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<IActionResult> AddProduct(ProductModel productModel)
        {
            var productEntity = await this.BlazorRestaurantDbContext.Product
                .Where(p => p.Name == productModel.Name).SingleOrDefaultAsync();
            if (productEntity != null)
                throw new Exception($"There is already a product named: {productModel.Name}");
            else
            {
                productEntity = this.Mapper.Map<ProductModel, Product>(productModel);
                await this.BlazorRestaurantDbContext.Product.AddAsync(productEntity);
                await this.BlazorRestaurantDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        /// <summary>
        /// Edits the specified product
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Constants.Roles.Admin)]
        public async Task<IActionResult> EditProduct(ProductModel productModel)
        {
            var productEntity = await this.BlazorRestaurantDbContext.Product
                .Where(p => p.Name == productModel.Name).AsNoTracking().SingleOrDefaultAsync();
            if (productEntity == null)
                throw new Exception($"There is no product with Id: {productModel.ProductId}");
            else
            {
                productEntity = this.Mapper.Map<ProductModel, Product>(productModel);
                this.BlazorRestaurantDbContext.Product.Update(productEntity);
                await this.BlazorRestaurantDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        /// <summary>
        /// Lists all of the Product Types
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ProductTypeModel[]> ListProductTypes()
        {
            return await this.BlazorRestaurantDbContext.ProductType
                .Select(p => this.Mapper.Map<ProductType, ProductTypeModel>(p)).ToArrayAsync();
        }

        /// <summary>
        /// Returns a list of all the products
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ProductModel[]> ListProducts()
        {
            return await this.BlazorRestaurantDbContext
                .Product
                .OrderBy(p => p.Name)
                .Select(p => this.Mapper.Map<Product, ProductModel>(p)).ToArrayAsync();
        }

        /// <summary>
        /// Gets a product by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ProductModel> GetProductById(int productId)
        {
            var result = await this.BlazorRestaurantDbContext.Product.Where(p => p.ProductId == productId)
                .Select(p=>this.Mapper.Map<Product, ProductModel>(p))
                .SingleOrDefaultAsync();
            if (result == null)
                throw new Exception($"There is no product with Id: {productId}");
            else
                return result;
        }

        /// <summary>
        /// Deletes the product with the specified Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var productEntity = await this.BlazorRestaurantDbContext.Product.SingleOrDefaultAsync(p => p.ProductId == productId);
            if (productEntity == null)
                throw new Exception($"There is not product with Id: {productId}");
            else
            {
                this.BlazorRestaurantDbContext.Product.Remove(productEntity);
                await this.BlazorRestaurantDbContext.SaveChangesAsync();
                return Ok();
            }

        }
    }
}
