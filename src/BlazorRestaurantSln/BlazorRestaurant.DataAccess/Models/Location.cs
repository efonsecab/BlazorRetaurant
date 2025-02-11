﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BlazorRestaurant.DataAccess.Models
{
    [Table("Location", Schema = "profile")]
    [Index(nameof(Name), Name = "UI_Location_Name", IsUnique = true)]
    public partial class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [StringLength(1000)]
        public string FreeFormAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Required]
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset RowCreationDateTime { get; set; }
        [Required]
        [StringLength(256)]
        public string RowCreationUser { get; set; }
        [Required]
        [StringLength(250)]
        public string SourceApplication { get; set; }
        [Required]
        [Column("OriginatorIPAddress")]
        [StringLength(100)]
        public string OriginatorIpaddress { get; set; }
    }
}