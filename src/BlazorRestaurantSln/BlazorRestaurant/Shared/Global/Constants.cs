﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Global
{
    public class Constants
    {
        public class Claims
        {
            public const string ObjectIdentifier = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        }
        public class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public class AdminPagesRoutes
        {
            public const string AdminIndex = "/Admin/Index";
            public const string ListImages = "/Admin/Images/List";
            public const string ManageImages = "/Admin/Images/Manage";
            public const string ListPromos = "/Admin/Promos/List";
            public const string AddPromo = "/Admin/Promos/Manage";
            public const string EditPromo = "/Admin/Promos/Manage/{PromoId:long}";
            public const string ErrorsLogPowerBI = "/Admin/Errors/ErrorsPowerBI";
            public const string ErrorLog = "/Admin/Errors/ErrorLog";
            public const string AddProduct = "/Admin/Products/Manage";
            public const string EditProduct = "/Admin/Products/Manage/{ProductId:int}";
            public const string ListProducts = "/Admin/Products/List";
            public const string ListOrders = "/Admin/Orders/List";
        }

        public class UserPagesRoutes
        {
            public const string UserIndex = "/User/Index";
            public const string AddOrder = "/User/Orders/Manage";
            public const string EditOrder = "/User/Orders/Manage/{OrderId:long}";
            public const string ListOrders = "/User/Orders/List";
        }
    }
}
