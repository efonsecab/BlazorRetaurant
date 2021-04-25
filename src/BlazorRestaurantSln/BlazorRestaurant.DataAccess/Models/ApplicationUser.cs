﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BlazorRestaurant.DataAccess.Models
{
    [Index(nameof(AzureAdB2cobjectId), Name = "UI_ApplicationUser_AzureAdB2CObjectId", IsUnique = true)]
    public partial class ApplicationUser
    {
        [Key]
        public long ApplicationUserId { get; set; }
        [Required]
        [StringLength(150)]
        public string FullName { get; set; }
        [Required]
        [StringLength(150)]
        public string EmailAddress { get; set; }
        public DateTimeOffset LastLogIn { get; set; }
        [Column("AzureAdB2CObjectId")]
        public Guid AzureAdB2cobjectId { get; set; }

        [InverseProperty("ApplicationUser")]
        public virtual ApplicationUserRole ApplicationUserRole { get; set; }
    }
}